using System.Text.Json.Serialization;
using Mirama.Modules.PM.Domain.Aggregates.Project.Milestone;

namespace Mirama.Modules.PM.Application.Features.V1.Projects.Milestones;

internal static class ProjectMilestoneMapper
{
    internal static ProjectMilestoneResponse ToResponse(ProjectMilestone milestone) => new()
    {
        Id = milestone.Id.Value,
        Title = milestone.Title,
        Description = milestone.Description,
        DueDate = milestone.DueDate,
        Status = milestone.Status.ToString(),
        Color = milestone.Color,
        DateCreated = milestone.DateCreated
    };
}

public sealed record ProjectMilestoneResponse
{
    [JsonPropertyName("id")]
    public Guid Id { get; init; }

    [JsonPropertyName("title")]
    public string Title { get; init; } = string.Empty;

    [JsonPropertyName("description")]
    public string? Description { get; init; }

    [JsonPropertyName("dueDate")]
    public DateTime DueDate { get; init; }

    [JsonPropertyName("status")]
    public string Status { get; init; } = string.Empty;

    [JsonPropertyName("color")]
    public string? Color { get; init; }

    [JsonPropertyName("dateCreated")]
    public DateTime DateCreated { get; init; }
}
