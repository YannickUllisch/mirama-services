using System.Text.Json.Serialization;

namespace Mirama.Modules.Identity.Application.Features.V1.Organizations.Members;

public sealed record MemberResponse
{
    [JsonPropertyName("id")]
    public Guid Id { get; init; }

    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;

    [JsonPropertyName("email")]
    public string Email { get; init; } = string.Empty;

    [JsonPropertyName("userId")]
    public Guid? UserId { get; init; }

    [JsonPropertyName("iamRoleId")]
    public Guid IamRoleId { get; init; }

    [JsonPropertyName("organizationId")]
    public Guid OrganizationId { get; init; }
}
