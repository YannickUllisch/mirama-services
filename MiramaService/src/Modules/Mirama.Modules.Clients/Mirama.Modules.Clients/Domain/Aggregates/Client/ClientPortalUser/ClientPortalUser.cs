using Mirama.SharedKernel.Abstractions.Domain.Core;

namespace Mirama.Modules.Clients.Domain.Aggregates.Client.ClientPortalUser;

public class ClientPortalUser : OrganizationEntity<ClientPortalUserId>
{
    public Guid ClientId { get; init; }
    public Guid ContactId { get; init; }
    public bool IsActive { get; private set; }
    public DateTime? LastLogin { get; private set; }

    private ClientPortalUser() { }

    private ClientPortalUser(Guid contactId)
    {
        this.ContactId = contactId;
        this.IsActive = true;
    }

    public static ClientPortalUser Create(Guid contactId)
    {
        return new ClientPortalUser(contactId) { Id = new ClientPortalUserId(Guid.NewGuid()) };
    }

    public void RecordLogin() => LastLogin = DateTime.UtcNow;

    public void Revoke() => IsActive = false;

    public void Reactivate() => IsActive = true;
}
