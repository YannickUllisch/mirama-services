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

    private Invitation(InvitationDetails details)
    {
        Email = details.Email.Trim();
        Name = details.Name.Trim();
        InviterId = details.InviterId;
        IamRoleId = details.IamRoleId;
        Status = InvitationStatus.Pending;
        ExpiresAt = DateTime.UtcNow.AddDays(7);
    }

    private Invitation() { }

    public static Invitation Create(InvitationDetails details)
    {
        return new Invitation(details) { Id = new InvitationId(Guid.NewGuid()) };
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
