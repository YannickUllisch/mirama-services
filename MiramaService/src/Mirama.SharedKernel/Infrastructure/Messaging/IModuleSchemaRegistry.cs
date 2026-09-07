
using System.Reflection;

namespace Mirama.SharedKernel.Infrastructure.Messaging;

public interface IModuleSchemaRegistry
{
    void Register(Assembly moduleAssembly, string schemaName);

    string ResolveSchema(Assembly handlerAssembly);
}
