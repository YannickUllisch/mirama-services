using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Mirama.Modules.Identity.Application.Common;
using Mirama.Modules.Identity.Domain.Aggregates.Policy;
using Mirama.Modules.Identity.Domain.Aggregates.Role;
using Mirama.Modules.Identity.Infrastructure.Persistence;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Abstractions.Persistence;
using Mirama.SharedKernel.Models;
using Mirama.SharedKernel.Models.Permissions;

namespace Mirama.Modules.Identity.Application.Features.V1.AccessControl.Roles.AttachPolicy;

public class AttachPolicyController : TenantControllerBase
{
    [HttpPost("roles/{roleId:guid}/policies/{policyId:guid}")]
    [RequirePermission(Permissions.IamRole.Manage)]
    public async Task<ActionResult<RoleResponse>> Attach([FromRoute] Guid roleId, [FromRoute] Guid policyId)
    {
        var result = await this.Dispatcher.Send(new AttachPolicyCommand(roleId, policyId));
        return result.Match(Ok, Problem);
    }
}

public sealed record AttachPolicyCommand(Guid RoleId, Guid PolicyId) : ICommand<ErrorOr<RoleResponse>>;

internal class AttachPolicyCommandHandler(
    IdentityDbContext dbContext,
    IMemoryCache cache,
    IRequestContextProvider contextProvider) : IRequestHandler<AttachPolicyCommand, ErrorOr<RoleResponse>>
{
    public async Task<ErrorOr<RoleResponse>> HandleAsync(AttachPolicyCommand request, CancellationToken ct)
    {
        var tenantId = contextProvider.TenantId;

        var roleId = new RoleId(request.RoleId);
        var role = await dbContext.Roles.FirstOrDefaultAsync(r => r.Id == roleId, ct);

        if (role is null)
            return Error.NotFound("Role.NotFound", "Role not found.");
        if (role.IsSystemRole)
            return Error.Forbidden("Role.SystemRole", "System roles cannot be modified.");
        if (role.TenantId != tenantId)
            return Error.Forbidden("Role.Ownership", "You can only modify roles in your tenant.");

        var policyId = new PolicyId(request.PolicyId);
        var policy = await dbContext.Policies
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == policyId && (p.TenantId == null || p.TenantId == tenantId), ct);

        if (policy is null)
            return Error.NotFound("Policy.NotFound", "Policy not found.");

        var attachResult = role.AttachPolicy(policyId, policy.Scope);
        if (attachResult.IsError) return attachResult.Errors;

        cache.Remove(PermissionCacheKeys.RolePerms(request.RoleId));

        return role.MapResponse();
    }
}
