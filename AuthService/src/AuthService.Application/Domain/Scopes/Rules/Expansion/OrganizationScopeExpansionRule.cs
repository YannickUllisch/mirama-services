
using AuthService.Application.Domain.Authorization.Interfaces;
using AuthService.Application.Domain.Scopes.Interfaces;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace AuthService.Application.Domain.Scopes.Rules.Expansion;

public sealed class OrganizationScopeExpansionRule : IScopeRule
{
    public ScopeRulePhase Phase => ScopeRulePhase.Expansion;
    public IEnumerable<string> SupportedGrantTypes => [GrantTypes.AuthorizationCode, GrantTypes.TokenExchange, GrantTypes.RefreshToken];

    public bool IsApplicable(IAuthorizationContext context)
        => context.GrantedScopes.Contains(ScopeType.Organization);

    public void Apply(IAuthorizationContext context)
    {
        // If organization scope is granted after validation rule-set, we need to give user access to all Services, 
        // even if not requested to maintain functionality of frontend.
        // Some of these scopes may be filtered out again in the Filtering phase based on e.g. Roles
        context.GrantedScopes.Add(ScopeType.AccountRead);
        context.GrantedScopes.Add(ScopeType.AccountWrite);

        context.GrantedScopes.Add(ScopeType.ProjectRead);
        context.GrantedScopes.Add(ScopeType.ProjectWrite);

        context.GrantedScopes.Add(ScopeType.LLMRead);
        context.GrantedScopes.Add(ScopeType.LLMWrite);
    }
}
