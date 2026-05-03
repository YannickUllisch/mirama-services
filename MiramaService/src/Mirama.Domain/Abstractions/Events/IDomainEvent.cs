
namespace Mirama.Domain.Abstractions.Events;

public interface IDomainEvent 
{
    DateTime OccurredAt { get; }
}