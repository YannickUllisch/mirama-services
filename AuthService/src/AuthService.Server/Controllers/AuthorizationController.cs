using System.Security.Claims;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace AuthService.Server.Controllers;

public class AuthorizationController(IOpenIddictApplicationManager applicationManager) : Controller
{
    private readonly IOpenIddictApplicationManager _applicationManager = applicationManager;

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

        // Use the client_id as the subject identifier.
        identity.SetClaim(Claims.Subject, await _applicationManager.GetClientIdAsync(application));
        identity.SetClaim(Claims.Name, await _applicationManager.GetDisplayNameAsync(application));

        identity.SetDestinations(GetDestinations);

        return SignIn(new ClaimsPrincipal(identity), OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }

    private async Task<IActionResult> HandleAuthorizationCodeGrantAsync(OpenIddictRequest request)
    {
        var authenticateResult = await HttpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        var principal = authenticateResult.Principal;

        if (principal == null)
        {
            return Forbid(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }
        
        if (principal.Identity is not ClaimsIdentity identity)
        {
            return Forbid(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        identity.SetDestinations(GetDestinations); 

        return SignIn(new ClaimsPrincipal(identity), OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }

    private async Task<IActionResult> HandleRefreshTokenGrantAsync(OpenIddictRequest request)
    {
        throw new NotImplementedException();
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
            
            case "organization":
                yield return Destinations.AccessToken;

                if (claim.Subject!.HasScope("organization") == true)
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