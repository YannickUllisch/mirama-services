
using System.Security.Claims;
using AuthService.Application.Domain.Scopes;
using OpenIddict.Abstractions;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace AuthService.Application.Domain.Claims;

public static class ClaimDestinations
{
    public static IEnumerable<string> GetDestinations(Claim claim)
    {
        // Note: by default, claims are NOT automatically included in the access and identity tokens.
        // To allow OpenIddict to serialize them, you must attach them a destination, that specifies
        // whether they should be included in access tokens, in identity tokens or in both.

        switch (claim.Type)
        {
            case ClaimType.Name:
                yield return Destinations.AccessToken;

                if (claim.Subject!.HasScope(ScopeType.Profile))
                    yield return Destinations.IdentityToken;

                yield break;

            case ClaimType.Email:
                yield return Destinations.AccessToken;

                if (claim.Subject!.HasScope(ScopeType.Email))
                    yield return Destinations.IdentityToken;

                yield break;

            case ClaimType.Role:
                yield return Destinations.AccessToken;

                if (claim.Subject!.HasScope(ScopeType.Roles))
                    yield return Destinations.IdentityToken;

                yield break;

            case ClaimType.Profile:
                yield return Destinations.AccessToken;

                if (claim.Subject!.HasScope(ScopeType.Profile))
                    yield return Destinations.IdentityToken;

                yield break;

            case ClaimType.Organization:
                yield return Destinations.AccessToken;

                if (claim.Subject!.HasScope(ScopeType.Organization) == true)
                    yield return Destinations.IdentityToken;

                yield break;

            case ClaimType.Tenant:
                yield return Destinations.AccessToken;

                if (claim.Subject!.HasScope(ScopeType.Tenant) == true)
                    yield return Destinations.IdentityToken;

                yield break;

            // Never include the security stamp in the access and identity tokens, as it's a secret value.
            case "AspNet.Identity.SecurityStamp": yield break;

            default:
                yield return Destinations.AccessToken;
                yield break;
        }
    }
}