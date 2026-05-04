
namespace Mirama.SharedKernel.Abstractions.Domain.Events;

public interface IDomainEvent
{
    DateTime OccurredAt { get; }
}