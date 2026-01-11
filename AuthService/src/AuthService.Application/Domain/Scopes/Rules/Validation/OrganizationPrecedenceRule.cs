
using AuthService.Application.Domain.Authorization.Interfaces;
using AuthService.Application.Domain.Scopes.Interfaces;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace AuthService.Application.Domain.Scopes.Rules.Validation;

public sealed class OrganizationPrecedenceRule : IScopeRule
{
    public IEnumerable<string> SupportedGrantTypes => [GrantTypes.AuthorizationCode, GrantTypes.TokenExchange, GrantTypes.RefreshToken];
    public ScopeRulePhase Phase => ScopeRulePhase.Validation;

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
