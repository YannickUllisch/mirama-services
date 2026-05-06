using Mirama.SharedKernel.Abstractions.Domain.Events;

namespace Mirama.Modules.Projects.Contracts.Events;

public sealed record ProjectCreatedEvent(
    Guid ProjectId,
    Guid OrganizationId,
    Guid TenantId,
    string Name) : IDomainEvent
{
    public DateTime OccurredAt { get; init; } = DateTime.UtcNow;
}
