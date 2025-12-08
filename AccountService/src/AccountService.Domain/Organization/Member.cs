
using AccountService.Domain.Common;
using AccountService.Domain.Organization.ValueObjects;
using AccountService.Domain.User.ValueObjects;

namespace AccountService.Domain.Organization;

public class Member : AuditableEntity
{
    public MemberId Id { get; private set; } = default!;

    public OrganizationId OrganizationId { get; private set; } = default!;

    public UserId UserId { get; private set; } = default!;

    public OrganizationRole Role { get; private set; }

    private Member(OrganizationId organizationId, UserId userId, OrganizationRole role, string createdBy)
    {
        Id = new MemberId(new Guid());
        OrganizationId = organizationId;
        UserId = userId;
        Role = role;
        Created = DateTime.UtcNow;
        CreatedBy = createdBy;
    }

    private Member() { }

    public static Member Create(OrganizationId organizationId, UserId userId, OrganizationRole role, string createdBy)
    {
        return new Member(organizationId, userId, role, createdBy);
    }

    public void SetRole(OrganizationRole role, string modifiedBy)
    {
        Role = role;
        LastModified = DateTime.UtcNow;
        LastModifiedBy = modifiedBy;
    }
}
