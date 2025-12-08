
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace AccountService.Application;
 
 public static class DependencyInjection
 {
     public static IServiceCollection AddApplication(this IServiceCollection services)
     {
         services.AddMediatR(options =>
         {
             options.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
            //  options.AddOpenBehavior(typeof(ValidationBehaviour<,>));
         });
         services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly, includeInternalTypes: true);
         return services;
     }
 }