
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mirama.SharedKernel.Infrastructure.Behaviours;
using Mirama.SharedKernel.Abstractions.Persistence;
using Mirama.SharedKernel.Infrastructure.Options;

namespace Mirama.SharedKernel;

public static class DependencyInjection
{
    public static IServiceCollection AddSharedServices(this IServiceCollection services, IConfiguration config)
    {
        var applicationAssembly = typeof(DependencyInjection).Assembly;

        services.AddMediatR(options =>
        {
            options.RegisterServicesFromAssembly(applicationAssembly);
            options.AddOpenBehavior(typeof(ValidationBehaviour<,>));
        });

        services.AddValidatorsFromAssembly(applicationAssembly, includeInternalTypes: true);

        services.Configure<ApplicationOptions>(config.GetSection(ApplicationOptions.Key));
        services.Configure<AuthenticationOptions>(config.GetSection(AuthenticationOptions.Key));

        services.AddScoped<IRequestContextProvider, IRequestContextProvider>();

        return services;
    }
}