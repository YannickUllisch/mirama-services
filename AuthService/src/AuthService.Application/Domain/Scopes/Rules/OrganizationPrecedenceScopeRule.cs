

using AuthService.Application.Common;
using AuthService.Application.Common.Interfaces;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace AuthService.Application.Domain.Scopes.Rules;

public sealed class OrganizationPrecedenceScopeRule : IScopeRule
{
    public IEnumerable<string> SupportedGrantTypes => [GrantTypes.AuthorizationCode, GrantTypes.TokenExchange, GrantTypes.RefreshToken];

    public bool IsApplicable(IAuthorizationContext context)
        => SupportedGrantTypes.Contains(context.GrantType);

    public void Apply(IAuthorizationContext context)
    {
        if (context.GrantedScopes.Contains(ScopeType.Organization))
        {
            // If orgId was not sent as parameter in request, we cannot allow organization scope
            // otherwise of Org was requested and Id is sent, we do disallow the tenant scope
            if (context.OrganizationId != null)
            {
                context.GrantedScopes.Remove(ScopeType.Tenant);
            }
            else
            {
                context.GrantedScopes.Remove(ScopeType.Organization);
            }
        }

    }
}
