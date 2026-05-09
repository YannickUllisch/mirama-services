using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.Identity.Application.Features.V1.AccessControl.Roles;
using Mirama.Modules.Identity.Domain.Aggregates.Policy;
using Mirama.Modules.Identity.Domain.Aggregates.Role;
using Mirama.Modules.Identity.Infrastructure.Persistence;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Abstractions.Persistence;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.Identity.Application.Features.V1.AccessControl.Roles.AttachPolicy;

public class AttachPolicyController : TenantControllerBase
{
    [HttpPost("roles/{roleId:guid}/policies/{policyId:guid}")]
    public async Task<ActionResult<RoleResponse>> Attach([FromRoute] Guid roleId, [FromRoute] Guid policyId)
    {
        var result = await this.Dispatcher.Send(new AttachPolicyCommand(roleId, policyId));
        return result.Match(Ok, Problem);
    }
}

public sealed record AttachPolicyCommand(Guid RoleId, Guid PolicyId) : ICommand<ErrorOr<RoleResponse>>;

internal class AttachPolicyCommandHandler(
    IdentityDbContext dbContext,
    IRequestContextProvider contextProvider) : IRequestHandler<AttachPolicyCommand, ErrorOr<RoleResponse>>
{
    public async Task<ErrorOr<RoleResponse>> HandleAsync(AttachPolicyCommand request, CancellationToken ct)
    {
        var tenantId = contextProvider.TenantId;

        var role = await dbContext.Roles
            .FirstOrDefaultAsync(r => r.Id.Value == request.RoleId, ct);

        if (role is null)
            return Error.NotFound("Role.NotFound", "Role not found.");

        if (role.IsSystemRole)
            return Error.Forbidden("Role.SystemRole", "System roles cannot be modified.");

        if (role.TenantId != tenantId)
            return Error.Forbidden("Role.Ownership", "You can only modify roles in your tenant.");

        var policyExists = await dbContext.Policies
            .AsNoTracking()
            .AnyAsync(p => p.Id.Value == request.PolicyId && (p.TenantId == null || p.TenantId == tenantId), ct);

        if (!policyExists)
            return Error.NotFound("Policy.NotFound", "Policy not found.");

        role.AttachPolicy(new PolicyId(request.PolicyId));

        return role.MapResponse();
    }
}
