
using System.Net.Http.Headers;
using AuthService.Application.Common.Interfaces;
using AuthService.Application.Common.Types;
using AuthService.Application.Domain.Authentication;
using AuthService.Application.Domain.Authentication.Interfaces;
using AuthService.Application.Domain.Scopes;

namespace AuthService.Application.Infrastructure.HttpServices;

public sealed class AccountHttpClient(HttpClient client, IInternalTokenService tokenService) : IAccountHttpClient
{
    private readonly HttpClient _client = client;
    private readonly IInternalTokenService _tokenService = tokenService;

    public async Task<IAuthenticatedUser> GetOrCreateUserAsync(string accountId, string email, string name, string address, string? image, string? orgId)
    {   
        // Generating internal JWT to securely access Account Microservice
        var accessToken = _tokenService.IssueInternalAccessToken([ScopeType.AccountRead], ResourceType.Account);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var payload = new
        {
            AccountId = accountId,
            Email = email,
            Name = name,
            Address = address,
            Image = image,
            OrganizationId = orgId
        };
        
        // Generating content
        var content = System.Net.Http.Json.JsonContent.Create(payload);
        var response = await _client.PostAsync("/account", content);

        Console.WriteLine(response.StatusCode);
        return new AuthenticatedUser
        {
            UserId = Guid.NewGuid().ToString(),
            TenantId = Guid.NewGuid().ToString(),
            OrganizationId = orgId,
            Email = email,
            Name = name,
            Role = "admin",
            IsActive = true,
            Image = image,
        };
    }

    public async Task<IAuthenticatedUser> GetUserAsync(string userId, string? orgId)
    {
        var accessToken = _tokenService.IssueInternalAccessToken([ScopeType.AccountRead], ResourceType.Account);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        
        // Constructing requestUri
        var accountUri = $"/user?id=${userId}";
        if (orgId != null)
        {
            accountUri += $"&organizationId={orgId}";
        }
        var response = await _client.GetAsync(accountUri);

        return new AuthenticatedUser
        {
            UserId = Guid.NewGuid().ToString(),
            TenantId = Guid.NewGuid().ToString(),
            OrganizationId = orgId,
            Email = "user@example.com",
            Name = "John Doe",
            Role = "admin",
            IsActive = true,
            Image = "https://example.com/avatar.png"
        };
    }
}