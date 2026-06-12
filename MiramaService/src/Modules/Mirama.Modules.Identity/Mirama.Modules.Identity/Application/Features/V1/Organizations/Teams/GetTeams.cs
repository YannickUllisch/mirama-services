using ErrorOr;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.Identity.Infrastructure.Persistence;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Abstractions.Persistence;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.Identity.Application.Features.V1.Organizations.Teams;

public class GetTeamsController : OrganizationControllerBase
{
    [HttpGet("teams")]
    public async Task<ActionResult<PaginatedList<TeamResponse>>> Get([FromQuery] GetTeamsQuery query)
    {
        var res = await this.Dispatcher.Send(query);
        return res.Match(Ok, Problem);
    }
}

public sealed record GetTeamsQuery : IQuery<ErrorOr<PaginatedList<TeamResponse>>>
{
    public int? PageSize { get; init; }
    public int? PageNumber { get; init; }
}

internal class GetTeamsQueryValidator : AbstractValidator<GetTeamsQuery>
{
    public GetTeamsQueryValidator()
    {
        RuleFor(q => q.PageSize).LessThanOrEqualTo(50);
    }
}

internal class GetTeamsQueryHandler(
    IdentityDbContext dbContext,
    IRequestContextProvider contextProvider) : IRequestHandler<GetTeamsQuery, ErrorOr<PaginatedList<TeamResponse>>>
{
    public async Task<ErrorOr<PaginatedList<TeamResponse>>> HandleAsync(GetTeamsQuery request, CancellationToken ct)
    {
        var organizationId = contextProvider.OrganizationId;
        if (organizationId is null)
            return Error.Unauthorized("Team.NoOrg", "Organization context required.");

        var query = dbContext.Teams
            .AsNoTracking()
            .Include(t => t.Members)
            .Where(t => t.OrganizationId == organizationId.Value)
            .OrderBy(t => t.Name);

        if (request.PageNumber is not null && request.PageSize is not null)
        {
            var total = await query.CountAsync(ct);
            var page = await query.Skip((request.PageNumber.Value - 1) * request.PageSize.Value).Take(request.PageSize.Value).ToListAsync(ct);
            return new PaginatedList<TeamResponse>(page.ConvertAll(t => t.MapResponse()), total, request.PageNumber.Value, request.PageSize.Value);
        }

        var items = await query.ToListAsync(ct);
        return new PaginatedList<TeamResponse>(items.ConvertAll(t => t.MapResponse()), items.Count, 1, items.Count > 0 ? items.Count : 1);
    }
}
