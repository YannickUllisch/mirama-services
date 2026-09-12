
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Mirama.SharedKernel.Infrastructure.Messaging.Outbox;

public static class OutboxCleanupServiceCollectionExtensions
{
    public static IServiceCollection AddOutboxCleanup<TDbContext>(
        this IServiceCollection services,
        IConfiguration configuration,
        string moduleName)
        where TDbContext : DbContext
    {
        services.Configure<OutboxCleanupOptions>(moduleName, configuration.GetSection($"OutboxCleanup:{moduleName}"));

        services.AddSingleton<IHostedService>(sp => new OutboxCleanupWorker<TDbContext>(
            sp.GetRequiredService<IServiceScopeFactory>(),
            sp.GetRequiredService<IOptionsMonitor<OutboxCleanupOptions>>(),
            sp.GetRequiredService<ILogger<OutboxCleanupWorker<TDbContext>>>(),
            moduleName));

        return services;
    }
}
