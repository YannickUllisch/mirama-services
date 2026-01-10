

using AuthService.Application.Common;
using AuthService.Application.Common.Interfaces;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace AuthService.Application.Domain.Scopes.Rules;

public sealed class ClientCredentialsScopeRule : IScopeRule
{
    public IEnumerable<string> SupportedGrantTypes => [GrantTypes.ClientCredentials];

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
