
using AuthService.Application.Domain.Authorization.Interfaces;
using AuthService.Application.Domain.Scopes.Interfaces;
using OpenIddict.Abstractions;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace AuthService.Application.Domain.Scopes.Rules.Validation;

public sealed class TenantOrOrganizationScopeRequiredRule : IScopeRule
{
    public IEnumerable<string> SupportedGrantTypes => [GrantTypes.AuthorizationCode, GrantTypes.TokenExchange, GrantTypes.RefreshToken];

    public ScopeRulePhase Phase => ScopeRulePhase.Validation;

    public bool IsApplicable(IAuthorizationContext context)
        => SupportedGrantTypes.Contains(context.GrantType);

    public void Apply(IAuthorizationContext context)
    {
        var hasTenant = context.GrantedScopes.Contains(ScopeType.Tenant);
        var hasOrg = context.GrantedScopes.Contains(ScopeType.Organization);

        if (!hasTenant && !hasOrg)
        {
            context.Reject(
                OpenIddictConstants.Errors.InvalidScope,
                "Either the 'tenant' or 'organization' scope must be requested.");
        }
    }
}
