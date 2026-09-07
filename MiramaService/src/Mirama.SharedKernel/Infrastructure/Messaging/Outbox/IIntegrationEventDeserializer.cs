
namespace Mirama.SharedKernel.Infrastructure.Messaging.Outbox;

public interface IIntegrationEventDeserializer
{
    Type Resolve(string typeName);

    Type? TryResolve(string typeName);
}
