using ErrorOr;
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
    public async Task<ActionResult<List<TagResponse>>> Get([FromQuery] int? scope)
    {
        var result = await this.Dispatcher.Send(new GetTagsQuery(scope));
        return result.Match(Ok, Problem);
    }
}

public sealed record GetTagsQuery(int? Scope) : IQuery<ErrorOr<List<TagResponse>>>;

internal class GetTagsQueryHandler(
    IdentityDbContext dbContext) : IRequestHandler<GetTagsQuery, ErrorOr<List<TagResponse>>>
{
    public async Task<ErrorOr<List<TagResponse>>> HandleAsync(GetTagsQuery request, CancellationToken ct)
    {
        var query = dbContext.Tags.AsNoTracking();

        if (request.Scope.HasValue && request.Scope.Value != (int)TagScope.None)
        {
            var effectiveScope = request.Scope.Value | (int)TagScope.General;
            query = query.Where(t => ((int)t.Scope & effectiveScope) != 0);
        }

        var tags = await query
            .OrderBy(t => t.Name)
            .ToListAsync(ct);

        return tags.ConvertAll(t => t.MapResponse());
    }
}
