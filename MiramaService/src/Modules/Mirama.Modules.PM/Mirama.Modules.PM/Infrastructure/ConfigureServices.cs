using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Mirama.Modules.PM.Application.Common;
using Mirama.Modules.Projects.Infrastructure.Persistence;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Abstractions.Persistence;
using Mirama.SharedKernel.Infrastructure.Options;

namespace Mirama.Modules.PM.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddProjectsModule(this IServiceCollection services, IConfiguration config)
    {
        services.AddApplication(config);
        services.AddInfrastructure(config);

        return services;
    }

    private static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration config)
    {
        var assembly = Assembly.GetExecutingAssembly();

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
        services.AddDbContext<ProjectsDbContext>(static (sp, options) =>
        {
            var infra = sp.GetRequiredService<IOptions<InfrastructureOptions>>().Value;
            options
                .UseNpgsql(infra.DatabaseConnection, b => b
                    .MigrationsAssembly(typeof(ProjectsDbContext).Assembly.FullName)
                    .MigrationsHistoryTable("__EFMigrationsHistory", "projects"))
                .AddInterceptors(sp.GetRequiredService<Mirama.SharedKernel.Infrastructure.Interceptors.AuditSaveChangesInterceptor>());
        });

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ProjectsDbContext>());

        return services;
    }
}
