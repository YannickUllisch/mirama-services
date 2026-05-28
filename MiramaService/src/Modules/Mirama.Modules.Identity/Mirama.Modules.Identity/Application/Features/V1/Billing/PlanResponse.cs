using System.Text.Json.Serialization;
using Mirama.Modules.Identity.Domain.Aggregates.Plan;

namespace Mirama.Modules.Identity.Application.Features.V1.Billing;

internal static class PlanMapper
{
    internal static PlanResponse MapResponse(this Plan plan) => new()
    {
        Id = plan.Id.Value,
        Name = plan.Name,
        Description = plan.Description,
        Price = plan.Price,
        Interval = plan.Interval,
        Features = [.. plan.Features],
    };
}

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
