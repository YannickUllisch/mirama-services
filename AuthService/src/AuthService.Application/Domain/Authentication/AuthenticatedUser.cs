

using AuthService.Application.Domain.Authentication.Interfaces;

namespace AuthService.Application.Domain.Authentication;

public class AuthenticatedUser : IAuthenticatedUser
{
    public string UserId { get; init; } = string.Empty;
    public string TenantId { get; init; } = string.Empty;
    public string? OrganizationId { get; init; } = null;
    public string Email { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string Role { get; init; } = string.Empty;
    public bool IsActive { get; init; } = false;
    public string? Image { get; init; }
}