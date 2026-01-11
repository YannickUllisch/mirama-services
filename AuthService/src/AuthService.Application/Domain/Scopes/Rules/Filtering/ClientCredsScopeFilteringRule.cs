
using AuthService.Application.Domain.Authorization.Interfaces;
using AuthService.Application.Domain.Scopes.Interfaces;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace AuthService.Application.Domain.Scopes.Rules.Filtering;

public sealed class ClientCredsScopeFilteringRule : IScopeRule
{
    public IEnumerable<string> SupportedGrantTypes => [GrantTypes.ClientCredentials];
    public ScopeRulePhase Phase => ScopeRulePhase.Filtering;

    private static readonly HashSet<string> Allowed =
    [
        ScopeType.AccountRead,
        ScopeType.AccountWrite,
        ScopeType.ProjectRead,
        ScopeType.ProjectWrite,
        ScopeType.LLMRead,
        ScopeType.LLMWrite
    ];

    public bool IsApplicable(IAuthorizationContext context)
        => SupportedGrantTypes.Contains(context.GrantType);

    public void Apply(IAuthorizationContext context)
    {
        context.GrantedScopes.IntersectWith(Allowed);
    }
}
