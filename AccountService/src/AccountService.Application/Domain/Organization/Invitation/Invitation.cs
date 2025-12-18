

using AccountService.Application.Domain.Abstractions.Tenant;
using AccountService.Application.Domain.Organization.Invitation.Valueobjects;
using AccountService.Application.Domain.Organization.ValueObjects;
using AccountService.Application.Domain.User.ValueObjects;
namespace AccountService.Application.Domain.Organization.Invitation;

public class Invitation : TenantEntity<InvitationId>
{
    public string Email { get; private set; } = string.Empty;

    public OrganizationRole Role { get; private set; }

    public InvitationStatus Status { get; private set; }

    public DateTime ExpiresAt { get; private set; }

    public UserId InviterId { get; set; } = default!;

    public UserId? AcceptedBy = null;

    private Invitation(string email, OrganizationRole role, UserId inviterId)
    {
        Email = email;
        Role = role;
        Status = InvitationStatus.Pending;
        ExpiresAt = DateTime.Today.AddDays(7);
        InviterId = inviterId;
    }

    private Invitation() { }


    public static Invitation Create(string email, OrganizationRole role, UserId inviteeId)
    {
        return new Invitation(email, role, inviteeId);
    }

    public void Accept(UserId userId)
    {
        Status = InvitationStatus.Accepted;
        AcceptedBy = userId;
    }

    public void Decline()
    {
        Status = InvitationStatus.Declined;
    }

    public void ExtendFromToday()
    {
        ExpiresAt = DateTime.Today.AddDays(7);
    }
}