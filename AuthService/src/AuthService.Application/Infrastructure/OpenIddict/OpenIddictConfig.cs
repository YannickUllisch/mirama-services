

using AuthService.Application.Common.Options;
using AuthService.Application.Domain.Scopes;
using AuthService.Application.Infrastructure.Common;
using AuthService.Application.Infrastructure.OpenIddict.EventHandlers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using static OpenIddict.Server.OpenIddictServerEvents;

namespace AuthService.Application.Infrastructure.OpenIddict;

public static class OpenIddictConfiguration
{
    public static OpenIddictServerBuilder ConfigureServer(
        this OpenIddictServerBuilder options, IConfiguration config)
    {
        var secrets = config.GetSection(ApplicationOptions.Application).Get<ApplicationOptions>()
            ?? throw new InvalidOperationException("Application option configuration is missing or invalid.");

        // For now we just use JWTs instead of JWEs, maybe in the future we add JWE support
        options.DisableAccessTokenEncryption();

        // Force PKCE for Authorization Code flow even for Explicit Clients
        options.RequireProofKeyForCodeExchange();

        options.SetAccessTokenLifetime(TimeSpan.FromMinutes(15));
        options.SetRefreshTokenLifetime(TimeSpan.FromDays(30));

        options.RegisterScopes(
            ScopeType.OpenId,
            ScopeType.Roles,
            ScopeType.OfflineAccess,
            ScopeType.Profile,
            ScopeType.Email,
            ScopeType.Tenant,
            ScopeType.Organization,
            ScopeType.AccountWrite,
            ScopeType.AccountRead,
            ScopeType.ProjectWrite,
            ScopeType.ProjectRead,
            ScopeType.LLMWrite,
            ScopeType.LLMRead);

        options.RegisterAudiences(
                ResourceType.Account,
                ResourceType.Project,
                ResourceType.LLM);

        // Issuer refers to this Auth Server, hardcoded for testing purposes
        options.SetIssuer(new Uri(secrets.SelfUrl));

        // Registering openiddict event handlers
        options.AddEventHandler<ProcessSignInContext>(builder =>
        {
            builder.UseScopedHandler<AccountUserProvisioningHandler>();
        });

        return options;
    }
}
