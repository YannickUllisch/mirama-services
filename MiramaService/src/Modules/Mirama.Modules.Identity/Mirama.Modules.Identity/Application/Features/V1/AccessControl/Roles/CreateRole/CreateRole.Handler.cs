using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.Identity.Domain.Aggregates.Role;
using Mirama.Modules.Identity.Domain.Enums;
using Mirama.Modules.Identity.Infrastructure.Persistence;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Abstractions.Persistence;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.Identity.Application.Features.V1.AccessControl.Roles.CreateRole;

public class CreateRoleController : TenantControllerBase
{
    [HttpPost("roles")]
    public async Task<ActionResult<RoleResponse>> Create([FromBody] CreateRoleCommand command)
    {
        var result = await this.Dispatcher.Send(command);
        return result.Match(r => CreatedAtAction(nameof(Create), new { id = r.Id }, r), Problem);
    }
}

internal class CreateRoleCommandHandler(
    IdentityDbContext dbContext,
    IRequestContextProvider contextProvider) : IRequestHandler<CreateRoleCommand, ErrorOr<RoleResponse>>
{
    public async Task<ErrorOr<RoleResponse>> HandleAsync(CreateRoleCommand request, CancellationToken ct)
    {
        var tenantId = contextProvider.TenantId;
        if (tenantId is null)
            return Error.Unauthorized("Role.Create.NoTenant", "Tenant context required.");

        if (!Enum.TryParse<AccessScope>(request.Scope, ignoreCase: true, out var scope))
            return Error.Validation("Role.Scope.Invalid", "Invalid scope value.");

        var duplicate = await dbContext.Roles
            .AsNoTracking()
            .AnyAsync(r => r.TenantId == tenantId && r.Name == request.Name.Trim() && r.Scope == scope, ct);

        if (duplicate)
            return Error.Conflict("Role.Duplicate", "A role with this name and scope already exists.");

        var role = Role.Create(new RoleDetails(request.Name, scope, tenantId, request.Description));

        dbContext.Roles.Add(role);

        return role.MapResponse();
    }
}
