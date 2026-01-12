
using System.Security.Claims;
using OpenIddict.Abstractions;
using Microsoft.IdentityModel.Tokens;
using AuthService.Application.Domain.Claims.Interfaces;
using AuthService.Application.Domain.Authorization.Interfaces;

namespace AuthService.Application.Domain.Claims;

public sealed class ClaimsPipeline(IEnumerable<IClaimContributor> contributors) : IClaimsPipeline
{
    private readonly IEnumerable<IClaimContributor> _contributors = contributors;

    public async Task<ClaimsPrincipal> BuildAsync(IAuthorizationContext context)
    {
        var identity = new ClaimsIdentity(
            TokenValidationParameters.DefaultAuthenticationType,
            ClaimType.Name,
            ClaimType.Role);

        identity.SetScopes(context.GrantedScopes);

        foreach (var contributor in _contributors)
        {
            if (contributor.IsApplicable(context))
                await contributor.Contribute(context, identity);
        }

        identity.SetDestinations(ClaimDestinations.GetDestinations);

        var principal = new ClaimsPrincipal(identity);

        principal.SetScopes(context.GrantedScopes);

        return principal;
    }
}
