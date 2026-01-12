using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using AuthService.Server.Models;

namespace AuthService.Server.Controllers;

public class AuthenticationController : Controller
{
    [HttpGet("~/auth/login")]
    public IActionResult Login(string returnUrl)
    {
        var model = new LoginViewModel { ReturnUrl = returnUrl ?? "/" };
        return View(model);
    }

    [HttpPost("~/auth/GoogleLogin")]
    public IActionResult GoogleLogin(string returnUrl)
    {
        // Use the original returnUrl (OIDC authorize endpoint)
        return Challenge(new AuthenticationProperties { RedirectUri = returnUrl ?? "/" }, "Google");
    }
}
