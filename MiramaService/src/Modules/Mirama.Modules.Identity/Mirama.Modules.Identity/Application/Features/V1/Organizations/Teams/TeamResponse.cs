using System.Text.Json.Serialization;

namespace Mirama.Modules.Identity.Application.Features.V1.Organizations.Teams;

public sealed record TeamResponse
{
    [JsonPropertyName("id")]
    public Guid Id { get; init; }

    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;

    [JsonPropertyName("slug")]
    public string Slug { get; init; } = string.Empty;

    [JsonPropertyName("dateCreated")]
    public DateTime DateCreated { get; init; }

    [JsonPropertyName("organizationId")]
    public Guid OrganizationId { get; init; }

    [JsonPropertyName("memberIds")]
    public List<Guid> MemberIds { get; init; } = [];
}
