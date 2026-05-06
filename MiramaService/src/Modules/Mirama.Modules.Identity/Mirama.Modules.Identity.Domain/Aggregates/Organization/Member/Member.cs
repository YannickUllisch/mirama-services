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

    private Member(MemberDetails details)
    {
        Name = details.Name.Trim();
        Email = details.Email.Trim();
        IamRoleId = details.IamRoleId;
        UserId = details.UserId;
    }

    private Member() { }

    public static Member Create(MemberDetails details)
    {
        return new Member(details) { Id = new MemberId(Guid.NewGuid()) };
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
