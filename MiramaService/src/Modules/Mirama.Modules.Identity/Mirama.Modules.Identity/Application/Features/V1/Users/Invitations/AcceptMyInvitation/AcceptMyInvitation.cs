using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.Identity.Application.Features.V1.Organizations.Members;
using Mirama.Modules.Identity.Domain.Aggregates.Organization.Invitation;
using Mirama.Modules.Identity.Domain.Aggregates.Organization.Member;
using Mirama.Modules.Identity.Domain.Aggregates.User;
using Mirama.Modules.Identity.Infrastructure.Persistence;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Abstractions.Domain.Core;
using Mirama.SharedKernel.Abstractions.Persistence;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.Identity.Application.Features.V1.Users.Invitations.AcceptMyInvitation;

public class AcceptMyInvitationController : ApiControllerBase
{
    [HttpPost("users/invitations/{invitationId:guid}/accept")]
    public async Task<ActionResult<MemberResponse>> Accept([FromRoute] Guid invitationId)
    {
        var result = await this.Dispatcher.Send(new AcceptMyInvitationCommand(invitationId));
        return result.Match(Ok, Problem);
    }
}

public sealed record AcceptMyInvitationCommand(Guid InvitationId) : ICommand<ErrorOr<MemberResponse>>;

internal class AcceptMyInvitationCommandHandler(
    IdentityDbContext dbContext,
    IRequestContextProvider contextProvider) : IRequestHandler<AcceptMyInvitationCommand, ErrorOr<MemberResponse>>
{
    public async Task<ErrorOr<MemberResponse>> HandleAsync(AcceptMyInvitationCommand request, CancellationToken ct)
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
            return Error.Conflict("Invitation.NotPending", "Only pending invitations can be accepted.");

        if (invitation.ExpiresAt < DateTime.UtcNow)
            return Error.Conflict("Invitation.Expired", "This invitation has expired.");

        var alreadyMember = await dbContext.Members
            .AsNoTracking()
            .IgnoreQueryFilters()
            .AnyAsync(m => m.Email == user.Email && m.OrganizationId == invitation.OrganizationId, ct);

        if (alreadyMember)
            return Error.Conflict("Member.Duplicate", "You are already a member of this organization.");

        invitation.Accept();

        var member = Member.Create(new MemberDetails(user.Name, user.Email, invitation.IamRoleId, new UserId(userId)));
        ((IOrganizationOwned)member).SetOrganizationId(invitation.OrganizationId);
        dbContext.Members.Add(member);

        return new MemberResponse
        {
            Id = member.Id.Value,
            Name = member.Name,
            Email = member.Email,
            UserId = userId,
            IamRoleId = member.IamRoleId.Value,
            OrganizationId = invitation.OrganizationId,
        };
    }
}
