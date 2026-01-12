
using AuthService.Application.Domain.Authorization.Interfaces;
using AuthService.Application.Domain.Scopes.Interfaces;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace AuthService.Application.Domain.Scopes.Rules.Validation;

public sealed class OpenIdScopeRequiredRule : IScopeRule
{
    public IEnumerable<string> SupportedGrantTypes => [GrantTypes.AuthorizationCode, GrantTypes.TokenExchange, GrantTypes.RefreshToken];

    public ScopeRulePhase Phase => ScopeRulePhase.Validation;

    // We validate ClientId since, client creds should also be allowed to run TokenExchange without needing OpenId scope.
    public bool IsApplicable(IAuthorizationContext context)
        => SupportedGrantTypes.Contains(context.GrantType) && context.ClientId == null;

    public void Apply(IAuthorizationContext context)
    {
        var hasOpenId = context.GrantedScopes.Contains(ScopeType.OpenId);

        if (!hasOpenId)
        {
            context.Reject(Errors.InvalidScope, "OpenId scope must be requested.");
        }
    }
}
