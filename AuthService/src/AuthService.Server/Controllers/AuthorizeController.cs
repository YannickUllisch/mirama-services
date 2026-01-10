using System.Security.Claims;
using AuthService.Application.Common.Interfaces.Services;
using AuthService.Server.Common.Types;
using AuthService.Server.Common.Utils;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace AuthService.Server.Controllers;

public class AuthorizeController(IAuthorizeService authorizeService) : Controller
{
    private readonly IAuthorizeService _authorizeService = authorizeService;

    [HttpGet("~/connect/authorize")]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> AuthorizeNew()
    {
        var auth = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        if (!auth.Succeeded)
        {
            return Challenge(
                authenticationSchemes: CookieAuthenticationDefaults.AuthenticationScheme,
                properties: new AuthenticationProperties
                {
                    RedirectUri = $"{Request.PathBase}{Request.Path}{Request.QueryString}"
                });
        }

        var result = await _authorizeService.HandleAsync(HttpContext, auth);

        if (result.IsDenied)
        {
            return Forbid(
                new AuthenticationProperties
                {
                    Items =
                    {
                        [OpenIddictServerAspNetCoreConstants.Properties.Error] = result.Error,
                        [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = result.ErrorDescription
                    }
                },
                OpenIddictServerAspNetCoreDefaults.AuthenticationScheme
            );
        }

        return SignIn(
            result.Principal!,
            OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }

    // [HttpGet("~/connect/authorize")]
    // [IgnoreAntiforgeryToken]
    // public async Task<IActionResult> Authorize()
    // {
    //     var request = HttpContext.GetOpenIddictServerRequest()
    //         ?? throw new InvalidOperationException("Invalid OpenID Connect request.");

    //     var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

    //     if (!result.Succeeded)
    //     {
    //         return Challenge(
    //             authenticationSchemes: CookieAuthenticationDefaults.AuthenticationScheme,
    //             properties: new AuthenticationProperties
    //             {
    //                 RedirectUri = $"{Request.PathBase}{Request.Path}{Request.QueryString}"
    //             });
    //     }

    //     var identity = new ClaimsIdentity(
    //         TokenValidationParameters.DefaultAuthenticationType,
    //         Claims.Name,
    //         Claims.Role);

    //     // On initial login, we only login to the users tenant, not organization specific yet.
    //     // Hence all organization specific Scopes should be ignored. The account Microservice includes tenant specific
    //     // data hence part of it will be accessible, though all organization owned properties will be locked via policy
    //     var allowedInitialScopes = new[] { 
    //         ScopeType.AccountRead, 
    //         ScopeType.AccountWrite, 
    //         ScopeType.Tenant,
    //         Scopes.OpenId,
    //         Scopes.Profile,
    //         Scopes.Email,
    //         Scopes.OfflineAccess,
    //         Scopes.Roles};

    //     // Settings Base Claims, rest will be set in SignInContext Event handler and validated again in Token endpoint
    //     // Setting Claims at Identity level
    //     // identity.SetClaim(Claims.Subject, result.Principal!.FindFirst(ClaimTypes.NameIdentifier)!.Value);
    //     identity.SetClaim(Claims.Email, result.Principal!.FindFirst(ClaimTypes.Email)!.Value);
    //     // Specifying claim destinations i.e. which go to ID and access token
    //     identity.SetDestinations(ClaimDestinations.GetDestinations);


    //     var principal = new ClaimsPrincipal(identity);

    //     var requestedScopes = request.GetScopes();
    //     var grantedScopes = requestedScopes.Intersect(allowedInitialScopes);

    //     // Set scopes and resources requested by the client at principal level
    //     principal.SetScopes(grantedScopes);
    //     principal.SetResources(grantedScopes);

    //     return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    // }
}