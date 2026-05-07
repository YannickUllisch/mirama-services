
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Mirama.Modules.Identity.Infrastructure.Common.Options;
using Mirama.Modules.Identity.Infrastructure.Persistence;
using Mirama.Modules.Identity.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Abstractions.Persistence;
using Mirama.SharedKernel.Models.Decorators;
using Mirama.Modules.Identity.Application.Common.Models;
using Mirama.Modules.Identity.Application.Common.Interfaces;

namespace Mirama.Modules.Identity;

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

        // Wrap Identity handlers only, SharedKernel's AddSharedServices applies all other decorators after.
        // IUnitOfWork resolves to IdentityDbContext within this module's scope.
        services.Decorate(typeof(IRequestHandler<,>), typeof(TransactionDecorator<,>));

        return services;
    }

    private static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        // Options specific to Identity module
        services
            .AddOptions<InfrastructureOptions>()
            .Bind(config.GetSection(InfrastructureOptions.Key))
            .Validate(o => !string.IsNullOrWhiteSpace(o.DatabaseConnection),
                      "Valid Database Connection string is required.")
            .ValidateOnStart();

        // Services
        services.AddScoped(typeof(IIdentityQueryRepository<,>), typeof(IdentityQueryRepository<,>));
        services.AddScoped(typeof(IIdentityCommandRepository<,>), typeof(IdentityCommandRepository<,>));

        services.AddDbContext<IdentityDbContext>(static (sp, options) =>
        {
            var infra = sp.GetRequiredService<IOptions<InfrastructureOptions>>().Value;
            options.UseNpgsql(infra.DatabaseConnection, b => b
                .MigrationsAssembly(typeof(IdentityDbContext).Assembly.FullName)
                .MigrationsHistoryTable(tableName: "__EFMigrationsHistory", schema: "identity"));
        });

        // Expose IdentityDbContext as IUnitOfWork — TransactionDecorator resolves this.
        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<IdentityDbContext>());

        return services;
    }
}