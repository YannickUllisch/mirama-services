
using AuthService.Application.Common.Interfaces;
using AuthService.Application.Domain.Authorization;
using AuthService.Application.Domain.Authorization.Interfaces;
using AuthService.Application.Domain.Claims;
using AuthService.Application.Domain.Claims.Contributors;
using AuthService.Application.Domain.Claims.Interfaces;
using AuthService.Application.Domain.Scopes;
using AuthService.Application.Domain.Scopes.Interfaces;
using AuthService.Application.Domain.Scopes.Rules.Expansion;
using AuthService.Application.Domain.Scopes.Rules.Filtering;
using AuthService.Application.Domain.Scopes.Rules.Validation;
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
        // Expansion Rules
        services.AddScoped<IScopeRule, TenantScopeExpansionRule>();
        services.AddScoped<IScopeRule, OrganizationScopeExpansionRule>();
        // Filtering Rules
        services.AddScoped<IScopeRule, ClientCredsScopeFilteringRule>();
        services.AddScoped<IScopeRule, RoleScopeFilteringRule>();
        services.AddScoped<IScopeRule, RefreshTokenScopeFilteringRule>();
        // Validation Rules
        services.AddScoped<IScopeRule, OpenIdScopeRequiredRule>();
        services.AddScoped<IScopeRule, OrganizationPrecedenceRule>();
        services.AddScoped<IScopeRule, TenantOrOrganizationScopeRequiredRule>();


        // Claim Building Pipeline Registration
        services.AddScoped<IClaimsPipeline, ClaimsPipeline>();
        services.AddScoped<IClaimContributor, BasicClaimsContributor>();
        services.AddScoped<IClaimContributor, OrganizationClaimContributor>();
        services.AddScoped<IClaimContributor, TenantClaimContributor>();
        services.AddScoped<IClaimContributor, SubjectClaimContributor>();
        services.AddScoped<IClaimContributor, ResourcesContributor>();

        return services;
    }

    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        return services;
    }
}