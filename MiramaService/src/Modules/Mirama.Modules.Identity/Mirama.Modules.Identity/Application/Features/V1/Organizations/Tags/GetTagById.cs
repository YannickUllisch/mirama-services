using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.Identity.Domain.Aggregates.Organization.Tag;
using Mirama.Modules.Identity.Infrastructure.Persistence;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.Identity.Application.Features.V1.Organizations.Tags;

public class GetTagByIdController : OrganizationControllerBase
{
    [HttpGet("tags/{tagId:guid}")]
    public async Task<ActionResult<TagResponse>> Get([FromRoute] Guid tagId)
    {
        var result = await this.Dispatcher.Send(new GetTagByIdQuery(tagId));
        return result.Match(Ok, Problem);
    }
}

public sealed record GetTagByIdQuery(Guid TagId) : IQuery<ErrorOr<TagResponse>>;

internal class GetTagByIdQueryHandler(
    IdentityDbContext dbContext) : IRequestHandler<GetTagByIdQuery, ErrorOr<TagResponse>>
{
    public async Task<ErrorOr<TagResponse>> HandleAsync(GetTagByIdQuery request, CancellationToken ct)
    {
        var tag = await dbContext.Tags
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == new TagId(request.TagId), ct);

        if (tag is null)
            return Error.NotFound("Tag.NotFound", "Tag not found.");

        return tag.MapResponse();
    }
}
