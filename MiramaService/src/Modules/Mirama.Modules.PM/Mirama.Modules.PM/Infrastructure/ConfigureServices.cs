using System.Reflection;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Mirama.Modules.PM.Application.Common;
using Mirama.Modules.PM.Application.Common.Interfaces;
using Mirama.Modules.PM.Infrastructure.Persistence;
using Mirama.Modules.PM.Infrastructure.Persistence.Repositories;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Abstractions.Persistence;
using Mirama.SharedKernel.Infrastructure.Options;

namespace Mirama.Modules.PM.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddProjectsModule(this IServiceCollection services, IConfiguration config)
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

        services.Decorate(typeof(IRequestHandler<,>), typeof(PMTransactionDecorator<,>));

        return services;
    }

    private static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.AddScoped(typeof(IPMCommandRepository<,>), typeof(PMCommandRepository<,>));
        services.AddScoped(typeof(IPMQueryRepository<,>), typeof(PMQueryRepository<,>));

        services.AddDbContext<PMDbContext>(static (sp, options) =>
        {
            var infra = sp.GetRequiredService<IOptions<InfrastructureOptions>>().Value;
            options
                .UseNpgsql(infra.DatabaseConnection, b => b
                    .MigrationsAssembly(typeof(PMDbContext).Assembly.FullName)
                    .MigrationsHistoryTable("__EFMigrationsHistory", "projects"))
                .AddInterceptors(sp.GetRequiredService<Mirama.SharedKernel.Infrastructure.Interceptors.AuditSaveChangesInterceptor>());
        });

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<PMDbContext>());
        services.AddScoped<IModuleMigrator, PMModuleMigrator>();

        return services;
    }
}
