
using AuthService.Application.Domain.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace AuthService.Application.Common.Interfaces;

public interface ITokenService
{
    Task<AuthorizationResult> HandleAsync(HttpContext httpContext, AuthenticateResult result);
}
