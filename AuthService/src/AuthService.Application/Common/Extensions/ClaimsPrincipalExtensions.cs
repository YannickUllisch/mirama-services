
using System.Security.Claims;

namespace AuthService.Application.Common.Extension;

internal static class ClaimsPrincipalExtensions
{
    public static string GetRequiredClaim(this ClaimsPrincipal principal, params string[] types)
    {
        foreach (var type in types)
        {
            var value = principal.FindFirst(type)?.Value;
            if (!string.IsNullOrWhiteSpace(value))
            {
                return value;
            }
        }

        throw new MissingFieldException($"Missing required claim: {string.Join(", ", types)}");
    }

    public static string? GetOptionalClaim(this ClaimsPrincipal principal, params string[] types)
    {
        foreach (var type in types)
        {
            var value = principal.FindFirst(type)?.Value;
            if (!string.IsNullOrWhiteSpace(value))
            {
                return value;
            }
        }

        return null;
    }
}
