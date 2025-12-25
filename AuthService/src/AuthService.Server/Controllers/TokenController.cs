using System.Security.Claims;
using AuthService.Server.Common.Enums;
using AuthService.Server.Common.Extensions;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace AuthService.Server.Controllers;

public class TokenController(IOpenIddictApplicationManager applicationManager, IOpenIddictScopeManager scopeManager) : Controller
{
    private readonly IOpenIddictApplicationManager _applicationManager = applicationManager;
    private readonly IOpenIddictScopeManager _scopeManager = scopeManager;

    [HttpPost("~/connect/token"), Produces("application/json")]
    public async Task<IActionResult> Exchange()
    {
        var request = HttpContext.GetOpenIddictServerRequest();
        if (request is null)
        {
            return BadRequest("Invalid OpenIddict request.");
        }

        return request.GrantType switch
        {
            GrantTypes.ClientCredentials => await HandleClientCredentialsGrantAsync(request),
            GrantTypes.AuthorizationCode => await HandleAuthorizationCodeGrantAsync(request),
            GrantTypes.RefreshToken => await HandleRefreshTokenGrantAsync(request),
            GrantTypes.TokenExchange => await HandleTokenExchangeGrantAsync(request),
            _ => BadRequest("Unsupported grant type."),
        };
    }

    private async Task<IActionResult> HandleClientCredentialsGrantAsync(OpenIddictRequest request)
    {
        // Note: the client credentials are automatically validated by OpenIddict:
        // if client_id or client_secret are invalid, this action won't be invoked.
        var application = await _applicationManager.FindByClientIdAsync(request.ClientId!) ??
            throw new InvalidOperationException("The application cannot be found.");

        // Create a new ClaimsIdentity containing the claims that
        // will be used to create an id_token, a token or a code.
        var identity = new ClaimsIdentity(TokenValidationParameters.DefaultAuthenticationType, Claims.Name, Claims.Role);
        var scopes = request.GetScopes();
        
        // Use the client_id as the subject identifier.
        identity.SetClaim(Claims.Subject, await _applicationManager.GetClientIdAsync(application));

        // Organization has precedent over tenant, if organization scope is requested we ignore tenant scope
        if (scopes.Contains(ScopeExtensionType.Organization.AsString()))
        {
            // Fetch requested orgId from body (since token request is POST)
            var requestedOrgId = request.GetParameter("organization_id")?.ToString();

            // Hardcoded permission check for demo
            if (string.IsNullOrEmpty(requestedOrgId) || requestedOrgId != "orgIdExample")
            {
                return BadRequest(new { error = "invalid_request", error_description = "Invalid organization requested." });
            }
        
            identity.SetClaim("tid", "tenantIdExample");
            identity.SetClaim("oid", requestedOrgId);
        }
        else if (scopes.Contains(ScopeExtensionType.Tenant.AsString())) 
        {
            identity.SetClaim("tid", "tenantOnlyScopeId");
        }

        identity.SetDestinations(GetDestinations);
        

        return SignIn(new ClaimsPrincipal(identity), OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }

    private async Task<IActionResult> HandleAuthorizationCodeGrantAsync(OpenIddictRequest request)
    {
        var result = await HttpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        if (result?.Principal?.Identity is not ClaimsIdentity identity)
            return Forbid(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

        var scopes = result.Principal.GetScopes();

        // Example: Add tenant/org claims based on scopes
        if (scopes.Contains(ScopeExtensionType.Organization.AsString()))
        {
            identity.SetClaim("tid", "tenantIdExample");
            identity.SetClaim("oid", "orgIdExample");
        }
        else if (scopes.Contains(ScopeExtensionType.Tenant.AsString()))
        {
            identity.SetClaim("tid", "tenantOnlyScopeId");
        }

        // Set audiences based on resources of requested scopes
        var resources = await _scopeManager.ListResourcesAsync(scopes).ToListAsync();
        identity.SetResources(resources);

        identity.SetDestinations(GetDestinations);

        return SignIn(new ClaimsPrincipal(identity), OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }
    
    private async Task<IActionResult> HandleRefreshTokenGrantAsync(OpenIddictRequest request)
    {
        // Authenticate the incoming refresh token
        var authenticateResult = await HttpContext.AuthenticateAsync(
            OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

        var principal = authenticateResult.Principal;
        if (principal is null)
            return Forbid(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

        var identity = (ClaimsIdentity)principal.Identity!;
        var scopes = principal.GetScopes(); // scopes associated with this token

        // Hardcoded IDs, change later with results from Account Service
        var tenantId = "tenantIdExample";
        var orgId = "orgIdExample";

        if (scopes.Contains(ScopeExtensionType.Organization.AsString()))
        {
            // Organization scope is present, we revalidate permissions for the org
            var isOrgAllowed = true;
            if (!isOrgAllowed)
            {
                return Forbid(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            }

            // Add both organization and assumed tenant values
            identity.SetClaim("oid", orgId);
            identity.SetClaim("tid", tenantId);
        }
        else if (scopes.Contains(ScopeExtensionType.Tenant.AsString()))
        {
            // Only tenant scope -> return tenant claim, if only tenant is present
            // user can only ever get access to its own tenantId
            // We should still refetch it just for extra safety
            identity.SetClaim("tid", tenantId);
        }

        return SignIn(new ClaimsPrincipal(identity),
                    OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }


    private async Task<IActionResult> HandleTokenExchangeGrantAsync(OpenIddictRequest request)
    {
        throw new NotImplementedException();
    }

    private static IEnumerable<string> GetDestinations(Claim claim)
    {
        // Note: by default, claims are NOT automatically included in the access and identity tokens.
        // To allow OpenIddict to serialize them, you must attach them a destination, that specifies
        // whether they should be included in access tokens, in identity tokens or in both.

        switch (claim.Type)
        {
            case Claims.Name or Claims.PreferredUsername:
                yield return Destinations.AccessToken;

                if (claim.Subject!.HasScope(Scopes.Profile))
                    yield return Destinations.IdentityToken;

                yield break;

            case Claims.Email:
                yield return Destinations.AccessToken;

                if (claim.Subject!.HasScope(Scopes.Email))
                    yield return Destinations.IdentityToken;

                yield break;

            case Claims.Role:
                yield return Destinations.AccessToken;

                if (claim.Subject!.HasScope(Scopes.Roles))
                    yield return Destinations.IdentityToken;

                yield break;
            
            case Claims.Profile:
                yield return Destinations.AccessToken;

                if (claim.Subject!.HasScope(Scopes.Profile))
                    yield return Destinations.IdentityToken;

                yield break;
            
            case "oid":
                yield return Destinations.AccessToken;

                if (claim.Subject!.HasScope("oid") == true)
                    yield return Destinations.IdentityToken;

                yield break;
            
            case "tid":
                yield return Destinations.AccessToken;

                if (claim.Subject!.HasScope("tid") == true)
                    yield return Destinations.IdentityToken;

                yield break;

            // Never include the security stamp in the access and identity tokens, as it's a secret value.
            case "AspNet.Identity.SecurityStamp": yield break;

            default:
                yield return Destinations.AccessToken;
                yield break;
        }
    }
}