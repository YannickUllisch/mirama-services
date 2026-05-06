using Mirama.Modules.Identity.Domain.Aggregates.Role;
using Mirama.Modules.Identity.Domain.Aggregates.User;
using Mirama.SharedKernel.Abstractions.Domain.Core;

namespace Mirama.Modules.Identity.Domain.Aggregates.Organization.Member;

public class Member : OrganizationEntity<MemberId>
{
    public string Name { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public UserId? UserId { get; private set; }
    public RoleId IamRoleId { get; private set; } = default!;

    private Member(string name, string email, RoleId iamRoleId, UserId? userId)
    {
        Name = name;
        Email = email;
        IamRoleId = iamRoleId;
        UserId = userId;
    }

    private Member() { }

    public static Member Create(string name, string email, RoleId iamRoleId, UserId? userId = null)
    {
        return new Member(name, email, iamRoleId, userId);
    }

    public void SetRole(RoleId iamRoleId)
    {
        IamRoleId = iamRoleId;
    }

    public void LinkUser(UserId userId)
    {
        UserId = userId;
    }
}
