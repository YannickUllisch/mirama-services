
using AccountService.Domain.Common;
using AccountService.Domain.Organization.Invitation.ValueObjects;
using AccountService.Domain.Organization.ValueObjects;
using AccountService.Domain.User.ValueObjects;

namespace AccountService.Domain.Organization.Invitation;

public class Invitation : AuditableEntity
{
    public InvitationId Id { get; private set; }

    public OrganizationId OrganizationId { get; private set; }

    public string Email { get; private set; }

    public OrganizationRole Role { get; private set; }

    public InvitationStatus Status { get; private set; }

    public DateTime ExpiresAt { get; private set; }

    public UserId? InviterId { get; set; } = default!;

    private UserId? _acceptedBy = null;
    
    private Invitation(OrganizationId orgId, string email, OrganizationRole role, UserId inviterId, string createdBy)
    {
        Id = new InvitationId(new Guid());
        OrganizationId = orgId;
        Email = email;
        Status = InvitationStatus.Pending;
        Created = DateTime.UtcNow;
        CreatedBy = createdBy;
        ExpiresAt = DateTime.Today.AddDays(7);
        InviterId = inviterId;
    }

    public static Invitation Create(OrganizationId orgId, string email, OrganizationRole role, UserId inviteeId, string createdBy)
    {
        return new Invitation(orgId, email, role, inviteeId, createdBy);
    }

    public void Accept(UserId userId, string modifiedBy)
    {
        Status = InvitationStatus.Accepted;
        _acceptedBy = userId;
        LastModified = DateTime.UtcNow;
        LastModifiedBy = modifiedBy;
    }

    public void Decline(string modifiedBy)
    {
        Status = InvitationStatus.Declined;
        LastModified = DateTime.UtcNow;
        LastModifiedBy = modifiedBy;
    }

    public void ExtendFromToday(string modifiedBy)
    {
        ExpiresAt = DateTime.Today.AddDays(7);
        LastModified = DateTime.UtcNow;
        LastModifiedBy = modifiedBy;
    }
}