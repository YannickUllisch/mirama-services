using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.Identity.Application.Common.Interfaces;
using Mirama.Modules.Identity.Domain.Aggregates.Role;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Abstractions.Persistence;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.Identity.Application.Features.V1.AccessControl.Roles;

public class GetRoleByIdController : TenantControllerBase
{
    [HttpGet("roles/{id:guid}")]
    public async Task<ActionResult<RoleResponse>> Get([FromRoute] Guid id)
    {
        var res = await this.Dispatcher.Send(new GetRoleByIdQuery(id));
        return res.Match(Ok, Problem);
    }
}

public sealed record GetRoleByIdQuery(Guid Id) : IQuery<ErrorOr<RoleResponse>>;

internal class GetRoleByIdQueryHandler(
    IIdentityQueryRepository<Role, RoleId> roleRepository,
    IRequestContextProvider contextProvider) : IRequestHandler<GetRoleByIdQuery, ErrorOr<RoleResponse>>
{
    public async Task<ErrorOr<RoleResponse>> HandleAsync(GetRoleByIdQuery request, CancellationToken ct)
    {
        var tenantId = contextProvider.TenantId;

        var role = await roleRepository.Query()
            .Where(r => r.TenantId == null || r.TenantId == tenantId)
            .FirstOrDefaultAsync(r => r.Id.Value == request.Id, ct);

        if (role is null)
            return Error.NotFound("Role.NotFound", "Role not found.");

        return role.MapResponse();
    }
}
