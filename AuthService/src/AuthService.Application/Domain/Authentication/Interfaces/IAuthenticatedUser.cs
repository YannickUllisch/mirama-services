
namespace AuthService.Application.Domain.Authentication.Interfaces;

public interface IAuthenticatedUser
{
    public Guid UserId { get; init; }
    public Guid TenantId { get; init; }
    public Guid? OrganizationId { get; init; }
    public string Email { get; init; }
    public string Name { get; init; }
    public string Role { get; init; }
    public bool IsActive { get; init; }
    public string? Image { get; init; }
}