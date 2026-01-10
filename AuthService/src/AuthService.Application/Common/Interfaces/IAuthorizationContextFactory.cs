
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace AuthService.Application.Common.Interfaces;

public interface IAuthorizationContextFactory
{
    Task<IAuthorizationContext> CreateForAuthorizeAsync(HttpContext httpContext, AuthenticateResult result);
    Task<IAuthorizationContext> CreateForTokenAsync(HttpContext httpContext, AuthenticateResult result);
}
