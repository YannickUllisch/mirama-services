using System.Reflection;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Mirama.Modules.Workspace.Application.Common;
using Mirama.Modules.Workspace.Application.Common.Interfaces;
using Mirama.Modules.Workspace.Infrastructure.Persistence;
using Mirama.Modules.Workspace.Infrastructure.Persistence.Repositories;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Abstractions.Domain.Events;
using Mirama.SharedKernel.Abstractions.Persistence;
using Mirama.SharedKernel.Infrastructure.Messaging.Inbox;
using Mirama.SharedKernel.Infrastructure.Messaging.Outbox;
using Mirama.SharedKernel.Infrastructure.Options;

namespace Mirama.Modules.Workspace.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddWorkspaceModule(this IServiceCollection services, IConfiguration config)
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

        services.Scan(scan => scan
            .FromAssemblies(assembly)
            .AddClasses(classes => classes.AssignableTo(typeof(IIntegrationEventMapper<>)), publicOnly: false)
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        // Module-specific decorator, avoids IUnitOfWork being overridden by other modules.
        services.Decorate(typeof(IRequestHandler<,>), typeof(WorkspaceTransactionDecorator<,>));

        return services;
    }

    private static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        var assembly = Assembly.GetExecutingAssembly();

        // Register all IModuleService implementations (cross-module service contracts),
        // e.g. IViewStateService for other modules to read a user's saved state synchronously.
        services.Scan(scan => scan
            .FromAssemblies(assembly)
            .AddClasses(classes => classes.AssignableTo<IModuleService>(), publicOnly: false)
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        services.AddScoped(typeof(IWorkspaceCommandRepository<,>), typeof(WorkspaceCommandRepository<,>));
        services.AddScoped(typeof(IWorkspaceQueryRepository<,>), typeof(WorkspaceQueryRepository<,>));

        services.AddDbContext<WorkspaceDbContext>(static (sp, options) =>
        {
            var infra = sp.GetRequiredService<IOptions<InfrastructureOptions>>().Value;
            options
                .UseNpgsql(infra.DatabaseConnection, b => b
                    .MigrationsAssembly(typeof(WorkspaceDbContext).Assembly.FullName)
                    .MigrationsHistoryTable("__EFMigrationsHistory", "workspace"))
                .AddInterceptors(
                    sp.GetRequiredService<Mirama.SharedKernel.Infrastructure.Interceptors.AuditStampingInterceptor>(),
                    sp.GetRequiredService<Mirama.SharedKernel.Infrastructure.Interceptors.DomainEventDispatchInterceptor>(),
                    sp.GetRequiredService<Mirama.SharedKernel.Infrastructure.Interceptors.AuditSaveChangesInterceptor>());
        });

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<WorkspaceDbContext>());
        services.AddScoped<IModuleMigrator, WorkspaceModuleMigrator>();

        // No integration event types exist in Workspace yet, so both processors
        // simply idle - registered anyway so Workspace is ready the moment it
        // defines its first domain event mapper or hosts its first integration
        // event handler. Mirama.Modules.Workspace.Contracts has no types of its
        // own yet, so this points at the main module assembly instead purely to
        // get a handle on an assembly to scan; switch to a Contracts assembly
        // reference once a cross-module event is actually defined there (a
        // same-module-only event can stay in this assembly - see the outbox
        // design doc, Part 2).
        services.AddOutboxProcessor<WorkspaceDbContext>(config, moduleName: "Workspace", typeof(WorkspaceDbContext).Assembly);
        services.AddInboxProcessor<WorkspaceDbContext>(config, moduleName: "Workspace");

        return services;
    }
}
