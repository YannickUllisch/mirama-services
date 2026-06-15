using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.Identity.Application.Common.Interfaces;
using Mirama.Modules.Identity.Domain.Aggregates.Role;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Abstractions.Persistence;
using Mirama.SharedKernel.Models;
using Mirama.SharedKernel.Models.Permissions;

namespace Mirama.Modules.Identity.Application.Features.V1.AccessControl.Roles.UpdateRole;

public class UpdateRoleController : TenantControllerBase
{
    [RequirePermission(Permissions.IamRole.Manage)]
    [HttpPut("roles/{id:guid}")]
    public async Task<ActionResult<RoleResponse>> Update([FromRoute] Guid id, [FromBody] UpdateRoleCommand command)
    {
        var result = await this.Dispatcher.Send(command with { Id = id });
        return result.Match(Ok, Problem);
    }
}

internal class UpdateRoleCommandHandler(
    IIdentityCommandRepository<Role, RoleId> roleRepository,
    IRequestContextProvider contextProvider) : IRequestHandler<UpdateRoleCommand, ErrorOr<RoleResponse>>
{
    public async Task<ErrorOr<RoleResponse>> HandleAsync(UpdateRoleCommand request, CancellationToken ct)
    {
        var role = await roleRepository.Query()
            .FirstOrDefaultAsync(r => r.Id == new RoleId(request.Id), ct);

        if (role is null)
            return Error.NotFound("Role.NotFound", "Role not found.");

        if (role.IsSystemRole)
            return Error.Forbidden("Role.SystemRole", "System roles cannot be modified.");

        if (role.TenantId != contextProvider.TenantId)
            return Error.Forbidden("Role.Ownership", "You can only modify roles in your tenant.");

        role.Update(new RoleDetails(request.Name, role.Scope, role.TenantId, request.Description));

        return role.MapResponse();
    }
}
