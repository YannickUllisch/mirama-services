
using AuthService.Server.Common.Types;
using AuthService.Server.Common.Options;
using AuthService.Server.Infrastructure.Persistence;
using Microsoft.Extensions.Options;
using OpenIddict.Abstractions;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace AuthService.Server.Infrastructure.BackgroundJobs;

public class ClientWorker(IServiceProvider serviceProvider, IOptions<OAuthClientOptions> clientOptions) : IHostedService
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;
    private readonly IOptions<OAuthClientOptions> _clientOptions = clientOptions;


    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<OpenIdDbContext>();
        await context.Database.EnsureCreatedAsync(cancellationToken);

        // Run Worker on Startup only
        await InitOIDCScopes(scope, cancellationToken);
        await RegisterMiramaFrontend(scope, _clientOptions.Value, cancellationToken);
        await RegisterPostman(scope, _clientOptions.Value, cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private static async ValueTask InitOIDCScopes(IServiceScope scope, CancellationToken cancellationToken)
    {
        var scopeManager = scope.ServiceProvider.GetRequiredService<IOpenIddictScopeManager>();

        var scopes = new[]
        {
            new OpenIddictScopeDescriptor
            {
                Name = ScopeType.Organization,
                DisplayName = "Organization Access",
                Resources = { ResourceType.Account, ResourceType.Project }
            }, 
            new OpenIddictScopeDescriptor
            {
                Name = ScopeType.Tenant,
                DisplayName = "Tenant Access",
                Resources = { ResourceType.Account }
            },
            new OpenIddictScopeDescriptor
            {
                Name = ScopeType.AccountRead,
                DisplayName = "Read access to Account API",
                Resources = { ResourceType.Account }
            },
            new OpenIddictScopeDescriptor
            {
                Name = ScopeType.AccountWrite,
                DisplayName = "Write access to Account API",
                Resources = { ResourceType.Account }
            },
            new OpenIddictScopeDescriptor
            {
                Name = ScopeType.ProjectRead,
                DisplayName = "Read access to Project API",
                Resources = { ResourceType.Project }
            },
            new OpenIddictScopeDescriptor
            {
                Name = ScopeType.ProjectWrite,
                DisplayName = "Write access to Project API",
                Resources = { ResourceType.Project }
            },
            new OpenIddictScopeDescriptor
            {
                Name = ScopeType.LLMRead,
                DisplayName = "Read access to LLM API",
                Resources = { ResourceType.LLM }
            },
            new OpenIddictScopeDescriptor
            {
                Name = ScopeType.LLMWrite,
                DisplayName = "Write access to LLM API",
                Resources = { ResourceType.LLM }
            },
        };

        foreach (var scopeDescriptor in scopes)
        {
            if (string.IsNullOrEmpty(scopeDescriptor.Name))
            {
                throw new ArgumentException("Scope name cannot be null or empty.", nameof(scopeDescriptor.Name));
            }

            var scopeInstance = await scopeManager.FindByNameAsync(scopeDescriptor.Name, cancellationToken);

            if (scopeInstance == null)
            {
                await scopeManager.CreateAsync(scopeDescriptor, cancellationToken);
            }
            else
            {
                await scopeManager.UpdateAsync(scopeInstance, scopeDescriptor, cancellationToken);
            }
        }
    }

    private static async ValueTask RegisterMiramaFrontend(IServiceScope scopeService, OAuthClientOptions clientOptions, CancellationToken cancellationToken)
    {
        var appManager = scopeService.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();

        var appDescriptor = new OpenIddictApplicationDescriptor
        {
            ClientId = clientOptions.MiramaFrontendClientId,
            ClientSecret = clientOptions.MiramaFrontendClientSecret,
            DisplayName = "OpenIdMiramaFrontend",
            RedirectUris = { new Uri(clientOptions.MiramaFrontendRedirectUri) },
            ClientType = ClientTypes.Confidential,
            ConsentType = ConsentTypes.Explicit,
            Permissions =
                {
                    Permissions.Endpoints.Token,
                    Permissions.Endpoints.Authorization,
                    Permissions.Endpoints.EndSession,

                    Permissions.ResponseTypes.Code,

                    Permissions.GrantTypes.TokenExchange,
                    Permissions.GrantTypes.AuthorizationCode,
                    Permissions.GrantTypes.RefreshToken,
                    
                    Permissions.Scopes.Profile,
                    Permissions.Scopes.Email,
                    Permissions.Scopes.Roles,
                    Permissions.Prefixes.Scope + ScopeType.Organization,
                    Permissions.Prefixes.Scope + ScopeType.Tenant,
                    Permissions.Prefixes.Scope + ScopeType.AccountRead,
                    Permissions.Prefixes.Scope + ScopeType.AccountWrite,
                    Permissions.Prefixes.Scope + ScopeType.ProjectRead,
                    Permissions.Prefixes.Scope + ScopeType.ProjectWrite,
                    Permissions.Prefixes.Scope + ScopeType.LLMRead,
                    Permissions.Prefixes.Scope + ScopeType.LLMWrite,
                }
        };

        var client = await appManager.FindByClientIdAsync(appDescriptor.ClientId, cancellationToken);

        if (client == null)
        {
            await appManager.CreateAsync(appDescriptor, cancellationToken);
        }
        else
        {
            await appManager.UpdateAsync(client, appDescriptor, cancellationToken);
        }
    }

    private static async ValueTask RegisterPostman(IServiceScope scopeService, OAuthClientOptions clientOptions, CancellationToken cancellationToken)
    {
        var appManager = scopeService.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();

        var appDescriptor = new OpenIddictApplicationDescriptor
        {
            ClientId = clientOptions.PostmanClientId,
            ClientSecret = clientOptions.PostmanClientSecret,
            DisplayName = "OpenIDPostman",
            RedirectUris = { new Uri(clientOptions.PostmanRedirectUri) },
            ClientType = ClientTypes.Confidential,
            ConsentType = ConsentTypes.Explicit,
            Permissions =
                {
                    Permissions.Endpoints.Token,
                    Permissions.Endpoints.Authorization,
                    Permissions.Endpoints.Introspection,
                    Permissions.Endpoints.Revocation,

                    Permissions.ResponseTypes.Code,

                    Permissions.GrantTypes.ClientCredentials,
                    Permissions.GrantTypes.AuthorizationCode,
                    Permissions.GrantTypes.RefreshToken,

                    Permissions.Scopes.Profile,
                    Permissions.Scopes.Email,
                    Permissions.Scopes.Roles,
                    Permissions.Prefixes.Scope + ScopeType.Organization,
                    Permissions.Prefixes.Scope + ScopeType.Tenant,
                    Permissions.Prefixes.Scope + ScopeType.AccountRead,
                    Permissions.Prefixes.Scope + ScopeType.AccountWrite,
                    Permissions.Prefixes.Scope + ScopeType.ProjectRead,
                    Permissions.Prefixes.Scope + ScopeType.ProjectWrite,
                    Permissions.Prefixes.Scope + ScopeType.LLMRead,
                    Permissions.Prefixes.Scope + ScopeType.LLMWrite,
                }
        };

        var client = await appManager.FindByClientIdAsync(appDescriptor.ClientId, cancellationToken);

        if (client == null)
        {
            await appManager.CreateAsync(appDescriptor, cancellationToken);
        }
        else
        {
            await appManager.UpdateAsync(client, appDescriptor, cancellationToken);
        }
    }
}