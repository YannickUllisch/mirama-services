
using AuthService.Application.Domain.Authentication.Interfaces;

namespace AuthService.Application.Infrastructure.HttpServices;

public interface IAccountHttpClient
{
    Task<IAuthenticatedUser> GetOrCreateUserAsync(string accountId, string email, string name, string address, string? image, string? orgId);
    Task<IAuthenticatedUser> GetUserAsync(string userId, string? orgId);
}