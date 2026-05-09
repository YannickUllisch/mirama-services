using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.Identity.Domain.Aggregates.Organization.Invitation;
using Mirama.Modules.Identity.Domain.Aggregates.Role;
using Mirama.Modules.Identity.Infrastructure.Persistence;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Abstractions.Persistence;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.Identity.Application.Features.V1.Organizations.Invitations.SendInvitation;

public class SendInvitationController : OrganizationControllerBase
{
    [HttpPost("invitations")]
    public async Task<ActionResult<InvitationResponse>> Send([FromBody] SendInvitationCommand command)
    {
        var result = await this.Dispatcher.Send(command);
        return result.Match(r => CreatedAtAction(nameof(Send), new { invitationId = r.Id }, r), Problem);
    }
}

internal class SendInvitationCommandHandler(
    IdentityDbContext dbContext,
    IRequestContextProvider contextProvider) : IRequestHandler<SendInvitationCommand, ErrorOr<InvitationResponse>>
{
    public async Task<ErrorOr<InvitationResponse>> HandleAsync(SendInvitationCommand request, CancellationToken ct)
    {
        var organizationId = contextProvider.OrganizationId;
        var userId = contextProvider.UserId;

        if (organizationId is null)
            return Error.Unauthorized("Invitation.NoContext", "Organization and user context required.");

        var roleExists = await dbContext.Roles
            .AsNoTracking()
            .AnyAsync(r => r.Id == new RoleId(request.IamRoleId), ct);

        if (!roleExists)
            return Error.NotFound("Invitation.Role.NotFound", "Role not found.");

        var existing = await dbContext.Invitations
            .AsNoTracking()
            .AnyAsync(i => i.Email == request.Email.Trim() && i.Status == InvitationStatus.Pending, ct);

        if (existing)
            return Error.Conflict("Invitation.Duplicate", "A pending invitation for this email already exists.");

        var details = new InvitationDetails(request.Email, request.Name, userId, new RoleId(request.IamRoleId));
        var invitation = Invitation.Create(details);

        dbContext.Invitations.Add(invitation);

        return invitation.MapResponse() with { OrganizationId = organizationId.Value };
    }
}
