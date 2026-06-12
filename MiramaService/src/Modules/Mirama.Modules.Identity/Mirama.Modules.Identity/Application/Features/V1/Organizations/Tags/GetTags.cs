using ErrorOr;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.Identity.Domain.Aggregates.Organization.Tag;
using Mirama.Modules.Identity.Infrastructure.Persistence;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.Identity.Application.Features.V1.Organizations.Tags;

public class GetTagsController : OrganizationControllerBase
{
    [HttpGet("tags")]
    public async Task<ActionResult<PaginatedList<TagResponse>>> Get([FromQuery] GetTagsQuery query)
    {
        var result = await this.Dispatcher.Send(query);
        return result.Match(Ok, Problem);
    }
}

public sealed record GetTagsQuery : IQuery<ErrorOr<PaginatedList<TagResponse>>>
{
    public int? Scope { get; init; }
    public int? PageSize { get; init; }
    public int? PageNumber { get; init; }
}

internal class GetTagsQueryValidator : AbstractValidator<GetTagsQuery>
{
    public GetTagsQueryValidator()
    {
        RuleFor(q => q.PageSize).LessThanOrEqualTo(50);
    }
}

internal class GetTagsQueryHandler(
    IdentityDbContext dbContext) : IRequestHandler<GetTagsQuery, ErrorOr<PaginatedList<TagResponse>>>
{
    public async Task<ErrorOr<PaginatedList<TagResponse>>> HandleAsync(GetTagsQuery request, CancellationToken ct)
    {
        var query = dbContext.Tags.AsNoTracking();

        if (request.Scope.HasValue && request.Scope.Value != (int)TagScope.None)
        {
            var effectiveScope = request.Scope.Value | (int)TagScope.General;
            query = query.Where(t => ((int)t.Scope & effectiveScope) != 0);
        }

        var orderedQuery = query.OrderBy(t => t.Name);

        if (request.PageNumber is not null && request.PageSize is not null)
        {
            var total = await orderedQuery.CountAsync(ct);
            var page = await orderedQuery.Skip((request.PageNumber.Value - 1) * request.PageSize.Value).Take(request.PageSize.Value).ToListAsync(ct);
            return new PaginatedList<TagResponse>(page.ConvertAll(t => t.MapResponse()), total, request.PageNumber.Value, request.PageSize.Value);
        }

        var items = await orderedQuery.ToListAsync(ct);
        return new PaginatedList<TagResponse>(items.ConvertAll(t => t.MapResponse()), items.Count, 1, items.Count > 0 ? items.Count : 1);
    }
}
