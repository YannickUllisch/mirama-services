
namespace AuthService.Server.Common.Interfaces;

public interface IAccountService
{
    Task CreateUserAsync(string userId, string email);
    Task GetUserTenantAsync(string userId);
    Task GetUserWithOrganizationAsync(string userId, string orgId);
}