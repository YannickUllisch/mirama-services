

using AccountService.Application.Domain.Abstractions.Core;
using AccountService.Application.Domain.Aggregates.User;

namespace AccountService.Application.Domain.Aggregates.Organization.Invitation;

public class Invitation : Entity<InvitationId>, IOrganizationOwned
{
    public string Email { get; private set; } = string.Empty;

    public OrganizationRole Role { get; private set; }

    public InvitationStatus Status { get; private set; }

    public DateTime ExpiresAt { get; private set; }

    public UserId InviterId { get; set; } = default!;

    public UserId? AcceptedBy = null;

    public OrganizationId OrganizationId { get; private set; } = default!;


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

    void IOrganizationOwned.SetOrganizationId(Guid organizationId)
    {
        if (OrganizationId.Value != Guid.Empty)
        {
            throw new InvalidOperationException("OrganizationId already set.");
        }
        OrganizationId = new OrganizationId(organizationId);;
    }
}