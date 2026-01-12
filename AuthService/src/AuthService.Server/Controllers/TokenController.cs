
using AuthService.Application.Common.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Server.AspNetCore;

namespace AuthService.Server.Controllers;

public class TokenController(ITokenService tokenService) : Controller
{
    private readonly ITokenService _tokenService = tokenService;

    [HttpPost("~/connect/token"), Produces("application/json")]
    public async Task<IActionResult> GetToken()
    {
        var auth = await HttpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

        var result = await _tokenService.HandleAsync(HttpContext, auth);

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

        return SignIn(result.Principal!, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }
}