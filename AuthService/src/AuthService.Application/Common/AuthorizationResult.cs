
using System.Security.Claims;

namespace AuthService.Application.Common;

public sealed class AuthorizationResult
{
    public bool IsDenied { get; }
    public string? Error { get; }
    public string? ErrorDescription { get; }
    public ClaimsPrincipal? Principal { get; }

    private AuthorizationResult(
        bool isDenied,
        string? error,
        string? errorDescription,
        ClaimsPrincipal? principal)
    {
        IsDenied = isDenied;
        Error = error;
        ErrorDescription = errorDescription;
        Principal = principal;
    }

    public static AuthorizationResult Deny(string error, string description)
        => new(true, error, description, null);

    public static AuthorizationResult Success(ClaimsPrincipal principal)
        => new(false, null, null, principal);
}
