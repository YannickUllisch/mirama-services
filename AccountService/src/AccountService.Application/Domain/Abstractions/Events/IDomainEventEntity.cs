
namespace AccountService.Application.Domain.Abstractions.Events;

public interface IDomainEventEntity 
{
    IReadOnlyCollection<IDomainEvent> GetDomainEvents();
    void ClearDomainEvents();   
}