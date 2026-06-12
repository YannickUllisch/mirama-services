
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Mirama.Modules.Identity.Infrastructure.Persistence;
using Mirama.Modules.Identity.Infrastructure.Persistence.Repositories;
using Mirama.Modules.Identity.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Abstractions.Permissions;
using Mirama.SharedKernel.Abstractions.Persistence;
using Mirama.Modules.Identity.Application.Common.Models;
using Mirama.Modules.Identity.Application.Common.Interfaces;
using Mirama.SharedKernel.Infrastructure.Options;
using Mirama.Modules.Identity.Application.Common;

namespace Mirama.Modules.Identity.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddIdentityModule(this IServiceCollection services, IConfiguration config)
    {
        services.AddApplication(config);
        services.AddInfrastructure(config);

        return services;
    }

    private static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration config)
    {
        services.AddSingleton<IGlobalRoleProvider, GlobalRoleProvider>();

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

        // Use module-specific decorator, avoids IUnitOfWork being overridden by other modules.
        services.Decorate(typeof(IRequestHandler<,>), typeof(IdentityTransactionDecorator<,>));

        return services;
    }

    private static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        // Services
        services.AddScoped<IPermissionService, PermissionService>();
        services.AddScoped(typeof(IIdentityQueryRepository<,>), typeof(IdentityQueryRepository<,>));
        services.AddScoped(typeof(IIdentityCommandRepository<,>), typeof(IdentityCommandRepository<,>));

        services.AddDbContext<IdentityDbContext>(static (sp, options) =>
        {
            var infra = sp.GetRequiredService<IOptions<InfrastructureOptions>>().Value;
            options
                .UseNpgsql(infra.DatabaseConnection, b => b
                    .MigrationsAssembly(typeof(IdentityDbContext).Assembly.FullName)
                    .MigrationsHistoryTable(tableName: "__EFMigrationsHistory", schema: "identity"))
                .AddInterceptors(sp.GetRequiredService<Mirama.SharedKernel.Infrastructure.Interceptors.AuditSaveChangesInterceptor>());
        });

        return services;
    }
}