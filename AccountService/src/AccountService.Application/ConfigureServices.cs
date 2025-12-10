
using AccountService.Application.Common;
using AccountService.Application.Common.Behaviours;
using AccountService.Application.Common.Interfaces;
using AccountService.Application.Infrastructure.Persistence;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AccountService.Application;
 
 public static class DependencyInjection
 {
     public static IServiceCollection AddApplication(this IServiceCollection services)
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

     public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(config.GetConnectionString("Database"),
            b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
            
        return services;
    }
 }