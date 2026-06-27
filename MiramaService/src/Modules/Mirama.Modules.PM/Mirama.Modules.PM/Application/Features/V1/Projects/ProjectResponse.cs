using Mirama.Modules.PM.Application.Features.V1.Projects.Members;
using Mirama.Modules.PM.Application.Features.V1.Projects.Milestones;
using Mirama.Modules.PM.Domain.Aggregates.Project;

namespace Mirama.Modules.PM.Application.Features.V1.Projects;

public sealed record ProjectResponse(
    Guid ProjectId,
    string Name,
    string Slug,
    string? Description,
    DateTime StartDate,
    DateTime? EndDate,
    Guid StatusId,
    Guid PriorityId,
    int Budget,
    bool IsArchived,
    DateTime DateCreated,
    List<Guid> TagIds,
    List<ProjectMemberResponse> Members,
    List<Guid> TeamIds,
    List<ProjectMilestoneResponse> Milestones);

internal static class ProjectMapper
{
    internal static ProjectResponse ToResponse(Project project) =>
        new(
            project.Id.Value,
            project.Name,
            project.Slug,
            project.Description,
            project.StartDate,
            project.EndDate,
            project.StatusId,
            project.PriorityId,
            project.Budget,
            project.IsArchived,
            project.DateCreated,
            project.TagIds,
            project.Members.Select(ProjectMemberMapper.ToResponse).ToList(),
            project.Teams.Select(t => t.TeamId).ToList(),
            project.Milestones.Select(ProjectMilestoneMapper.ToResponse).ToList());
}
