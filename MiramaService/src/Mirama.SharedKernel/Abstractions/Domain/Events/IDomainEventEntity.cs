
namespace Mirama.SharedKernel.Abstractions.Domain.Events;

public interface IDomainEventEntity
{
    IReadOnlyCollection<IDomainEvent> GetDomainEvents();
    void ClearDomainEvents();
}