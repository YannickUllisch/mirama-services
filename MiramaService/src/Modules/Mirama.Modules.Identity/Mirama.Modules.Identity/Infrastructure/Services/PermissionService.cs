using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Mirama.Modules.Identity.Domain.Aggregates.Policy;
using Mirama.Modules.Identity.Domain.Aggregates.Role;
using Mirama.Modules.Identity.Domain.Aggregates.User;
using Mirama.Modules.Identity.Infrastructure.Persistence;
using Mirama.SharedKernel.Abstractions.Permissions;
using Mirama.SharedKernel.Models.Permissions;

namespace Mirama.Modules.Identity.Infrastructure.Services;

internal sealed class PermissionService(
    IdentityDbContext dbContext,
    IMemoryCache cache,
    IProjectRoleProvider projectRoleProvider) : IPermissionService
{
    private static readonly TimeSpan CacheTtl = TimeSpan.FromMinutes(5);

    public async Task<bool> HasPermissionAsync(
        ClaimsPrincipal user,
        string[] required,
        Guid? projectId = null,
        CancellationToken ct = default)
    {
        if (required.Length == 0) return true;

        var tenantRole = user.FindFirstValue("tenantRole");
        if (tenantRole == nameof(TenantRole.Owner)) return true;

        if (!Guid.TryParse(user.FindFirstValue(ClaimTypes.NameIdentifier), out var userId)) return false;
        if (!Guid.TryParse(user.FindFirstValue("roleId"), out var roleId)) return false;

        var orgId = user.FindFirstValue("oid");
        var cacheKey = $"perms:{userId}:{orgId}:{roleId}:{projectId?.ToString() ?? "none"}";

        if (!cache.TryGetValue(cacheKey, out HashSet<string>? effective))
        {
            effective = await ResolvePermissionsForRole(roleId, ct);

            if (projectId.HasValue)
            {
                var projectRoleId = await projectRoleProvider.GetProjectRoleIdAsync(userId, projectId.Value, ct);
                if (projectRoleId.HasValue)
                {
                    var projectPerms = await ResolvePermissionsForRole(projectRoleId.Value, ct);
                    effective.UnionWith(projectPerms);
                }
            }

            cache.Set(cacheKey, effective, CacheTtl);
        }

        return required.All(r => PermissionMatcher.IsGranted(effective!, r));
    }

    private async Task<HashSet<string>> ResolvePermissionsForRole(Guid roleId, CancellationToken ct)
    {
        var role = await dbContext.Roles
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == new RoleId(roleId), ct);

        if (role is null || role.Policies.Count == 0) return [];

        var policyIdValues = role.Policies.Select(p => p.Value).ToList();

        var statements = await dbContext.Policies
            .AsNoTracking()
            .Where(p => policyIdValues.Contains(p.Id.Value))
            .SelectMany(p => p.Statements)
            .ToListAsync(ct);

        var allows = statements
            .Where(s => s.Effect == Effect.Allow)
            .Select(s => s.Action)
            .ToHashSet();

        var denies = statements
            .Where(s => s.Effect == Effect.Deny)
            .Select(s => s.Action)
            .ToHashSet();

        allows.ExceptWith(denies);
        return allows;
    }
}
