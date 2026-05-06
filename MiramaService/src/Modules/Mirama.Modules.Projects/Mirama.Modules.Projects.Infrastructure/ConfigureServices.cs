using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Mirama.Modules.Projects.Infrastructure.Common.Options;
using Mirama.Modules.Projects.Infrastructure.Persistence;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Abstractions.Persistence;
using Mirama.SharedKernel.Models.Decorators;

namespace Mirama.Modules.Projects;

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

        services.Decorate(typeof(IRequestHandler<,>), typeof(TransactionDecorator<,>));

        return services;
    }

    private static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services
            .AddOptions<InfrastructureOptions>()
            .Bind(config.GetSection(InfrastructureOptions.Key))
            .Validate(o => !string.IsNullOrWhiteSpace(o.DatabaseConnection),
                      "Projects: valid DatabaseConnection is required.")
            .ValidateOnStart();

        services.AddDbContext<ProjectsDbContext>(static (sp, options) =>
        {
            var infra = sp.GetRequiredService<IOptions<InfrastructureOptions>>().Value;
            options.UseNpgsql(infra.DatabaseConnection, b => b
                .MigrationsAssembly(typeof(ProjectsDbContext).Assembly.FullName)
                .MigrationsHistoryTable("__EFMigrationsHistory", "projects"));
        });

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ProjectsDbContext>());

        return services;
    }
}
