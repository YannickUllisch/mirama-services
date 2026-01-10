
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Server.Controllers;

public class LogoutController : Controller
{
    [HttpPost("~/connect/logout")]
    public async Task<IActionResult> Logout(string redirect_uri)
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        return Redirect(string.IsNullOrEmpty(redirect_uri) ? "/" : redirect_uri);
    }
}