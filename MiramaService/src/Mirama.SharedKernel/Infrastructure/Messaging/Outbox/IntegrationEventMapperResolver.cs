
using Microsoft.Extensions.DependencyInjection;
using Mirama.SharedKernel.Abstractions.Domain.Events;

namespace Mirama.SharedKernel.Infrastructure.Messaging.Outbox;

public sealed class IntegrationEventMapperResolver(IServiceProvider serviceProvider) : IIntegrationEventMapperResolver
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public IReadOnlyList<IIntegrationEvent> Map(IDomainEvent domainEvent)
    {
        ArgumentNullException.ThrowIfNull(domainEvent);

        var mapperType = typeof(IIntegrationEventMapper<>).MakeGenericType(domainEvent.GetType());
        var mappers = _serviceProvider.GetServices(mapperType).OfType<object>();

        var results = new List<IIntegrationEvent>();
        foreach (dynamic mapper in mappers)
        {
            IEnumerable<IIntegrationEvent> mapped = mapper.Map((dynamic)domainEvent);
            results.AddRange(mapped);
        }

        return results;
    }
}
