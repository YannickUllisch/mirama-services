using System.Text.Json.Serialization;

namespace Mirama.Modules.Identity.Application.Features.V1.AccessControl.Roles;

public sealed record RoleResponse
{
    [JsonPropertyName("id")]
    public Guid Id { get; init; }

    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;

    [JsonPropertyName("description")]
    public string? Description { get; init; }

    [JsonPropertyName("scope")]
    public string Scope { get; init; } = string.Empty;

    [JsonPropertyName("isSystemRole")]
    public bool IsSystemRole { get; init; }

    [JsonPropertyName("policyIds")]
    public List<Guid> PolicyIds { get; init; } = [];
}
