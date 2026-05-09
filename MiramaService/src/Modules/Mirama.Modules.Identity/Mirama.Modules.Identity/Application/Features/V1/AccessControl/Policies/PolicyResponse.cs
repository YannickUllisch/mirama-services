using System.Text.Json.Serialization;

namespace Mirama.Modules.Identity.Application.Features.V1.AccessControl.Policies;

public sealed record PolicyResponse
{
    [JsonPropertyName("id")]
    public Guid Id { get; init; }

    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;

    [JsonPropertyName("description")]
    public string? Description { get; init; }

    [JsonPropertyName("scope")]
    public string Scope { get; init; } = string.Empty;

    [JsonPropertyName("isManaged")]
    public bool IsManaged { get; init; }

    [JsonPropertyName("isSystemPolicy")]
    public bool IsSystemPolicy { get; init; }

    [JsonPropertyName("statements")]
    public List<PolicyStatementResponse> Statements { get; init; } = [];
}

public sealed record PolicyStatementResponse
{
    [JsonPropertyName("id")]
    public Guid Id { get; init; }

    [JsonPropertyName("action")]
    public string Action { get; init; } = string.Empty;

    [JsonPropertyName("resource")]
    public string Resource { get; init; } = string.Empty;

    [JsonPropertyName("effect")]
    public string Effect { get; init; } = string.Empty;
}
