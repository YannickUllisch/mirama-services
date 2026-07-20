using System.Text.Json.Serialization;
using Mirama.Modules.PM.Domain.Aggregates.WorkflowConfig.Status;

namespace Mirama.Modules.PM.Application.Features.V1.Projects.Workflow.Statuses;

internal static class StatusMapper
{
    internal static StatusResponse ToResponse(StatusConfig status) => new()
    {
        Id = status.Id.Value,
        Name = status.Name,
        Color = status.Color,
        Category = status.Category.ToString(),
        Position = status.Position,
        IsDefault = status.IsDefault,
        IsTerminal = status.IsTerminal
    };
}

public sealed record StatusResponse
{
    [JsonPropertyName("id")]
    public Guid Id { get; init; }

    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;

    [JsonPropertyName("color")]
    public string? Color { get; init; }

    [JsonPropertyName("category")]
    public string Category { get; init; } = string.Empty;

    [JsonPropertyName("position")]
    public int Position { get; init; }

    [JsonPropertyName("isDefault")]
    public bool IsDefault { get; init; }

    [JsonPropertyName("isTerminal")]
    public bool IsTerminal { get; init; }
}
