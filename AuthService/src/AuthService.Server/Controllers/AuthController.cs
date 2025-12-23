using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using AuthService.Server.Infrastructure.Persistence;

namespace AuthService.Server.Controllers
{
    public class AuthController(OpenIdDbContext context) : Controller
    {
        private readonly OpenIdDbContext _context = context;

        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            TempData["returnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public IActionResult GoogleLogin(string returnUrl)
        {
            // Use the original returnUrl (OIDC authorize endpoint)
            return Challenge(new AuthenticationProperties { RedirectUri = returnUrl ?? "/" }, "Google");
        }  
    }
}