using Mirama.SharedKernel.Abstractions.Domain.Events;

namespace Mirama.Modules.Clients.Domain.Events;

public sealed record IntakeFormSubmissionConverted(
    Guid SubmissionId,
    Guid ClientId) : IDomainEvent
{
    public DateTime OccurredAt { get; init; } = DateTime.UtcNow;
}
