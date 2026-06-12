using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.Identity.Domain.Aggregates.Organization.Invitation;
using Mirama.Modules.Identity.Infrastructure.Persistence;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.Identity.Application.Features.V1.Organizations.Invitations.ExtendInvitation;

public class ExtendInvitationController : OrganizationControllerBase
{
    [HttpPut("invitations/{invitationId:guid}/extend")]
    public async Task<ActionResult<InvitationResponse>> Extend([FromRoute] Guid invitationId)
    {
        var result = await this.Dispatcher.Send(new ExtendInvitationCommand(invitationId));
        return result.Match(Ok, Problem);
    }
}

public sealed record ExtendInvitationCommand(Guid InvitationId) : ICommand<ErrorOr<InvitationResponse>>;

internal class ExtendInvitationCommandHandler(
    IdentityDbContext dbContext) : IRequestHandler<ExtendInvitationCommand, ErrorOr<InvitationResponse>>
{
    public async Task<ErrorOr<InvitationResponse>> HandleAsync(ExtendInvitationCommand request, CancellationToken ct)
    {
        var invitation = await dbContext.Invitations
            .FirstOrDefaultAsync(i => i.Id == new InvitationId(request.InvitationId), ct);

        if (invitation is null)
            return Error.NotFound("Invitation.NotFound", "Invitation not found.");

        if (invitation.Status != InvitationStatus.Pending)
            return Error.Conflict("Invitation.NotPending", "Only pending invitations can be extended.");

        invitation.ExtendFromToday();

        return invitation.MapResponse();
    }
}
