using System.Security.Claims;
using AuthService.Application.Domain.Authentication.Interfaces;

namespace AuthService.Application.Domain.Authorization.Interfaces;

public interface IAuthorizationContext
{
    ClaimsPrincipal Subject { get; }
    string GrantType { get; }
    Guid? OrganizationId { get; }
    ISet<string> RequestedScopes { get; }
    ISet<string> GrantedScopes { get; set; }
    IAuthenticatedUser? AuthenticatedUser { get; set; }
    string? ClientId { get; set; }
    
    DelegationContext? Delegation { get; set; }

    bool IsRejected { get; }
    string? Error { get; }
    string? ErrorDescription { get; }
    
    void Reject(string error, string description);
}