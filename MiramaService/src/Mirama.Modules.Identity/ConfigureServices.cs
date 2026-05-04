
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Mirama.Modules.Identity.Infrastructure.Common.Options;
using Mirama.Modules.Identity.Infrastructure.Persistence;
using Mirama.Modules.Identity.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Mirama.SharedKernel.Infrastructure.Behaviours;
using Mirama.SharedKernel.Abstractions.Persistence;
using Mirama.Modules.Identity.Application.Common.Models;
using Mirama.SharedKernel.Infrastructure.Options;

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
        var applicationAssembly = typeof(DependencyInjection).Assembly;

        services.AddMediatR(options =>
        {
            options.RegisterServicesFromAssembly(applicationAssembly);
            options.AddOpenBehavior(typeof(ValidationBehaviour<,>));
        });

        services.AddValidatorsFromAssembly(applicationAssembly, includeInternalTypes: true);

        services.AddSingleton<IGlobalRoleProvider, GlobalRoleProvider>();

        return services;
    }

    private static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        // Configuring Options Pattern
        services.Configure<ApplicationOptions>(config.GetSection(ApplicationOptions.Key));
        services.Configure<AuthenticationOptions>(config.GetSection(AuthenticationOptions.Key));
        services
            .AddOptions<InfrastructureOptions>()
            .Bind(config.GetSection(InfrastructureOptions.Key))
            .Validate(o => !string.IsNullOrWhiteSpace(o.DatabaseConnection),
                      "Valid Database Connection string is required.")
            .ValidateOnStart();


        // Services
        services.AddScoped<IRequestContextProvider, IRequestContextProvider>();
        services.AddScoped(typeof(IReadRepository<,>), typeof(ReadRepository<,>));
        services.AddScoped(typeof(ICommandRepository<,>), typeof(CommandRepository<,>));

        services.AddDbContext<ApplicationDbContext>(static (sp, options) =>
        {
            var infra = sp.GetRequiredService<IOptions<InfrastructureOptions>>().Value;
            options.UseNpgsql(infra.DatabaseConnection, b => b
                .MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)
                .MigrationsHistoryTable(tableName: "__EFMigrationsHistory", schema: "account"));
        });

        return services;
    }
}