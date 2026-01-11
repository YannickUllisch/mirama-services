
using System.Security.Claims;
using AuthService.Application.Domain.Authorization.Interfaces;
using AuthService.Application.Domain.Claims.Interfaces;
using AuthService.Application.Domain.Scopes;
using OpenIddict.Abstractions;

namespace AuthService.Application.Domain.Claims.Contributors;

public sealed class TenantClaimContributor : IClaimContributor
{
    public bool IsApplicable(IAuthorizationContext context)
        => context.GrantedScopes.Contains(ScopeType.Tenant);

    public Task Contribute(IAuthorizationContext context, ClaimsIdentity identity)
    {
        identity.SetClaim(ClaimType.Tenant, context.AuthenticatedUser!.TenantId);
        
        return Task.CompletedTask;
    }
}
