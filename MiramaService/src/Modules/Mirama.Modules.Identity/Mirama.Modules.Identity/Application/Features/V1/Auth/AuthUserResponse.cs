
using System.Text.Json.Serialization;
using Mirama.Modules.Identity.Domain.Aggregates.User;

namespace Mirama.Modules.Identity.Application.Features.V1.Auth;

internal static class AuthUserResponseMapper
{
    internal static AuthUserResponse MapAuthUserResponse(this User user, Guid tenantId, AuthOrgMembershipResponse? orgMembership)
    {
        return new()
        {
            Id = user.Id.Value,
            TenantId = tenantId,
            Name = user.Name,
            Email = user.Email,
            IsOnboarded = user.IsOnboarded,
            OrganizationInfo = orgMembership,
            Image = user.Image,
        };
    }
}

public sealed record AuthUserResponse
{
    [JsonPropertyName("id")]
    public Guid Id { get; init; }

    [JsonPropertyName("tenantId")]
    public Guid TenantId { get; init; }

    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;

    [JsonPropertyName("email")]
    public string Email { get; init; } = string.Empty;

    [JsonPropertyName("isOnboarded")]
    public bool IsOnboarded { get; init; }

    [JsonPropertyName("organizationInfo")]
    public AuthOrgMembershipResponse? OrganizationInfo { get; init; }

    [JsonPropertyName("image")]
    public string? Image { get; init; }
}