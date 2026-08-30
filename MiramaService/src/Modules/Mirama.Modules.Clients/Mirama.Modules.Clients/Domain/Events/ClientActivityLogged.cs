using Mirama.SharedKernel.Abstractions.Domain.Events;

namespace Mirama.Modules.Clients.Domain.Events;

public sealed record ClientActivityLogged(
    Guid ClientId,
    Guid ActivityLogEntryId,
    string ActivityType) : IDomainEvent
{
    public DateTime OccurredAt { get; init; } = DateTime.UtcNow;
}
