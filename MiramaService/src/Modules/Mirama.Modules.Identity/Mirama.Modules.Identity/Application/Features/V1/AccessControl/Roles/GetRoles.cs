using System.Text.Json.Serialization;
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
    [HttpGet("roles")]
    public async Task<ActionResult<PaginatedList<RoleResponse>>> Get([FromQuery] GetRolesQuery query)
    {
        var res = await this.Dispatcher.Send(query);
        return res.Match(Ok, Problem);
    }
}

public sealed record GetRolesQuery : IQuery<ErrorOr<PaginatedList<RoleResponse>>>
{
    [JsonPropertyName("pageSize")]
    public int? PageSize { get; init; }

    [JsonPropertyName("pageNumber")]
    public int? PageNumber { get; init; }

    [JsonPropertyName("scope")]
    public string? Scope { get; init; }
}

internal class GetRolesQueryValidator : AbstractValidator<GetRolesQuery>
{
    public GetRolesQueryValidator()
    {
        RuleFor(q => q.PageSize).LessThanOrEqualTo(50);

        When(q => q.Scope is not null, () =>
            RuleFor(q => q.Scope!)
                .Must(s => Enum.TryParse<AccessScope>(s, ignoreCase: true, out _))
                .WithMessage("Scope must be 'Organization' or 'Project'"));
    }
}

internal class GetRolesQueryHandler(
    IIdentityQueryRepository<Role, RoleId> roleRepository,
    IRequestContextProvider contextProvider) : IRequestHandler<GetRolesQuery, ErrorOr<PaginatedList<RoleResponse>>>
{
    public async Task<ErrorOr<PaginatedList<RoleResponse>>> HandleAsync(GetRolesQuery request, CancellationToken ct)
    {
        var tenantId = contextProvider.TenantId;

        IQueryable<Role> query = roleRepository.Query()
            .Where(r => r.TenantId == null || r.TenantId == tenantId)
            .OrderBy(r => r.TenantId != null)
            .ThenBy(r => r.Name);

        if (request.Scope is not null && Enum.TryParse<AccessScope>(request.Scope, ignoreCase: true, out var scope))
            query = query.Where(r => r.Scope == scope);

        if (request.PageNumber is not null && request.PageSize is not null)
        {
            var total = await query.CountAsync(ct);
            var page = await query
                .Skip((request.PageNumber.Value - 1) * request.PageSize.Value)
                .Take(request.PageSize.Value)
                .ToListAsync(ct);

            return new PaginatedList<RoleResponse>(page.ConvertAll(r => r.MapResponse()), total, request.PageNumber.Value, request.PageSize.Value);
        }

        var roles = await query.ToListAsync(ct);
        return new PaginatedList<RoleResponse>(roles.ConvertAll(r => r.MapResponse()), roles.Count, 1, roles.Count > 0 ? roles.Count : 1);
    }
}
