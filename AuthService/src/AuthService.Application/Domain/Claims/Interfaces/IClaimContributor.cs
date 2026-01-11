
using System.Security.Claims;
using AuthService.Application.Domain.Authorization.Interfaces;

namespace AuthService.Application.Domain.Claims.Interfaces;

public interface IClaimContributor
{
    bool IsApplicable(IAuthorizationContext context);
    Task Contribute(IAuthorizationContext context, ClaimsIdentity identity);
}
