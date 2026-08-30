using Mirama.SharedKernel.Abstractions.Domain.Events;

namespace Mirama.Modules.Clients.Domain.Events;

public sealed record ContractTerminated(
    Guid ContractId,
    string PartyType,
    Guid PartyId) : IDomainEvent
{
    public DateTime OccurredAt { get; init; } = DateTime.UtcNow;
}
