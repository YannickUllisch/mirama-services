using ErrorOr;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.Identity.Application.Common.Interfaces;
using Mirama.Modules.Identity.Domain.Aggregates.Role;
using Mirama.Modules.Identity.Domain.Enums;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Abstractions.Persistence;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.Identity.Application.Features.V1.AccessControl.Roles;

public class GetRolesController : TenantControllerBase
{
    [HttpGet("roles/{scope}")]
    public async Task<ActionResult<List<RoleResponse>>> Get([FromRoute] string scope)
    {
        var res = await this.Dispatcher.Send(new GetRolesQuery(scope));
        return res.Match(Ok, Problem);
    }
}

public sealed record GetRolesQuery(string Scope) : IQuery<ErrorOr<List<RoleResponse>>>;

internal class GetRolesQueryValidator : AbstractValidator<GetRolesQuery>
{
    public GetRolesQueryValidator()
    {
        RuleFor(q => q.Scope)
            .Must(s => Enum.TryParse<AccessScope>(s, ignoreCase: true, out _))
            .WithMessage("Provided scope is not supported. Valid values: " + string.Join(", ", Enum.GetNames<AccessScope>()));
    }
}

internal class GetRolesQueryHandler(
    IIdentityQueryRepository<Role, RoleId> roleRepository,
    IRequestContextProvider contextProvider) : IRequestHandler<GetRolesQuery, ErrorOr<List<RoleResponse>>>
{
    public async Task<ErrorOr<List<RoleResponse>>> HandleAsync(GetRolesQuery request, CancellationToken ct)
    {
        var tenantId = contextProvider.TenantId;

        Enum.TryParse<AccessScope>(request.Scope, ignoreCase: true, out var scope);

        var roles = await roleRepository.Query()
            .Where(r => r.TenantId == null || r.TenantId == tenantId)
            .Where(r => r.Scope == scope)
            .OrderBy(r => r.TenantId != null)
            .ThenBy(r => r.Name)
            .ToListAsync(ct);

        return roles.ConvertAll(r => r.MapResponse());
    }
}
