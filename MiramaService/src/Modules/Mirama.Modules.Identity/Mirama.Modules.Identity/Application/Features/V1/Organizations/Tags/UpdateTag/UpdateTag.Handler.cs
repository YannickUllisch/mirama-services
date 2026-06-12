using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.Identity.Domain.Aggregates.Organization.Tag;
using Mirama.Modules.Identity.Infrastructure.Persistence;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.Identity.Application.Features.V1.Organizations.Tags.UpdateTag;

public class UpdateTagController : OrganizationControllerBase
{
    [HttpPut("tags/{tagId:guid}")]
    public async Task<ActionResult<TagResponse>> Update([FromRoute] Guid tagId, [FromBody] UpdateTagCommand command)
    {
        var result = await this.Dispatcher.Send(command with { TagId = tagId });
        return result.Match(Ok, Problem);
    }
}

internal class UpdateTagCommandHandler(
    IdentityDbContext dbContext) : IRequestHandler<UpdateTagCommand, ErrorOr<TagResponse>>
{
    public async Task<ErrorOr<TagResponse>> HandleAsync(UpdateTagCommand request, CancellationToken ct)
    {
        var tag = await dbContext.Tags
            .FirstOrDefaultAsync(t => t.Id == new TagId(request.TagId), ct);

        if (tag is null)
            return Error.NotFound("Tag.NotFound", "Tag not found.");

        var newSlug = Tag.GenerateSlug(request.Name);

        var slugConflict = await dbContext.Tags
            .AsNoTracking()
            .AnyAsync(t => t.Slug == newSlug && t.Id != new TagId(request.TagId), ct);

        if (slugConflict)
            return Error.Conflict("Tag.Duplicate", "A tag with this name already exists in the organization.");

        tag.Update(new TagDetails(request.Name, request.Color, request.Description, (TagScope)request.Scope));

        return tag.MapResponse();
    }
}
