using ErrorOr;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.Identity.Infrastructure.Persistence;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.Identity.Application.Features.V1.Organizations;

public class GetOrganizationsController : TenantControllerBase
{
    [HttpGet("organizations")]
    public async Task<ActionResult<PaginatedList<OrganizationResponse>>> Get([FromQuery] GetOrganizationsQuery query)
    {
        var res = await this.Dispatcher.Send(query);
        return res.Match(Ok, Problem);
    }
}

public sealed record GetOrganizationsQuery : IQuery<ErrorOr<PaginatedList<OrganizationResponse>>>
{
    public int? PageSize { get; init; }
    public int? PageNumber { get; init; }
}

internal class GetOrganizationsQueryValidator : AbstractValidator<GetOrganizationsQuery>
{
    public GetOrganizationsQueryValidator()
    {
        RuleFor(q => q.PageSize).LessThanOrEqualTo(50);
    }
}

internal class GetOrganizationsQueryHandler(
    IdentityDbContext dbContext) : IRequestHandler<GetOrganizationsQuery, ErrorOr<PaginatedList<OrganizationResponse>>>
{
    public async Task<ErrorOr<PaginatedList<OrganizationResponse>>> HandleAsync(GetOrganizationsQuery request, CancellationToken ct)
    {
        var query = dbContext.Organizations.AsNoTracking().OrderBy(o => o.DateCreated);

        if (request.PageNumber is not null && request.PageSize is not null)
        {
            var total = await query.CountAsync(ct);
            var page = await query.Skip((request.PageNumber.Value - 1) * request.PageSize.Value).Take(request.PageSize.Value).ToListAsync(ct);
            return new PaginatedList<OrganizationResponse>(page.ConvertAll(o => o.MapResponse()), total, request.PageNumber.Value, request.PageSize.Value);
        }

        var items = await query.ToListAsync(ct);
        return new PaginatedList<OrganizationResponse>(items.ConvertAll(o => o.MapResponse()), items.Count, 1, items.Count > 0 ? items.Count : 1);
    }
}
