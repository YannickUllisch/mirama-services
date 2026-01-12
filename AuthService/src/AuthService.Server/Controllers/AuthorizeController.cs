
using AuthService.Application.Common.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Server.AspNetCore;

namespace AuthService.Server.Controllers;

public class AuthorizeController(IAuthorizeService authorizeService) : Controller
{
    private readonly IAuthorizeService _authorizeService = authorizeService;

    [HttpGet("~/connect/authorize")]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> Authorize()
    {
        // Check local Authentication status
        // If User is not authenticated, send an Auth challenge
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

}