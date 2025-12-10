
using AccountService.Application.Domain.Abstractions;
using AccountService.Application.Domain.Organization.ValueObjects;
using AccountService.Application.Domain.User.ValueObjects;

namespace AccountService.Application.Domain.Organization;


public class Member : AuditableEntity
{
    public MemberId Id { get; private set; } = default!;

    public OrganizationId OrganizationId { get; private set; } = default!;

    public UserId UserId { get; private set; } = default!;

    public OrganizationRole Role { get; private set; }

    private Member(OrganizationId organizationId, UserId userId, OrganizationRole role)
    {
        Id = new MemberId(new Guid());
        OrganizationId = organizationId;
        UserId = userId;
        Role = role;
        Created = DateTime.UtcNow;
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
