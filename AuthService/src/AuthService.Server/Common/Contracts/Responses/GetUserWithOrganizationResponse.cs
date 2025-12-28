
using System.Text.Json.Serialization;

namespace AuthService.Server.Common.Contracts.Responses;

public sealed record GetUserWithOrganizationResponse
{
    [JsonPropertyName("userId")]
    public Guid UserId { get; init; } = Guid.Empty;

    [JsonPropertyName("tenantId")]
    public Guid TenantId { get; init; } = Guid.Empty;

    [JsonPropertyName("organizationId")]
    public Guid OrganizationId { get; init; } = Guid.Empty;

    [JsonPropertyName("email")]
    public string Email { get; init; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;

    [JsonPropertyName("role")]
    public string Role { get; init; } = string.Empty;

    [JsonPropertyName("isActive")]
    public bool IsActive { get; init; }

    [JsonPropertyName("image")]
    public string? Image { get; init; } = string.Empty;
}