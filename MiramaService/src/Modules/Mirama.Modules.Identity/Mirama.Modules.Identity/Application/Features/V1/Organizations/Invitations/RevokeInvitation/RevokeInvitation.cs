using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.Identity.Domain.Aggregates.Organization.Invitation;
using Mirama.Modules.Identity.Infrastructure.Persistence;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.Identity.Application.Features.V1.Organizations.Invitations.RevokeInvitation;

public class RevokeInvitationController : OrganizationControllerBase
{
    [HttpDelete("invitations/{invitationId:guid}")]
    public async Task<IActionResult> Revoke([FromRoute] Guid invitationId)
    {
        var result = await this.Dispatcher.Send(new RevokeInvitationCommand(invitationId));
        return result.Match(_ => NoContent(), Problem);
    }
}

public sealed record RevokeInvitationCommand(Guid InvitationId) : ICommand<ErrorOr<Deleted>>;

internal class RevokeInvitationCommandHandler(
    IdentityDbContext dbContext) : IRequestHandler<RevokeInvitationCommand, ErrorOr<Deleted>>
{
    public async Task<ErrorOr<Deleted>> HandleAsync(RevokeInvitationCommand request, CancellationToken ct)
    {
        var invitation = await dbContext.Invitations
            .FirstOrDefaultAsync(i => i.Id == new InvitationId(request.InvitationId), ct);

        if (invitation is null)
            return Error.NotFound("Invitation.NotFound", "Invitation not found.");

        if (invitation.Status == InvitationStatus.Accepted)
            return Error.Conflict("Invitation.AlreadyAccepted", "Accepted invitations cannot be revoked.");

        dbContext.Invitations.Remove(invitation);
        return Result.Deleted;
    }
}
