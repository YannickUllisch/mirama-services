
namespace AuthService.Application.Infrastructure.HttpServices;

public interface IAccountService
{
    Task CreateUserAsync(string userId, string email);
    Task GetUserTenantAsync(string userId);
    Task GetUserWithOrganizationAsync(string userId, string orgId);
}