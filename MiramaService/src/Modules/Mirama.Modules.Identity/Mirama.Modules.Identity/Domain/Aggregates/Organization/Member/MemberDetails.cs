using Mirama.Modules.Identity.Domain.Aggregates.Role;
using Mirama.Modules.Identity.Domain.Aggregates.User;

namespace Mirama.Modules.Identity.Domain.Aggregates.Organization.Member;

public sealed record MemberDetails(
    string Name,
    string Email,
    RoleId IamRoleId,
    UserId? UserId = null
);
