using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.Identity.Domain.Aggregates.Organization.Invitation;
using Mirama.Modules.Identity.Infrastructure.Persistence;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Abstractions.Persistence;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.Identity.Application.Features.V1.Organizations.Invitations.DeclineInvitation;

public class DeclineInvitationController : OrganizationControllerBase
{
    [HttpPut("invitations/{invitationId:guid}/decline")]
    public async Task<ActionResult<InvitationResponse>> Decline([FromRoute] Guid invitationId)
    {
        var result = await this.Dispatcher.Send(new DeclineInvitationCommand(invitationId));
        return result.Match(Ok, Problem);
    }
}

public sealed record DeclineInvitationCommand(Guid InvitationId) : ICommand<ErrorOr<InvitationResponse>>;

internal class DeclineInvitationCommandHandler(
    IdentityDbContext dbContext) : IRequestHandler<DeclineInvitationCommand, ErrorOr<InvitationResponse>>
{
    public async Task<ErrorOr<InvitationResponse>> HandleAsync(DeclineInvitationCommand request, CancellationToken ct)
    {
        var invitation = await dbContext.Invitations
            .FirstOrDefaultAsync(i => i.Id == new InvitationId(request.InvitationId), ct);

        if (invitation is null)
            return Error.NotFound("Invitation.NotFound", "Invitation not found.");

        if (invitation.Status != InvitationStatus.Pending)
            return Error.Conflict("Invitation.NotPending", "Only pending invitations can be declined.");

        invitation.Decline();

        return invitation.MapResponse();
    }
}
