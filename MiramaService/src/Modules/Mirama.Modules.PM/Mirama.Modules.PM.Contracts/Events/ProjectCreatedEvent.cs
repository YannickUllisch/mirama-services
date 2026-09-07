
using Mirama.SharedKernel.Abstractions.Domain.Events;

namespace Mirama.Modules.PM.Contracts.Events;

public sealed record ProjectCreatedEvent(
    Guid EventId,
    Guid ProjectId,
    string Name) : IIntegrationEvent
{
    public DateTime OccurredAt { get; init; } = DateTime.UtcNow;
    public Guid AggregateId => ProjectId;
}
