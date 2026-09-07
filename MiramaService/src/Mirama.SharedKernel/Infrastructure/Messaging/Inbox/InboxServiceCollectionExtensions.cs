
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Mirama.SharedKernel.Infrastructure.Messaging.Inbox;

public static class InboxServiceCollectionExtensions
{
    public static IServiceCollection AddInboxProcessor<TDbContext>(
        this IServiceCollection services,
        IConfiguration configuration,
        string moduleName)
        where TDbContext : DbContext
    {
        services.Configure<InboxOptions>(moduleName, configuration.GetSection($"Inbox:{moduleName}"));

        services.AddSingleton<IHostedService>(sp =>
        {
            using (var scope = sp.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<TDbContext>();
                var entityType = db.Model.FindEntityType(typeof(InboxMessage))
                    ?? throw new InvalidOperationException($"{typeof(TDbContext).Name} does not map InboxMessage.");
                var schema = entityType.GetSchema()
                    ?? throw new InvalidOperationException($"InboxMessage has no schema configured on {typeof(TDbContext).Name}.");
                sp.GetRequiredService<IModuleSchemaRegistry>().Register(typeof(TDbContext).Assembly, schema);
            }

            return new InboxProcessor<TDbContext>(
                sp.GetRequiredService<IServiceScopeFactory>(),
                sp.GetRequiredService<IOptionsMonitor<InboxOptions>>(),
                sp.GetRequiredService<ILogger<InboxProcessor<TDbContext>>>(),
                moduleName);
        });

        return services;
    }
}
