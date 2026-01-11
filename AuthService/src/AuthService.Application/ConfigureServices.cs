using AuthService.Application.Common;
using AuthService.Application.Common.Interfaces;
using AuthService.Application.Common.Interfaces.Services;
using AuthService.Application.Domain.Claims;
using AuthService.Application.Domain.Claims.Contributors;
using AuthService.Application.Domain.Scopes;
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
        services.AddScoped<IAuthorizationRule, TenantScopeExpansionRule>();
        services.AddScoped<IAuthorizationRule, OrganizationScopeExpansionRule>();
        // Filtering Rules
        services.AddScoped<IAuthorizationRule, ClientCredsScopeFilteringRule>();
        services.AddScoped<IAuthorizationRule, RoleScopeFilteringRule>();
        services.AddScoped<IAuthorizationRule, RefreshTokenScopeFilteringRule>();
        // Validation Rules
        services.AddScoped<IAuthorizationRule, OpenIdScopeRequiredRule>();
        services.AddScoped<IAuthorizationRule, OrganizationPrecedenceRule>();
        services.AddScoped<IAuthorizationRule, TenantOrOrganizationScopeRequiredRule>();


        // Claim Building Pipeline Registration
        services.AddScoped<IClaimsPipeline, ClaimsPipeline>();
        services.AddScoped<IClaimContributor, BasicClaimsContributor>();
        services.AddScoped<IClaimContributor, OrganizationClaimContributor>();
        services.AddScoped<IClaimContributor, TenantClaimContributor>();
        services.AddScoped<IClaimContributor, SubjectClaimContributor>();
        services.AddScoped<IClaimContributor, ResourcesContributor>();

        return services;
    }
}