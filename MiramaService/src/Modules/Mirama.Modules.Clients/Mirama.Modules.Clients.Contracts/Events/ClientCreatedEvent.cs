using Mirama.SharedKernel.Abstractions.Domain.Events;

namespace Mirama.Modules.Clients.Contracts.Events;

public sealed record ClientCreatedEvent(
    Guid ClientId,
    Guid TenantId,
    Guid OrganizationId,
    string Name,
    string ClientType) : IDomainEvent
{
    public DateTime OccurredAt { get; init; } = DateTime.UtcNow;
}
