
using Mirama.SharedKernel.Abstractions.Domain.Events;

namespace Mirama.Modules.Clients.Contracts.Events;

public sealed record ClientCreatedEvent(
    Guid EventId,
    Guid ClientId,
    Guid TenantId,
    Guid OrganizationId,
    string Name,
    string ClientType) : IIntegrationEvent
{
    public DateTime OccurredAt { get; init; } = DateTime.UtcNow;

    public Guid AggregateId => ClientId;
}
