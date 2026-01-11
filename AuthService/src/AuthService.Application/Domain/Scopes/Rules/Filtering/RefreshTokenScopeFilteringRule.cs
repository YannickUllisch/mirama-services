
using AuthService.Application.Domain.Authorization.Interfaces;
using AuthService.Application.Domain.Scopes.Interfaces;
using OpenIddict.Abstractions;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace AuthService.Application.Domain.Scopes.Rules.Filtering;

public sealed class RefreshTokenScopeFilteringRule : IScopeRule
{
    public IEnumerable<string> SupportedGrantTypes => [GrantTypes.RefreshToken];

    public ScopeRulePhase Phase => ScopeRulePhase.Filtering;

    public void Apply(IAuthorizationContext context)
    {
        var originalScopes = context.Subject
            .GetScopes()
            .ToHashSet();

        context.GrantedScopes.IntersectWith(originalScopes);
    }

    public bool IsApplicable(IAuthorizationContext context)
        => SupportedGrantTypes.Contains(context.GrantType);
}
