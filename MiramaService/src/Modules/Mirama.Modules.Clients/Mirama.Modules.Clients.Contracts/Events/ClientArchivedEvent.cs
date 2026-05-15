using Mirama.SharedKernel.Abstractions.Domain.Events;

namespace Mirama.Modules.Clients.Contracts.Events;

public sealed record ClientArchivedEvent(
    Guid ClientId,
    Guid TenantId,
    Guid OrganizationId) : IDomainEvent
{
    public DateTime OccurredAt { get; init; } = DateTime.UtcNow;
}
