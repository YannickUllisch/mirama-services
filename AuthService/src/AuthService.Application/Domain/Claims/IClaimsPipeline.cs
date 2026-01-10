
using System.Security.Claims;
using AuthService.Application.Common.Interfaces;

namespace AuthService.Application.Domain.Claims;

public interface IClaimsPipeline
{
    Task<ClaimsPrincipal> BuildAsync(IAuthorizationContext context);
}
