using Mirama.SharedKernel.Abstractions.Domain.Events;

namespace Mirama.Modules.Clients.Domain.Events;

public sealed record IntakeFormSubmitted(
    Guid SubmissionId,
    Guid IntakeFormId) : IDomainEvent
{
    public DateTime OccurredAt { get; init; } = DateTime.UtcNow;
}
