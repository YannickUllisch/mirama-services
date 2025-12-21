
namespace AccountService.Application.Domain.Abstractions.Core;

public interface IDomainEventEntity 
{
    IReadOnlyCollection<IDomainEvent> GetDomainEvents();
    void ClearDomainEvents();   
}