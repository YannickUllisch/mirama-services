
namespace Mirama.SharedKernel.Abstractions.Domain.Events;

public interface IIntegrationEventMapperResolver
{
    IReadOnlyList<IIntegrationEvent> Map(IDomainEvent domainEvent);
}
