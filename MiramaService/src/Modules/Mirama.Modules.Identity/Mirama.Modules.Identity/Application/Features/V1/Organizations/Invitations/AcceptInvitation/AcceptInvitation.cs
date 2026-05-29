using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.Identity.Domain.Aggregates.Organization.Invitation;
using Mirama.Modules.Identity.Infrastructure.Persistence;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Abstractions.Persistence;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.Identity.Application.Features.V1.Organizations.Invitations.AcceptInvitation;

public class AcceptInvitationController : OrganizationControllerBase
{
    [HttpPut("invitations/{invitationId:guid}/accept")]
    public async Task<ActionResult<InvitationResponse>> Accept([FromRoute] Guid invitationId)
    {
        var result = await this.Dispatcher.Send(new AcceptInvitationCommand(invitationId));
        return result.Match(Ok, Problem);
    }
}

public sealed record AcceptInvitationCommand(Guid InvitationId) : ICommand<ErrorOr<InvitationResponse>>;

internal class AcceptInvitationCommandHandler(
    IdentityDbContext dbContext) : IRequestHandler<AcceptInvitationCommand, ErrorOr<InvitationResponse>>
{
    public async Task<ErrorOr<InvitationResponse>> HandleAsync(AcceptInvitationCommand request, CancellationToken ct)
    {
        var invitation = await dbContext.Invitations
            .FirstOrDefaultAsync(i => i.Id == new InvitationId(request.InvitationId), ct);

        if (invitation is null)
            return Error.NotFound("Invitation.NotFound", "Invitation not found.");

        if (invitation.Status != InvitationStatus.Pending)
            return Error.Conflict("Invitation.NotPending", "Only pending invitations can be accepted.");

        if (invitation.ExpiresAt < DateTime.UtcNow)
            return Error.Conflict("Invitation.Expired", "This invitation has expired.");

        invitation.Accept();

        return invitation.MapResponse();
    }
}
