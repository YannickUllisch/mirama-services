using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.Identity.Application.Features.V1.Organizations.Members;
using Mirama.Modules.Identity.Domain.Aggregates.Organization.Invitation;
using Mirama.Modules.Identity.Domain.Aggregates.Organization.Member;
using Mirama.Modules.Identity.Infrastructure.Persistence;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Abstractions.Domain.Core;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.Identity.Application.Features.V1.Organizations.Invitations.AcceptInvitation;

public class AcceptInvitationController : OrganizationControllerBase
{
    [HttpPut("invitations/{invitationId:guid}/accept")]
    public async Task<ActionResult<MemberResponse>> Accept([FromRoute] Guid invitationId)
    {
        var result = await this.Dispatcher.Send(new AcceptInvitationCommand(invitationId));
        return result.Match(Ok, Problem);
    }
}

public sealed record AcceptInvitationCommand(Guid InvitationId) : ICommand<ErrorOr<MemberResponse>>;

internal class AcceptInvitationCommandHandler(IdentityDbContext dbContext) : IRequestHandler<AcceptInvitationCommand, ErrorOr<MemberResponse>>
{
    public async Task<ErrorOr<MemberResponse>> HandleAsync(AcceptInvitationCommand request, CancellationToken ct)
    {
        var invitation = await dbContext.Invitations
            .FirstOrDefaultAsync(i => i.Id == new InvitationId(request.InvitationId), ct);

        if (invitation is null)
            return Error.NotFound("Invitation.NotFound", "Invitation not found.");

        if (invitation.Status != InvitationStatus.Pending)
            return Error.Conflict("Invitation.NotPending", "Only pending invitations can be accepted.");

        if (invitation.ExpiresAt < DateTime.UtcNow)
            return Error.Conflict("Invitation.Expired", "This invitation has expired.");

        var alreadyMember = await dbContext.Members
            .AsNoTracking()
            .AnyAsync(m => m.Email == invitation.Email, ct);

        if (alreadyMember)
            return Error.Conflict("Member.Duplicate", "A member with this email already exists in the organization.");

        invitation.Accept();

        var member = Member.Create(new MemberDetails(invitation.Name, invitation.Email, invitation.IamRoleId));
        ((IOrganizationOwned)member).SetOrganizationId(invitation.OrganizationId);
        await dbContext.Members.AddAsync(member, ct);

        return member.MapResponse();
    }
}
