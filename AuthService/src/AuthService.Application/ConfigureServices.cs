using AuthService.Application.Common;
using AuthService.Application.Common.Interfaces;
using AuthService.Application.Common.Interfaces.Services;
using AuthService.Application.Domain.Claims;
using AuthService.Application.Domain.Claims.Contributors;
using AuthService.Application.Domain.Scopes;
using AuthService.Application.Domain.Scopes.Rules;
using AuthService.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace AuthService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Services
        services.AddScoped<IAuthorizeService, AuthorizeService>();
        services.AddScoped<ITokenService, TokenService>();

        // Factories
        services.AddScoped<IAuthorizationContextFactory, AuthorizationContextFactory>();

        // Scope Pipeline Registration
        services.AddScoped<IScopePolicyPipeline, ScopePolicyPipeline>();
        services.AddScoped<IAuthorizationRule, ClientCredentialsScopeRule>();
        services.AddScoped<IAuthorizationRule, OrganizationPrecedenceScopeRule>();
        services.AddScoped<IAuthorizationRule, RefreshTokenScopeRule>();
        services.AddScoped<IAuthorizationRule, RoleScopeRule>();
        services.AddScoped<IAuthorizationRule, TenantOrOrganizationScopeRequiredRule>();

        // Claim Building Pipeline Registration
        services.AddScoped<IClaimsPipeline, ClaimsPipeline>();
        services.AddScoped<IClaimContributor, BasicClaimsContributor>();
        services.AddScoped<IClaimContributor, OrganizationClaimContributor>();
        services.AddScoped<IClaimContributor, TenantClaimContributor>();
        services.AddScoped<IClaimContributor, SubjectClaimContributor>();

        return services;
    }
}