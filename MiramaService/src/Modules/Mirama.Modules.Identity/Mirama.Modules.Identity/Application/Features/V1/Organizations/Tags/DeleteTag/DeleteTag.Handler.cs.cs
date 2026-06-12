using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.Identity.Domain.Aggregates.Organization.Tag;
using Mirama.Modules.Identity.Infrastructure.Persistence;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.Identity.Application.Features.V1.Organizations.Tags.DeleteTag;

public class DeleteTagController : OrganizationControllerBase
{
    [HttpDelete("tags/{tagId:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid tagId)
    {
        var result = await this.Dispatcher.Send(new DeleteTagCommand(tagId));
        return result.Match(_ => NoContent(), Problem);
    }
}

public sealed record DeleteTagCommand(Guid TagId) : ICommand<ErrorOr<Deleted>>;

internal class DeleteTagCommandHandler(
    IdentityDbContext dbContext) : IRequestHandler<DeleteTagCommand, ErrorOr<Deleted>>
{
    public async Task<ErrorOr<Deleted>> HandleAsync(DeleteTagCommand request, CancellationToken ct)
    {
        var tag = await dbContext.Tags
            .FirstOrDefaultAsync(t => t.Id == new TagId(request.TagId), ct);

        if (tag is null)
            return Error.NotFound("Tag.NotFound", "Tag not found.");

        dbContext.Tags.Remove(tag);

        return Result.Deleted;
    }
}
