
using AuthService.Application.Common.Interfaces;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace AuthService.Application.Domain.Scopes.Rules.Expansion;

public sealed class TenantScopeExpansionRule : IScopeRule
{
    public ScopeRulePhase Phase => ScopeRulePhase.Expansion;
    public IEnumerable<string> SupportedGrantTypes => [GrantTypes.AuthorizationCode, GrantTypes.TokenExchange, GrantTypes.RefreshToken];

    public bool IsApplicable(IAuthorizationContext context)
        => context.GrantedScopes.Contains(ScopeType.Tenant);

    public void Apply(IAuthorizationContext context)
    {
        // If Tenant Scope is granted we need to allow Account access even if not requested
        // We do not have to check for duplicates since GrantedScopes is a hash set
        context.GrantedScopes.Add(ScopeType.AccountRead);
        context.GrantedScopes.Add(ScopeType.AccountWrite);
    }
}
