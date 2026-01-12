
using AuthService.Application.Domain.Authentication;
using AuthService.Application.Domain.Authentication.Interfaces;

namespace AuthService.Application.Infrastructure.HttpServices;

public sealed class AccountHttpClient : IAccountHttpClient
{
    // private readonly HttpClient _client = client;

    public Task<IAuthenticatedUser> GetOrCreateUserAsync(string accountId, string email, string name, string address, string? image, string? orgId)
    {
        return Task.FromResult<IAuthenticatedUser>(new AuthenticatedUser
        {
            UserId = Guid.NewGuid().ToString(),
            TenantId = Guid.NewGuid().ToString(),
            OrganizationId = orgId,
            Email = email,
            Name = name,
            Role = "admin",
            IsActive = true,
            Image = image,
        });
    }

    public Task<IAuthenticatedUser> GetUserAsync(string userId, string? orgId)
    {
        // var accountUri = $"/user?id=${userId}";

        // if (orgId != null)
        // {
        //     accountUri += $"&organizationId={orgId}";
        // }
        // var response = await _client.GetAsync(accountUri);

        return Task.FromResult<IAuthenticatedUser>(new AuthenticatedUser
        {
            UserId = Guid.NewGuid().ToString(),
            TenantId = Guid.NewGuid().ToString(),
            OrganizationId = orgId,
            Email = "user@example.com",
            Name = "John Doe",
            Role = "admin",
            IsActive = true,
            Image = "https://example.com/avatar.png"
        });
    }
}