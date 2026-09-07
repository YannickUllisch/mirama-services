using Mirama.SharedKernel.Abstractions.Domain.Events;

namespace Mirama.Modules.PM.Domain.Events;

public sealed record ProjectCreated(
    Guid ProjectId,
    string Name) : IDomainEvent
{
    public DateTime OccurredAt { get; init; } = DateTime.UtcNow;
}
