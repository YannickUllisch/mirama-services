using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Mirama.Modules.Identity.Application.Common;
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
    private static readonly TimeSpan RoleCacheTtl = TimeSpan.FromMinutes(10);
    private static readonly TimeSpan MemberCacheTtl = TimeSpan.FromMinutes(5);

    public async Task<bool> HasPermissionAsync(
        ClaimsPrincipal user,
        string[] required,
        Guid? projectId = null,
        CancellationToken ct = default)
    {
        if (required.Length == 0) return true;

        if (user.FindFirstValue("tenantRole") == nameof(TenantRole.Owner)) return true;

        if (!Guid.TryParse(user.FindFirstValue(ClaimTypes.NameIdentifier), out var userId)) return false;
        if (!Guid.TryParse(user.FindFirstValue("oid"), out var orgId)) return false;

        var roleIds = await GetMemberRoleIdsAsync(userId, orgId, ct);

        if (projectId.HasValue)
        {
            var projectRoleId = await projectRoleProvider.GetProjectRoleIdAsync(userId, projectId.Value, ct);
            if (projectRoleId.HasValue)
                roleIds = [.. roleIds, new RoleId(projectRoleId.Value)];
        }

        if (roleIds.Count == 0) return false;

        var effective = await UnionPermissionsAsync(roleIds, ct);
        return required.All(r => PermissionMatcher.IsGranted(effective, r));
    }

    private async Task<List<RoleId>> GetMemberRoleIdsAsync(Guid userId, Guid orgId, CancellationToken ct)
    {
        var cacheKey = PermissionCacheKeys.MemberRoles(userId, orgId);
        if (cache.TryGetValue(cacheKey, out List<RoleId>? roleIds))
            return roleIds!;

        var member = await dbContext.Members
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.UserId == new UserId(userId) && m.OrganizationId == orgId, ct);

        roleIds = member?.IamRoleIds ?? [];
        cache.Set(cacheKey, roleIds, MemberCacheTtl);
        return roleIds;
    }

    private async Task<IReadOnlySet<string>> UnionPermissionsAsync(IEnumerable<RoleId> roleIds, CancellationToken ct)
    {
        var allAllows = new HashSet<string>();
        var allDenies = new HashSet<string>();

        foreach (var roleId in roleIds)
        {
            var (allows, denies) = await GetRolePermissionsAsync(roleId, ct);
            allAllows.UnionWith(allows);
            allDenies.UnionWith(denies);
        }

        // Explicit deny across any role wins over allow in any role
        allAllows.ExceptWith(allDenies);
        return allAllows;
    }

    private async Task<(HashSet<string> Allows, HashSet<string> Denies)> GetRolePermissionsAsync(
        RoleId roleId, CancellationToken ct)
    {
        var cacheKey = PermissionCacheKeys.RolePerms(roleId.Value);
        if (cache.TryGetValue(cacheKey, out (HashSet<string>, HashSet<string>) cached))
            return cached;

        var role = await dbContext.Roles
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == roleId, ct);

        if (role is null || role.Policies.Count == 0)
        {
            var empty = (new HashSet<string>(), new HashSet<string>());
            cache.Set(cacheKey, empty, RoleCacheTtl);
            return empty;
        }

        var statements = await dbContext.Policies
            .AsNoTracking()
            .Where(p => role.Policies.Contains(p.Id))
            .SelectMany(p => p.Statements)
            .ToListAsync(ct);

        var allows = statements.Where(s => s.Effect == Effect.Allow).Select(s => s.Action).ToHashSet();
        var denies = statements.Where(s => s.Effect == Effect.Deny).Select(s => s.Action).ToHashSet();

        var result = (allows, denies);
        cache.Set(cacheKey, result, RoleCacheTtl);
        return result;
    }
}
