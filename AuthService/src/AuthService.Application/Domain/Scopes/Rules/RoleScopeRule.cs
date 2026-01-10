
using System.Security.Claims;
using AuthService.Application.Common;
using AuthService.Application.Common.Interfaces;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace AuthService.Application.Domain.Scopes.Rules;

public sealed class RoleScopeRule : IScopeRule
{
    public IEnumerable<string> SupportedGrantTypes => [GrantTypes.AuthorizationCode, GrantTypes.RefreshToken, GrantTypes.TokenExchange];

    public static readonly Dictionary<string, string[]> AllowedScopesByRole = new()
    {
        ["owner"] =
        [
            ScopeType.OpenId,
            ScopeType.Email,
            ScopeType.Roles,
            ScopeType.Profile,
            ScopeType.OfflineAccess,
            ScopeType.Tenant,
            ScopeType.Organization,
            ScopeType.AccountRead,
            ScopeType.AccountWrite,
            ScopeType.ProjectRead,
            ScopeType.ProjectWrite,
            ScopeType.LLMRead,
            ScopeType.LLMWrite
        ],
        ["admin"] =
        [
            ScopeType.OpenId,
            ScopeType.Email,
            ScopeType.Roles,
            ScopeType.Profile,
            ScopeType.OfflineAccess,
            ScopeType.Tenant,
            ScopeType.Organization,
            ScopeType.AccountRead,
            ScopeType.AccountWrite,
            ScopeType.ProjectRead,
            ScopeType.ProjectWrite,
            ScopeType.LLMRead,
            ScopeType.LLMWrite
        ],
        ["user"] =
        [
            ScopeType.OpenId,
            ScopeType.Email,
            ScopeType.Roles,
            ScopeType.Profile,
            ScopeType.OfflineAccess,
            ScopeType.Tenant,
            ScopeType.Organization,
            ScopeType.AccountRead,
            ScopeType.AccountWrite,
            ScopeType.ProjectRead,
            ScopeType.ProjectWrite,
        ]
    };

    public void Apply(IAuthorizationContext context)
    {
        var role = context.AuthenticatedUser!.Role;

        if (!AllowedScopesByRole.TryGetValue(role, out var allowedScopes))
        {
            context.Reject("invalid_request", $"Role '{role}' is not recognized.");
            return;
        }

        context.RequestedScopes.IntersectWith(allowedScopes);
    }

    public bool IsApplicable(IAuthorizationContext context)
        => SupportedGrantTypes.Contains(context.GrantType);
}
