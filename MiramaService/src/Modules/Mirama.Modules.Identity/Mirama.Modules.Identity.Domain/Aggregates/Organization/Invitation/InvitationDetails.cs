using Mirama.Modules.Identity.Domain.Aggregates.Role;

namespace Mirama.Modules.Identity.Domain.Aggregates.Organization.Invitation;

public sealed record InvitationDetails(
    string Email,
    string Name,
    Guid InviterId,
    RoleId IamRoleId
);
