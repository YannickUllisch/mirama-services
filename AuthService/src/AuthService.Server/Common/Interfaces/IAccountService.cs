
namespace AuthService.Server.Common.Interfaces;

public interface IAccountService
{
    void GetOrCreateUserAsync(string userId, string email);
}