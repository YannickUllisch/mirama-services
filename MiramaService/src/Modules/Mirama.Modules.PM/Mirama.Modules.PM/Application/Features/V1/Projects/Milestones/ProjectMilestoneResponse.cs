using Mirama.Modules.PM.Domain.Aggregates.Project.Milestone;

namespace Mirama.Modules.PM.Application.Features.V1.Projects.Milestones;

public sealed record ProjectMilestoneResponse(
    Guid MilestoneId,
    string Title,
    string? Description,
    DateTime DueDate,
    string Status,
    string? Color,
    DateTime DateCreated);

internal static class ProjectMilestoneMapper
{
    internal static ProjectMilestoneResponse ToResponse(ProjectMilestone milestone) =>
        new(milestone.Id.Value, milestone.Title, milestone.Description, milestone.DueDate, milestone.Status.ToString(), milestone.Color, milestone.DateCreated);
}
