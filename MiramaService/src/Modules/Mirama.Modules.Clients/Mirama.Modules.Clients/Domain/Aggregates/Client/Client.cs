using Mirama.Modules.Clients.Domain.Aggregates.Client.ActivityLog;
using Mirama.Modules.Clients.Domain.Aggregates.Client.PipelineHistory;
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
    public string? LeadSource { get; private set; }
    public Guid OwnerMemberId { get; private set; }

    public List<ContactEntity> Contacts { get; private set; } = [];
    public List<ClientPortalInvitationEntity> PortalInvitations { get; private set; } = [];
    public List<ClientPortalUserEntity> PortalUsers { get; private set; } = [];
    public List<ClientActivityLogEntry> ActivityLog { get; private set; } = [];
    public List<PipelineStageHistoryEntry> PipelineHistory { get; private set; } = [];

    private Client() { }

    private Client(ClientDetails details)
    {
        this.Name = details.Name.Trim();
        this.Type = details.Type;
        this.Status = ClientStatus.Prospect;
        this.Website = details.Website;
        this.Industry = details.Industry;
        this.Notes = details.Notes;
        this.LeadSource = details.LeadSource;
        this.OwnerMemberId = details.OwnerMemberId;
    }

    public static Client Create(ClientDetails details)
    {
        var client = new Client(details) { Id = new ClientId(Guid.NewGuid()) };
        client.PipelineHistory.Add(PipelineStageHistoryEntry.Create(null, ClientStatus.Prospect, details.OwnerMemberId));
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

    // Reassignment matters here specifically, an owning Member can leave or be offloaded
    // without the client relationship itself losing an owner.
    public void AssignOwner(Guid memberId) => this.OwnerMemberId = memberId;

    public void SetLeadSource(string? source) => this.LeadSource = source;

    // Pipeline stage moves. Each one is a named method rather than a generic
    // SetStatus(status), so an invalid jump (e.g. Archived straight to Qualify)
    // can be given real guard logic later without touching every call site.
    public void Qualify(Guid? changedByMemberId = null) => TransitionTo(ClientStatus.Qualified, changedByMemberId);

    public void SendProposal(Guid? changedByMemberId = null) => TransitionTo(ClientStatus.ProposalSent, changedByMemberId);

    public void Activate(Guid? changedByMemberId = null) => TransitionTo(ClientStatus.Active, changedByMemberId);

    public void Archive(Guid? changedByMemberId = null)
    {
        TransitionTo(ClientStatus.Archived, changedByMemberId);
        AddDomainEvent(new ClientArchived(Id.Value, OrganizationId));
    }

    private void TransitionTo(ClientStatus newStatus, Guid? changedByMemberId)
    {
        if (this.Status == newStatus) return;

        var previous = this.Status;
        this.Status = newStatus;
        this.PipelineHistory.Add(PipelineStageHistoryEntry.Create(previous, newStatus, changedByMemberId));
        AddDomainEvent(new ClientPipelineStageChanged(Id.Value, previous.ToString(), newStatus.ToString()));
    }

    public ClientActivityLogEntry LogActivity(ClientActivityType type, string body, Guid loggedByMemberId)
    {
        var entry = ClientActivityLogEntry.Create(type, body, loggedByMemberId);
        this.ActivityLog.Add(entry);
        AddDomainEvent(new ClientActivityLogged(Id.Value, entry.Id.Value, type.ToString()));
        return entry;
    }

    public ContactEntity AddContact(ContactDetails details)
    {
        var contact = ContactEntity.Create(details);
        this.Contacts.Add(contact);
        AddDomainEvent(new ContactAdded(Id.Value, contact.Id.Value, contact.Email));
        return contact;
    }

    public ClientPortalInvitationEntity InviteContact(Guid contactId, ClientPortalRole role, Guid invitedByMemberId)
    {
        if (Contacts.All(c => c.Id.Value != contactId))
            throw new InvalidOperationException("Contact does not belong to this client.");

        var invitation = ClientPortalInvitationEntity.Create(contactId, role, invitedByMemberId);
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
            existingUser.ChangeRole(invitation.Role);
            return existingUser;
        }

        var portalUser = ClientPortalUserEntity.Create(invitation.ContactId, invitation.Role);
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

    public void ChangePortalRole(Guid contactId, ClientPortalRole role)
    {
        var user = this.PortalUsers.FirstOrDefault(u => u.ContactId == contactId)
            ?? throw new InvalidOperationException("Contact has no active portal access.");
        user.ChangeRole(role);
    }
}
