using Mirama.Modules.Clients.Domain.Aggregates.Client;
using Mirama.Modules.Clients.Domain.Enums;
using Mirama.SharedKernel.Abstractions.Domain.Core;

namespace Mirama.Modules.Clients.Domain.Aggregates.Client.ActivityLog;

// An insert-only record, not a mutable entity. A call happened, a note was
// left, a meeting occurred, these are facts about the past, not current state.
// There is deliberately no Update method here, only Create.
public class ClientActivityLogEntry : OrganizationEntity<ClientActivityLogEntryId>
{
    public ClientId ClientId { get; init; } = null!;
    public ClientActivityType Type { get; init; }
    public string Body { get; init; } = string.Empty;
    public Guid LoggedByMemberId { get; init; }
    public DateTime LoggedAt { get; init; }

    private ClientActivityLogEntry() { }

    private ClientActivityLogEntry(ClientActivityType type, string body, Guid loggedByMemberId)
    {
        this.Type = type;
        this.Body = body.Trim();
        this.LoggedByMemberId = loggedByMemberId;
        this.LoggedAt = DateTime.UtcNow;
    }

    public static ClientActivityLogEntry Create(ClientActivityType type, string body, Guid loggedByMemberId)
    {
        if (string.IsNullOrWhiteSpace(body))
            throw new ArgumentException("Activity body cannot be empty.", nameof(body));

        return new ClientActivityLogEntry(type, body, loggedByMemberId)
        {
            Id = new ClientActivityLogEntryId(Guid.NewGuid())
        };
    }
}
