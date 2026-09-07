
using System.Collections.Concurrent;
using System.Reflection;

namespace Mirama.SharedKernel.Infrastructure.Messaging;

public sealed class ModuleSchemaRegistry : IModuleSchemaRegistry
{
    private readonly ConcurrentDictionary<Assembly, string> _schemaByAssembly = new();

    public void Register(Assembly moduleAssembly, string schemaName) =>
        _schemaByAssembly[moduleAssembly] = schemaName;

    public string ResolveSchema(Assembly handlerAssembly) =>
        _schemaByAssembly.TryGetValue(handlerAssembly, out var schema)
            ? schema
            : throw new InvalidOperationException(
                $"No module schema registered for assembly '{handlerAssembly.GetName().Name}'. " +
                "Every module that hosts an integration event handler must call AddInboxProcessor<TDbContext> " +
                "during startup so outbox fan-out knows which schema to route its Inbox rows into.");
}
