
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace AuthService.Application.Common.Interfaces.Services;

public interface IAuthorizeService
{
    Task<AuthorizationResult> HandleAsync(HttpContext httpContext, AuthenticateResult result);
}
