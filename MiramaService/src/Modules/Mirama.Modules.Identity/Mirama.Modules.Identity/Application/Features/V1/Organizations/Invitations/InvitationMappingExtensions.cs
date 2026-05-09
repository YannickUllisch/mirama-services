using Mirama.Modules.Identity.Domain.Aggregates.Organization.Invitation;

namespace Mirama.Modules.Identity.Application.Features.V1.Organizations.Invitations;

internal static class InvitationMapper
{
    internal static InvitationResponse MapResponse(this Invitation invitation) => new()
    {
        Id = invitation.Id.Value,
        Email = invitation.Email,
        Name = invitation.Name,
        InviterId = invitation.InviterId,
        IamRoleId = invitation.IamRoleId.Value,
        Status = invitation.Status.ToString(),
        ExpiresAt = invitation.ExpiresAt,
        OrganizationId = invitation.OrganizationId
    };
}
