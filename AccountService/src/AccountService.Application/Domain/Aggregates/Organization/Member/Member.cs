
using AccountService.Application.Domain.Abstractions.Organization;
using AccountService.Application.Domain.Aggregates.User;

namespace AccountService.Application.Domain.Aggregates.Organization.Member;

public class Member : OrganizationEntity<MemberId>
{
    public UserId UserId { get; private set; } = default!;

    public OrganizationRole Role { get; private set; }

    private Member(UserId userId, OrganizationRole role)
    {
        UserId = userId;
        Role = role;
    }

    private Member() { }

    public static Member Create(UserId userId, OrganizationRole role)
    {
        return new Member(userId, role);
    }

    public void SetRole(OrganizationRole role)
    {
        Role = role;
    }
}
