using AuthService.Server.Infrastructure.Persistence;
using OpenIddict.Abstractions;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace AuthService.Server.Infrastructure.BackgroundJobs;

public class ClientWorker(IServiceProvider serviceProvider) : IHostedService
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<OpenIdDbContext>();
        await context.Database.EnsureCreatedAsync(cancellationToken);

        // Run Worker on Startup only
        await InitOIDCScopes(scope, cancellationToken);
        await RegisterNextjs(scope, cancellationToken);
        await RegisterPostman(scope, cancellationToken);
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
                Name = "organization",
                DisplayName = "Organization Access",
                Resources = { "api://account", "api://project" }
            }, 
            new OpenIddictScopeDescriptor
            {
                Name = "postman",
                DisplayName = "Postman Access",
                Resources = { "api://account", "api://project" }
            }
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

    private static async ValueTask RegisterNextjs(IServiceScope scopeService, CancellationToken cancellationToken)
    {
        var appManager = scopeService.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();

        var appDescriptor = new OpenIddictApplicationDescriptor
        {
            ClientId = "temp-id",
            ClientSecret = "temp-secret",
            DisplayName = "OpenIdNextjs",
            RedirectUris = { new Uri("http://localhost:3000/api/auth/callback/oidc") },
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
                    Permissions.Prefixes.Scope + "organization"
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

    private static async ValueTask RegisterPostman(IServiceScope scopeService, CancellationToken cancellationToken)
    {
        var appManager = scopeService.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();

        var appDescriptor = new OpenIddictApplicationDescriptor
        {
            ClientId = "open-id-postman",
            ClientSecret = "open-id-postman-secret",
            DisplayName = "OpenIDPostman",
            RedirectUris = { new Uri("https://oauth.pstmn.io/v1/callback") },
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
                    Permissions.GrantTypes.RefreshToken,

                    Permissions.Scopes.Profile,
                    Permissions.Scopes.Email,
                    Permissions.Scopes.Roles,
                    Permissions.Prefixes.Scope + "postmam"
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