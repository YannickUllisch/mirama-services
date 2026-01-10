
using AuthService.Application.Common.Interfaces;

namespace AuthService.Application.Common;

public sealed class AuthorizationDecision
{
    public bool IsDenied { get; }
    public string? Error { get; }
    public string? ErrorDescription { get; }
    public IAuthorizationContext Context { get; }

    private AuthorizationDecision(bool isDenied, string? error, string? errorDescription, IAuthorizationContext context)
    {
        IsDenied = isDenied;
        Error = error;
        ErrorDescription = errorDescription;
        Context = context;
    }

    public static AuthorizationDecision Success(IAuthorizationContext ctx)
        => new(false, null, null, ctx);

    public static AuthorizationDecision Deny(
        string error,
        string description)
        => new(true, error, description, null!);
}
