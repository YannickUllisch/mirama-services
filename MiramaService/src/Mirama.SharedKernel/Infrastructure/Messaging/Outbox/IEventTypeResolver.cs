namespace Mirama.SharedKernel.Infrastructure.Messaging.Outbox;

public interface IEventTypeResolver
{
    Type Resolve(string typeName);

    Type? TryResolve(string typeName);
}
