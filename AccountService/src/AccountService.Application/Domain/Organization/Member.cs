
using AccountService.Application.Domain.Abstractions;
using AccountService.Application.Domain.Organization.ValueObjects;
using AccountService.Application.Domain.User.ValueObjects;

namespace AccountService.Application.Domain.Organization;

public class Member : Entity<MemberId>
{
    public OrganizationId OrganizationId { get; private set; } = default!;

    public UserId UserId { get; private set; } = default!;

    public OrganizationRole Role { get; private set; }

    private Member(OrganizationId organizationId, UserId userId, OrganizationRole role)
    {
        OrganizationId = organizationId;
        UserId = userId;
        Role = role;
    }

    private Member() { }

    public static Member Create(OrganizationId organizationId, UserId userId, OrganizationRole role)
    {
        return new Member(organizationId, userId, role);
    }

    public void SetRole(OrganizationRole role)
    {
        Role = role;
    }
}
