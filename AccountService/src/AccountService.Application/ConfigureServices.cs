
using AccountService.Application.Common;
using AccountService.Application.Common.Behaviours;
using AccountService.Application.Common.Interfaces;
using AccountService.Application.Common.Options;
using AccountService.Application.Domain.Aggregates.Organization;
using AccountService.Application.Domain.Aggregates.Tenant;
using AccountService.Application.Domain.Aggregates.User;
using AccountService.Application.Features.Organizations;
using AccountService.Application.Features.Users;
using AccountService.Application.Infrastructure.Persistence;
using AccountService.Application.Infrastructure.Persistence.Repositories;
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
        services.AddScoped<ICommandRepository<User>, UserCommandRepository>();
        services.AddScoped<ICommandRepository<Organization>, OrganizationCommandRepository>();
        services.AddScoped<ICommandRepository<Tenant>, TenantCommandRepository>();

        return services;
    }

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
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
        services.AddScoped<IRequestContextProvider, RequestContextProvider>();
        services.AddScoped(typeof(IReadRepository<,>), typeof(ReadRepository<,>));

        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            var infra = sp.GetRequiredService<IOptions<InfrastructureOptions>>().Value;
            options.UseNpgsql(infra.DatabaseConnection, b => b
                .MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)
                .MigrationsHistoryTable(tableName: "__EFMigrationsHistory", schema: "account"));
        });

        return services;
    }
}