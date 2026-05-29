using Mirama.Modules.Clients.Domain.Enums;
using Mirama.Modules.Clients.Domain.Events;
using Mirama.SharedKernel.Abstractions.Domain.Core;
using ContactEntity = Mirama.Modules.Clients.Domain.Aggregates.Client.Contact.Contact;
using ClientPortalUserEntity = Mirama.Modules.Clients.Domain.Aggregates.Client.ClientPortalUser.ClientPortalUser;
using ClientPortalInvitationEntity = Mirama.Modules.Clients.Domain.Aggregates.Client.ClientPortalInvitation.ClientPortalInvitation;
using Mirama.Modules.Clients.Domain.Aggregates.Client.Contact;

namespace Mirama.Modules.Clients.Domain.Aggregates.Client;

public class Client : OrganizationAggregateRoot<ClientId>
{
    public string Name { get; private set; } = string.Empty;
    public ClientType Type { get; private set; }
    public ClientStatus Status { get; private set; }
    public string? Website { get; private set; }
    public string? Industry { get; private set; }
    public string? Notes { get; private set; }

    public List<ContactEntity> Contacts { get; private set; } = [];
    public List<ClientPortalInvitationEntity> PortalInvitations { get; private set; } = [];
    public List<ClientPortalUserEntity> PortalUsers { get; private set; } = [];

    private Client() { }

    private Client(ClientDetails details)
    {
        this.Name = details.Name.Trim();
        this.Type = details.Type;
        this.Status = ClientStatus.Prospect;
        this.Website = details.Website;
        this.Industry = details.Industry;
        this.Notes = details.Notes;
    }

    public static Client Create(ClientDetails details)
    {
        var client = new Client(details) { Id = new ClientId(Guid.NewGuid()) };
        client.AddDomainEvent(new ClientCreated(client.Id.Value, details.Type.ToString()));
        return client;
    }

    public void Update(ClientDetails details)
    {
        this.Name = details.Name.Trim();
        this.Website = details.Website;
        this.Industry = details.Industry;
        this.Notes = details.Notes;
    }

    public void Activate() => Status = ClientStatus.Active;

    public void Archive()
    {
        this.Status = ClientStatus.Archived;
        AddDomainEvent(new ClientArchived(Id.Value, OrganizationId));
    }

    public ContactEntity AddContact(ContactDetails details)
    {
        var contact = ContactEntity.Create(details);
        this.Contacts.Add(contact);
        AddDomainEvent(new ContactAdded(Id.Value, contact.Id.Value, contact.Email));
        return contact;
    }

    public ClientPortalInvitationEntity InviteContact(Guid contactId)
    {
        if (Contacts.All(c => c.Id.Value != contactId))
            throw new InvalidOperationException("Contact does not belong to this client.");

        var invitation = ClientPortalInvitationEntity.Create(contactId);
        this.PortalInvitations.Add(invitation);
        AddDomainEvent(new ClientPortalInvitationSent(Id.Value, contactId, invitation.Token));
        return invitation;
    }

    public ClientPortalUserEntity AcceptInvitation(Guid token)
    {
        var invitation = PortalInvitations.FirstOrDefault(i => i.Token == token)
            ?? throw new InvalidOperationException("Invitation not found.");

        if (!invitation.IsValid())
            throw new InvalidOperationException("Invitation is expired or already used.");

        invitation.Accept();

        var existingUser = PortalUsers.FirstOrDefault(u => u.ContactId == invitation.ContactId);
        if (existingUser is not null)
        {
            existingUser.Reactivate();
            return existingUser;
        }

        var portalUser = ClientPortalUserEntity.Create(invitation.ContactId);
        this.PortalUsers.Add(portalUser);
        return portalUser;
    }

    public void RevokePortalAccess(Guid contactId)
    {
        var user = this.PortalUsers.FirstOrDefault(u => u.ContactId == contactId);
        user?.Revoke();

        foreach (var pending in this.PortalInvitations.Where(i =>
            i.ContactId == contactId && i.Status == Enums.PortalInvitationStatus.Pending))
        {
            pending.Revoke();
        }
    }
}
