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

namespace Mirama.Modules.Identity.Application.Features.V1.AccessControl.Roles.DetachPolicy;

public class DetachPolicyController : TenantControllerBase
{
    [HttpDelete("roles/{roleId:guid}/policies/{policyId:guid}")]
    public async Task<IActionResult> Detach([FromRoute] Guid roleId, [FromRoute] Guid policyId)
    {
        var result = await this.Dispatcher.Send(new DetachPolicyCommand(roleId, policyId));
        return result.Match(_ => NoContent(), Problem);
    }
}

public sealed record DetachPolicyCommand(Guid RoleId, Guid PolicyId) : ICommand<ErrorOr<Deleted>>;

internal class DetachPolicyCommandHandler(
    IdentityDbContext dbContext,
    IRequestContextProvider contextProvider) : IRequestHandler<DetachPolicyCommand, ErrorOr<Deleted>>
{
    public async Task<ErrorOr<Deleted>> HandleAsync(DetachPolicyCommand request, CancellationToken ct)
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

        var result = role.DetachPolicy(new PolicyId(request.PolicyId));
        if (result.IsError) return result.Errors;

        return Result.Deleted;
    }
}
