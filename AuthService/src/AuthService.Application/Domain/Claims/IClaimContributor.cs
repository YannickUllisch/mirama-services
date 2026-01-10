
using System.Security.Claims;
using AuthService.Application.Common.Interfaces;

namespace AuthService.Application.Domain.Claims;

public interface IClaimContributor
{
    bool IsApplicable(IAuthorizationContext context);
    Task Contribute(IAuthorizationContext context, ClaimsIdentity identity);
}
