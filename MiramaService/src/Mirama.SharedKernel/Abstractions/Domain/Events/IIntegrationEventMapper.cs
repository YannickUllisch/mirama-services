

namespace Mirama.SharedKernel.Abstractions.Domain.Events;

public interface IIntegrationEventMapper<in TDomainEvent> where TDomainEvent : IDomainEvent
{
    IEnumerable<IIntegrationEvent> Map(TDomainEvent domainEvent);
}
