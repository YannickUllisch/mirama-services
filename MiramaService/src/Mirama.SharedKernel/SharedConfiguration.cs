
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mirama.SharedKernel.Abstractions.Persistence;
using Mirama.SharedKernel.Infrastructure.Options;
using Mirama.SharedKernel.Models;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using System.Reflection;
using Mirama.SharedKernel.Models.Decorators;

namespace Mirama.SharedKernel;

public static class DependencyInjection
{
    public static IServiceCollection AddSharedServices(this IServiceCollection services, IConfiguration config)
    {
        var applicationAssembly = typeof(DependencyInjection).Assembly;

        services.AddValidatorsFromAssembly(applicationAssembly, includeInternalTypes: true);

        services.Configure<ApplicationOptions>(config.GetSection(ApplicationOptions.Key));
        services.Configure<AuthenticationOptions>(config.GetSection(AuthenticationOptions.Key));

        services.AddScoped<IRequestContextProvider, IRequestContextProvider>();

        // Dispatcher setup
        services.AddScoped<IDispatcher, Dispatcher>();

        services.Scan(scan => scan
            .FromAssemblies(Assembly.GetExecutingAssembly())
            .AddClasses(classes => classes.AssignableTo(typeof(IRequestHandler<,>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        services.Scan(scan => scan
            .FromAssemblies(Assembly.GetExecutingAssembly())
            .AddClasses(classes => classes.AssignableTo(typeof(INotificationHandler<>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        services.Decorate(typeof(IRequestHandler<,>), typeof(ValidationDecorator<,>));
        services.Decorate(typeof(IRequestHandler<,>), typeof(PerformanceDecorator<,>));
        services.Decorate(typeof(IRequestHandler<,>), typeof(LoggingDecorator<,>));

        return services;
    }
}