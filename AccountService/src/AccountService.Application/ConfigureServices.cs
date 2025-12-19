
using AccountService.Application.Common;
using AccountService.Application.Common.Behaviours;
using AccountService.Application.Common.Interfaces;
using AccountService.Application.Common.Options;
using AccountService.Application.Infrastructure.Common.Interfaces;
using AccountService.Application.Infrastructure.Persistence;
using AccountService.Application.Infrastructure.Services;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace AccountService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration config)
    {
        var applicationAssembly = typeof(DependencyInjection).Assembly;

        services.AddMediatR(options =>
        {
            options.RegisterServicesFromAssembly(applicationAssembly);
            options.AddOpenBehavior(typeof(ValidationBehaviour<,>));
        });

        services.AddValidatorsFromAssembly(applicationAssembly, includeInternalTypes: true);

        services.AddSingleton<IGlobalRoleProvider, GlobalRoleProvider>();

        services.Configure<ApplicationOptions>(config.GetSection(ApplicationOptions.Application));
        return services;
    }

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.AddScoped<IUserProvider, UserProviderService>();
        services.AddScoped<IOrganizationProvider, OrganizationProviderService>();

        services
            .AddOptions<InfrastructureOptions>()
            .Bind(config.GetSection(InfrastructureOptions.Infrastructure))
            .Validate(o => !string.IsNullOrWhiteSpace(o.DatabaseConnection),
                      "Valid Database Connection string is required.")
            .ValidateOnStart();

        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            var infra = sp.GetRequiredService<IOptions<InfrastructureOptions>>().Value;
            options.UseNpgsql(infra.DatabaseConnection, b => b
                .MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)
                .MigrationsHistoryTable(tableName: "__EFMigrationsHistory", schema: "auth"));
        });

        return services;
    }
}