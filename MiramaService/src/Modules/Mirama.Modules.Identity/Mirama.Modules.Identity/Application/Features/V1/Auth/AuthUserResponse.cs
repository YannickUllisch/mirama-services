
using System.Text.Json.Serialization;
using Mirama.Modules.Identity.Domain.Aggregates.User;

namespace Mirama.Modules.Identity.Application.Features.V1.Auth;

internal static class AuthUserResponseMapper
{
    internal static AuthUserResponse MapAuthUserResponse(this User user, Guid tenantId, TenantRole tenantRole, AuthOrgMembershipResponse? orgMembership)
    {
        return new()
        {
            UserId = user.Id.Value,
            TenantId = tenantId,
            TenantRole = Enum.GetName(tenantRole)!,
            Name = user.Name,
            Email = user.Email,
            OrganizationInfo = orgMembership,
            Image = user.Image,
        };
    }
}

public sealed record AuthUserResponse
{
    [JsonPropertyName("userId")]
    public Guid UserId { get; init; }

    [JsonPropertyName("tenantId")]
    public Guid TenantId { get; init; }

    [JsonPropertyName("tenantRole")]
    public string TenantRole { get; init; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;

    [JsonPropertyName("email")]
    public string Email { get; init; } = string.Empty;

    [JsonPropertyName("organizationInfo")]
    public AuthOrgMembershipResponse? OrganizationInfo { get; init; }

    [JsonPropertyName("image")]
    public string? Image { get; init; }
}