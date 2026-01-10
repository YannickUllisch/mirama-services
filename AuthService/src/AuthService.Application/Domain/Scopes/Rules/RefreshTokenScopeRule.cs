
using AuthService.Application.Common;
using AuthService.Application.Common.Interfaces;
using OpenIddict.Abstractions;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace AuthService.Application.Domain.Scopes.Rules;

public sealed class RefreshTokenScopeRule : IScopeRule
{
    public IEnumerable<string> SupportedGrantTypes => [GrantTypes.RefreshToken];


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
