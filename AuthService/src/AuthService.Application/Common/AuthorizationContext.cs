

using System.Security.Claims;
using AuthService.Application.Common.Interfaces;

namespace AuthService.Application.Common;

public sealed record AuthorizationContext : IAuthorizationContext
{
    public ClaimsPrincipal Subject { get; init; } = default!;
    public string GrantType { get; init; } = string.Empty;
    public Guid? OrganizationId { get; init; } = null;
    public ISet<string> RequestedScopes { get; init; } = new HashSet<string>();
    public ISet<string> GrantedScopes { get; set; } = new HashSet<string>();

    public IAuthenticatedUser? AuthenticatedUser { get; set; }
    public string? ClientId { get; set; } = null;

    public DelegationContext? Delegation { get; set; }

    public bool IsRejected { get; private set; } = false;
    public string? Error { get; private set; } = null;
    public string? ErrorDescription { get; private set; } = null;

    public void Reject(string error, string description)
    {
        IsRejected = true;
        Error = error;
        ErrorDescription = description;
    }
}
