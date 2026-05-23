using ErrorOr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.Identity.Domain.Aggregates.User;
using Mirama.Modules.Identity.Infrastructure.Persistence;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.Identity.Application.Features.V1.Auth.LinkUserExternalId;

[AllowAnonymous]
public class LinkUserExternalIdController : ApiControllerBase
{
    [HttpPost("auth/user/{userId:guid}/link-external")]
    public async Task<IActionResult> Post(
        [FromRoute] Guid userId,
        [FromBody] LinkExternalIdBody body)
    {
        var res = await this.Dispatcher.Send(new LinkUserExternalIdCommand(userId, body.ExternalId));
        return res.Match(_ => Ok(), Problem);
    }
}

internal class LinkUserExternalIdCommandHandler(
    IdentityDbContext dbContext) : IRequestHandler<LinkUserExternalIdCommand, ErrorOr<Success>>
{
    public async Task<ErrorOr<Success>> HandleAsync(LinkUserExternalIdCommand request, CancellationToken ct)
    {
        var user = await dbContext.Users
            .FirstOrDefaultAsync(u => u.Id == new UserId(request.UserId), ct);

        if (user is null)
            return Error.NotFound("User.NotFound", "User not found.");

        var result = user.LinkExternalId(request.ExternalId);
        if (result.IsError)
            return result.Errors;

        return Result.Success;
    }
}
