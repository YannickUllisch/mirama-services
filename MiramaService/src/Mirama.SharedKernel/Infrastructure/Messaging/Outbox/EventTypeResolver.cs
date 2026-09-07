
using System.Reflection;
using Mirama.SharedKernel.Abstractions.Domain.Events;

namespace Mirama.SharedKernel.Infrastructure.Messaging.Outbox;

public sealed class EventTypeResolver : IEventTypeResolver
{
    private readonly Dictionary<string, Type> _typesByName;

    public EventTypeResolver(Assembly contractsAssembly)
    {
        _typesByName = contractsAssembly.GetTypes()
            .Where(t => t is { IsClass: true, IsAbstract: false } && typeof(IIntegrationEvent).IsAssignableFrom(t))
            .ToDictionary(t => t.Name, t => t, StringComparer.Ordinal);
    }

    public Type Resolve(string typeName) =>
        TryResolve(typeName)
        ?? throw new InvalidOperationException(
            $"No IIntegrationEvent type named '{typeName}' found in the configured Contracts assembly. " +
            "This usually means an event type was renamed after messages using the old name were already outboxed.");

    public Type? TryResolve(string typeName) =>
        _typesByName.GetValueOrDefault(typeName);
}
