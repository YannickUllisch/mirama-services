
using AccountService.Application.Domain.Abstractions.Core;
using AccountService.Application.Domain.Aggregates.User;

namespace AccountService.Application.Domain.Aggregates.Organization.Member;

public class Member : Entity<MemberId>, IOrganizationOwned
{
    public UserId UserId { get; private set; } = default!;

    public OrganizationRole Role { get; private set; }

    public OrganizationId OrganizationId { get; private set; } = default!;

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

    void IOrganizationOwned.SetOrganizationId(Guid organizationId)
    {
        if (OrganizationId.Value != Guid.Empty)
        {
            throw new InvalidOperationException("OrganizationId already set.");
        }
        OrganizationId = new OrganizationId(organizationId);
    }
}
