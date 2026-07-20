using System.Text.Json.Serialization;
using Mirama.Modules.PM.Domain.Aggregates.WorkflowConfig.Priority;

namespace Mirama.Modules.PM.Application.Features.V1.Projects.Workflow.Priorities;

internal static class PriorityMapper
{
    internal static PriorityResponse ToResponse(PriorityConfig priority) => new()
    {
        Id = priority.Id.Value,
        Name = priority.Name,
        Color = priority.Color,
        Icon = priority.Icon,
        Level = priority.Level,
        IsDefault = priority.IsDefault
    };
}

public sealed record PriorityResponse
{
    [JsonPropertyName("id")]
    public Guid Id { get; init; }

    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;

    [JsonPropertyName("color")]
    public string? Color { get; init; }

    [JsonPropertyName("icon")]
    public string? Icon { get; init; }

    [JsonPropertyName("level")]
    public int Level { get; init; }

    [JsonPropertyName("isDefault")]
    public bool IsDefault { get; init; }
}
