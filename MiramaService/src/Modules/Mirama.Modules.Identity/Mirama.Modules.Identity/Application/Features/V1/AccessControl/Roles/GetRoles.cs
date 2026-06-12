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
    public async Task<ActionResult<PaginatedList<RoleResponse>>> Get([FromRoute] string scope, [FromQuery] GetRolesQuery query)
    {
        var res = await this.Dispatcher.Send(query with { Scope = scope });
        return res.Match(Ok, Problem);
    }
}

public sealed record GetRolesQuery : IQuery<ErrorOr<PaginatedList<RoleResponse>>>
{
    public string Scope { get; init; } = string.Empty;
    public int? PageSize { get; init; }
    public int? PageNumber { get; init; }
}

internal class GetRolesQueryValidator : AbstractValidator<GetRolesQuery>
{
    public GetRolesQueryValidator()
    {
        RuleFor(q => q.Scope)
            .Must(s => Enum.TryParse<AccessScope>(s, ignoreCase: true, out _))
            .WithMessage("Provided scope is not supported. Valid values: " + string.Join(", ", Enum.GetNames<AccessScope>()));
        RuleFor(q => q.PageSize).LessThanOrEqualTo(50);
    }
}

internal class GetRolesQueryHandler(
    IIdentityQueryRepository<Role, RoleId> roleRepository,
    IRequestContextProvider contextProvider) : IRequestHandler<GetRolesQuery, ErrorOr<PaginatedList<RoleResponse>>>
{
    public async Task<ErrorOr<PaginatedList<RoleResponse>>> HandleAsync(GetRolesQuery request, CancellationToken ct)
    {
        var tenantId = contextProvider.TenantId;

        Enum.TryParse<AccessScope>(request.Scope, ignoreCase: true, out var scope);

        var query = roleRepository.Query()
            .Where(r => r.TenantId == null || r.TenantId == tenantId)
            .Where(r => r.Scope == scope)
            .OrderBy(r => r.TenantId != null)
            .ThenBy(r => r.Name);

        if (request.PageNumber is not null && request.PageSize is not null)
        {
            var total = await query.CountAsync(ct);
            var page = await query.Skip((request.PageNumber.Value - 1) * request.PageSize.Value).Take(request.PageSize.Value).ToListAsync(ct);
            return new PaginatedList<RoleResponse>(page.ConvertAll(r => r.MapResponse()), total, request.PageNumber.Value, request.PageSize.Value);
        }

        var items = await query.ToListAsync(ct);
        return new PaginatedList<RoleResponse>(items.ConvertAll(r => r.MapResponse()), items.Count, 1, items.Count > 0 ? items.Count : 1);
    }
}
