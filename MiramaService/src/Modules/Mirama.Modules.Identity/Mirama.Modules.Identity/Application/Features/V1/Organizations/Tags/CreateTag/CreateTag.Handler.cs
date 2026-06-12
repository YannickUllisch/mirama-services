using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.Identity.Domain.Aggregates.Organization.Tag;
using Mirama.Modules.Identity.Infrastructure.Persistence;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.Identity.Application.Features.V1.Organizations.Tags.CreateTag;

public class CreateTagController : OrganizationControllerBase
{
    [HttpPost("tags")]
    public async Task<ActionResult<TagResponse>> Create([FromBody] CreateTagCommand command)
    {
        var result = await this.Dispatcher.Send(command);
        return result.Match(r => CreatedAtAction(nameof(Create), new { tagId = r.Id }, r), Problem);
    }
}

internal class CreateTagCommandHandler(
    IdentityDbContext dbContext) : IRequestHandler<CreateTagCommand, ErrorOr<TagResponse>>
{
    public async Task<ErrorOr<TagResponse>> HandleAsync(CreateTagCommand request, CancellationToken ct)
    {
        var slug = Tag.GenerateSlug(request.Name);

        var slugExists = await dbContext.Tags
            .AsNoTracking()
            .AnyAsync(t => t.Slug == slug, ct);

        if (slugExists)
            return Error.Conflict("Tag.Duplicate", "A tag with this name already exists in the organization.");

        var scope = (TagScope)request.Scope;
        var tag = Tag.Create(new TagDetails(request.Name, request.Color, request.Description, scope));
        dbContext.Tags.Add(tag);

        return tag.MapResponse();
    }
}
