namespace Mirama.Modules.PM.Domain.Aggregates.Project.Milestone;

public sealed record ProjectMilestoneDetails(
    string Title,
    DateTime DueDate,
    string? Description = null,
    string? Color = null);
