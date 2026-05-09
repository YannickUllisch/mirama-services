using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.Identity.Domain.Aggregates.Role;
using Mirama.Modules.Identity.Infrastructure.Persistence;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Abstractions.Persistence;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.Identity.Application.Features.V1.AccessControl.Roles.DeleteRole;

public class DeleteRoleController : TenantControllerBase
{
    [HttpDelete("roles/{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var result = await this.Dispatcher.Send(new DeleteRoleCommand(id));
        return result.Match(_ => NoContent(), Problem);
    }
}

public sealed record DeleteRoleCommand(Guid Id) : ICommand<ErrorOr<Deleted>>;

internal class DeleteRoleCommandHandler(
    IdentityDbContext dbContext,
    IRequestContextProvider contextProvider) : IRequestHandler<DeleteRoleCommand, ErrorOr<Deleted>>
{
    public async Task<ErrorOr<Deleted>> HandleAsync(DeleteRoleCommand request, CancellationToken ct)
    {
        var role = await dbContext.Roles
            .FirstOrDefaultAsync(r => r.Id.Value == request.Id, ct);

        if (role is null)
            return Error.NotFound("Role.NotFound", "Role not found.");

        if (role.IsSystemRole)
            return Error.Forbidden("Role.SystemRole", "System roles cannot be deleted.");

        if (role.TenantId != contextProvider.TenantId)
            return Error.Forbidden("Role.Ownership", "You can only delete roles in your tenant.");

        dbContext.Roles.Remove(role);

        return Result.Deleted;
    }
}
