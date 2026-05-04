
namespace Mirama.Modules.Identity.Domain.Abstractions.Events;

public interface IDomainEventEntity
{
    IReadOnlyCollection<IDomainEvent> GetDomainEvents();
    void ClearDomainEvents();
}