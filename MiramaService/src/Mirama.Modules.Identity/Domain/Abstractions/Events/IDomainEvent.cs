
namespace Mirama.Modules.Identity.Domain.Abstractions.Events;

public interface IDomainEvent
{
    DateTime OccurredAt { get; }
}