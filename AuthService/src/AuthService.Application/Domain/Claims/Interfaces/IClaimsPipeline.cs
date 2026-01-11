
using System.Security.Claims;
using AuthService.Application.Domain.Authorization.Interfaces;

namespace AuthService.Application.Domain.Claims.Interfaces;

public interface IClaimsPipeline
{
    Task<ClaimsPrincipal> BuildAsync(IAuthorizationContext context);
}
