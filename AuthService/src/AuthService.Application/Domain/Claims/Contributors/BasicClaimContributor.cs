
using System.Security.Claims;
using AuthService.Application.Domain.Authorization.Interfaces;
using AuthService.Application.Domain.Claims.Interfaces;
using AuthService.Application.Domain.Scopes;

namespace AuthService.Application.Domain.Claims.Contributors;

public class BasicClaimsContributor : IClaimContributor
{
    public bool IsApplicable(IAuthorizationContext context) => true;

    public Task Contribute(IAuthorizationContext context, ClaimsIdentity identity)
    {
        // Add claims based on granted scopes
        if (context.GrantedScopes.Contains(ScopeType.Profile))
        {
            identity.AddClaim(new Claim(ClaimType.Picture, context.AuthenticatedUser!.Image ?? ""));
            identity.AddClaim(new Claim(ClaimType.Name, context.AuthenticatedUser!.Name ?? ""));
        }
        if (context.GrantedScopes.Contains(ScopeType.Email))
        {
            identity.AddClaim(new Claim(ClaimType.Email, context.AuthenticatedUser!.Email));
        }
        if (context.GrantedScopes.Contains(ScopeType.Roles))
        {
            identity.AddClaim(new Claim(ClaimType.Role, context.AuthenticatedUser!.Role));
        }

        return Task.CompletedTask;
    }
}