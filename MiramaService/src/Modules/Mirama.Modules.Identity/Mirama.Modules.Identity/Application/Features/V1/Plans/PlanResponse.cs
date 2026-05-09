using System.Text.Json.Serialization;

namespace Mirama.Modules.Identity.Application.Features.V1.Plans;

public sealed record PlanResponse
{
    [JsonPropertyName("id")]
    public Guid Id { get; init; }

    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;

    [JsonPropertyName("description")]
    public string? Description { get; init; }

    [JsonPropertyName("price")]
    public int Price { get; init; }

    [JsonPropertyName("interval")]
    public string Interval { get; init; } = string.Empty;

    [JsonPropertyName("features")]
    public List<string> Features { get; init; } = [];
}
