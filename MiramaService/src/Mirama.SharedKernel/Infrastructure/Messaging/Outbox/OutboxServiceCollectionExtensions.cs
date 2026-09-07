
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Mirama.SharedKernel.Infrastructure.Messaging.Outbox;

public static class OutboxServiceCollectionExtensions
{
    public static IServiceCollection AddOutboxProcessor<TDbContext>(
        this IServiceCollection services,
        IConfiguration configuration,
        string moduleName,
        Assembly contractsAssembly)
        where TDbContext : DbContext
    {
        services.Configure<OutboxOptions>(moduleName, configuration.GetSection($"Outbox:{moduleName}"));

        services.AddSingleton<IHostedService>(sp => new OutboxProcessor<TDbContext>(
            sp.GetRequiredService<IServiceScopeFactory>(),
            sp.GetRequiredService<IOptionsMonitor<OutboxOptions>>(),
            new EventTypeResolver(contractsAssembly),
            sp.GetRequiredService<IModuleSchemaRegistry>(),
            sp.GetRequiredService<ILogger<OutboxProcessor<TDbContext>>>(),
            moduleName));

        return services;
    }
}
