
using AccountService.Application.Domain.Abstractions.Tenant;
using AccountService.Application.Domain.Organization.ValueObjects;
using AccountService.Application.Domain.User.ValueObjects;

namespace AccountService.Application.Domain.Organization;

public class Member : TenantEntity<MemberId>
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
