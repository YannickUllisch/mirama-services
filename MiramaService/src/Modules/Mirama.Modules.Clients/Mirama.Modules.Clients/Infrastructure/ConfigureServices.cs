using System.Reflection;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Mirama.Modules.Clients.Application.Common;
using Mirama.Modules.Clients.Infrastructure.Persistence;
using Mirama.Modules.Clients.Infrastructure.Persistence.Repositories;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Abstractions.Persistence;
using Mirama.SharedKernel.Infrastructure.Options;

namespace Mirama.Modules.Clients.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddClientsModule(this IServiceCollection services, IConfiguration config)
    {
        services.AddApplication();
        services.AddInfrastructure(config);
        return services;
    }

    private static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        services.AddValidatorsFromAssembly(assembly, includeInternalTypes: true);

        services.Scan(scan => scan
            .FromAssemblies(assembly)
            .AddClasses(classes => classes.AssignableTo(typeof(IRequestHandler<,>)), publicOnly: false)
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        services.Scan(scan => scan
            .FromAssemblies(assembly)
            .AddClasses(classes => classes.AssignableTo(typeof(INotificationHandler<>)), publicOnly: false)
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        services.Decorate(typeof(IRequestHandler<,>), typeof(ClientsTransactionDecorator<,>));

        return services;
    }

    private static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.AddScoped(typeof(IClientsQueryRepository<,>), typeof(ClientsQueryRepository<,>));
        services.AddScoped(typeof(IClientsCommandRepository<,>), typeof(ClientsCommandRepository<,>));

        services.AddDbContext<ClientsDbContext>(static (sp, options) =>
        {
            var infra = sp.GetRequiredService<IOptions<InfrastructureOptions>>().Value;
            options
                .UseNpgsql(infra.DatabaseConnection, b => b
                    .MigrationsAssembly(typeof(ClientsDbContext).Assembly.FullName)
                    .MigrationsHistoryTable("__EFMigrationsHistory", schema: "clients"))
                .AddInterceptors(sp.GetRequiredService<Mirama.SharedKernel.Infrastructure.Interceptors.AuditSaveChangesInterceptor>());
        });

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ClientsDbContext>());

        return services;
    }
}
