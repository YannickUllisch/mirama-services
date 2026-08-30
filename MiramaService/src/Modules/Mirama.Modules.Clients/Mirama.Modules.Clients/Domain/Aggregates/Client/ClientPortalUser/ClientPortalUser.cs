using Mirama.Modules.Clients.Domain.Aggregates.Client;
using Mirama.Modules.Clients.Domain.Enums;
using Mirama.SharedKernel.Abstractions.Domain.Core;

namespace Mirama.Modules.Clients.Domain.Aggregates.Client.ClientPortalUser;

public class ClientPortalUser : OrganizationEntity<ClientPortalUserId>
{
    public ClientId ClientId { get; init; } = null!;
    public Guid ContactId { get; init; }
    public ClientPortalRole Role { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime? LastLogin { get; private set; }

    private ClientPortalUser() { }

    private ClientPortalUser(Guid contactId, ClientPortalRole role)
    {
        this.ContactId = contactId;
        this.Role = role;
        this.IsActive = true;
    }

    public static ClientPortalUser Create(Guid contactId, ClientPortalRole role)
    {
        return new ClientPortalUser(contactId, role) { Id = new ClientPortalUserId(Guid.NewGuid()) };
    }

    // Auth and session issuance are out of scope here by design, this only records
    // that a login happened, not how it was verified.
    public void RecordLogin() => LastLogin = DateTime.UtcNow;

    public void Revoke() => IsActive = false;

    public void Reactivate() => IsActive = true;

    public void ChangeRole(ClientPortalRole role) => Role = role;
}
