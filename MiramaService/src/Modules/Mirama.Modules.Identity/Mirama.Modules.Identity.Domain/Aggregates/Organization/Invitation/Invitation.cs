using Mirama.Modules.Identity.Domain.Aggregates.Role;
using Mirama.SharedKernel.Abstractions.Domain.Core;

namespace Mirama.Modules.Identity.Domain.Aggregates.Organization.Invitation;

public class Invitation : OrganizationEntity<InvitationId>
{
    public string Email { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public Guid InviterId { get; private set; }
    public InvitationStatus Status { get; private set; }
    public DateTime ExpiresAt { get; private set; }
    public RoleId IamRoleId { get; private set; } = default!;

    private Invitation(string email, string name, Guid inviterId, RoleId iamRoleId)
    {
        Email = email;
        Name = name;
        InviterId = inviterId;
        IamRoleId = iamRoleId;
        Status = InvitationStatus.Pending;
        ExpiresAt = DateTime.UtcNow.AddDays(7);
    }

    private Invitation() { }

    public static Invitation Create(string email, string name, Guid inviterId, RoleId iamRoleId)
    {
        return new Invitation(email, name, inviterId, iamRoleId);
    }

    public void Accept()
    {
        Status = InvitationStatus.Accepted;
    }

    public void Decline()
    {
        Status = InvitationStatus.Declined;
    }

    public void ExtendFromToday()
    {
        ExpiresAt = DateTime.UtcNow.AddDays(7);
    }
}
