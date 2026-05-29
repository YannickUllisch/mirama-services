using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.Identity.Domain.Aggregates.Organization.Invitation;
using Mirama.Modules.Identity.Domain.Aggregates.User;
using Mirama.Modules.Identity.Infrastructure.Persistence;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Abstractions.Persistence;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.Identity.Application.Features.V1.Users.Invitations.DeclineMyInvitation;

public class DeclineMyInvitationController : ApiControllerBase
{
    [HttpPost("users/invitations/{invitationId:guid}/decline")]
    public async Task<IActionResult> Decline([FromRoute] Guid invitationId)
    {
        var result = await this.Dispatcher.Send(new DeclineMyInvitationCommand(invitationId));
        return result.Match(_ => NoContent(), Problem);
    }
}

public sealed record DeclineMyInvitationCommand(Guid InvitationId) : ICommand<ErrorOr<Deleted>>;

internal class DeclineMyInvitationCommandHandler(
    IdentityDbContext dbContext,
    IRequestContextProvider contextProvider) : IRequestHandler<DeclineMyInvitationCommand, ErrorOr<Deleted>>
{
    public async Task<ErrorOr<Deleted>> HandleAsync(DeclineMyInvitationCommand request, CancellationToken ct)
    {
        var userId = contextProvider.UserId;

        var user = await dbContext.Users
            .AsNoTracking()
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(u => u.Id == new UserId(userId), ct);

        if (user is null)
            return Error.NotFound("User.NotFound", "User not found.");

        var invitation = await dbContext.Invitations
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(i => i.Id == new InvitationId(request.InvitationId), ct);

        if (invitation is null)
            return Error.NotFound("Invitation.NotFound", "Invitation not found.");

        if (!invitation.Email.Equals(user.Email, StringComparison.OrdinalIgnoreCase))
            return Error.Forbidden("Invitation.WrongUser", "This invitation was not sent to you.");

        if (invitation.Status != InvitationStatus.Pending)
            return Error.Conflict("Invitation.NotPending", "Only pending invitations can be declined.");

        invitation.Decline();

        return Result.Deleted;
    }
}
