
using Mirama.Application.Common;
using Mirama.Application.Common.Behaviours;
using Mirama.Application.Common.Interfaces;
using Mirama.Application.Common.Options;
using Mirama.Application.Domain.Aggregates.Organization;
using Mirama.Application.Domain.Aggregates.Tenant;
using Mirama.Application.Domain.Aggregates.User;
using Mirama.Application.Features.Organizations;
using Mirama.Application.Features.Users;
using Mirama.Application.Infrastructure.Persistence;
using Mirama.Application.Infrastructure.Persistence.Repositories;
using Mirama.Application.Infrastructure.Services;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Mirama.Application;

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