using Mirama.Modules.Clients.Domain.Aggregates.Client;
using Mirama.Modules.Clients.Domain.Enums;
using Mirama.SharedKernel.Abstractions.Domain.Core;

namespace Mirama.Modules.Clients.Domain.Aggregates.Client.ClientPortalInvitation;

public class ClientPortalInvitation : OrganizationEntity<ClientPortalInvitationId>
{
    public ClientId ClientId { get; init; } = null!;
    public Guid ContactId { get; init; }
    public Guid Token { get; private set; }
    public PortalInvitationStatus Status { get; private set; }
    public DateTime ExpiresAt { get; private set; }
    public DateTime SentAt { get; private set; }

    private ClientPortalInvitation() { }

    private ClientPortalInvitation(Guid contactId)
    {
        this.ContactId = contactId;
        this.Token = Guid.NewGuid();
        this.Status = PortalInvitationStatus.Pending;
        this.SentAt = DateTime.UtcNow;
        this.ExpiresAt = DateTime.UtcNow.AddDays(7);
    }

    public static ClientPortalInvitation Create(Guid contactId)
    {
        return new ClientPortalInvitation(contactId)
        {
            Id = new ClientPortalInvitationId(Guid.NewGuid())
        };
    }

    public bool IsValid() => Status == PortalInvitationStatus.Pending && ExpiresAt > DateTime.UtcNow;

    public void Accept() => Status = PortalInvitationStatus.Accepted;

    public void Revoke() => Status = PortalInvitationStatus.Revoked;

    public void MarkExpired() => Status = PortalInvitationStatus.Expired;
}
