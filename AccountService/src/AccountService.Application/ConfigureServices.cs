
using AccountService.Application.Common.Behaviours;
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
         services.AddMediatR(options =>
         {
            options.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
            options.AddOpenBehavior(typeof(ValidationBehaviour<,>));
         });
         services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly, includeInternalTypes: true);
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