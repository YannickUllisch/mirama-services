
namespace AuthService.Application.Domain.Authentication.Interfaces;

public interface IAuthenticatedUser
{
    public string UserId { get; init; }
    public string TenantId { get; init; }
    public string? OrganizationId { get; init; }
    public string Email { get; init; }
    public string Name { get; init; }
    public string Role { get; init; }
    public bool IsActive { get; init; }
    public string? Image { get; init; }
}