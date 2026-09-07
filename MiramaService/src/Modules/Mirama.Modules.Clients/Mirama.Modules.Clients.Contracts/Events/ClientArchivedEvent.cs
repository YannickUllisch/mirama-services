
using Mirama.SharedKernel.Abstractions.Domain.Events;

namespace Mirama.Modules.Clients.Contracts.Events;

public sealed record ClientArchivedEvent(
    Guid EventId,
    Guid ClientId,
    Guid TenantId,
    Guid OrganizationId) : IIntegrationEvent
{
    public DateTime OccurredAt { get; init; } = DateTime.UtcNow;

    public Guid AggregateId => ClientId;
}
