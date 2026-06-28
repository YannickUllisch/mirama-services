using Mirama.Modules.Identity.Contracts.Organizations;
using Mirama.Modules.Identity.Contracts.Tags;
using Mirama.Modules.PM.Application.Features.V1.Projects.Members;
using Mirama.Modules.PM.Application.Features.V1.Projects.Milestones;
using Mirama.Modules.PM.Application.Features.V1.Projects.Priorities;
using Mirama.Modules.PM.Application.Features.V1.Projects.Statuses;
using Mirama.Modules.PM.Application.Features.V1.Projects.Teams;
using Mirama.Modules.PM.Domain.Aggregates.Project;
using Mirama.Modules.PM.Domain.Aggregates.WorkflowConfig.Priority;
using Mirama.Modules.PM.Domain.Aggregates.WorkflowConfig.Status;

namespace Mirama.Modules.PM.Application.Features.V1.Projects;

internal static class ProjectMapper
{
    internal static ProjectResponse ToResponse(
        Project project,
        Dictionary<Guid, StatusConfig> statusLookup,
        Dictionary<Guid, PriorityConfig> priorityLookup,
        Dictionary<Guid, TagDto> tagLookup,
        Dictionary<Guid, MemberDto> memberLookup,
        Dictionary<Guid, TeamDto> teamLookup)
    {
        statusLookup.TryGetValue(project.StatusId, out var status);
        priorityLookup.TryGetValue(project.PriorityId, out var priority);

        return new ProjectResponse(
            project.Id.Value,
            project.Name,
            project.Slug,
            project.Description,
            project.StartDate,
            project.EndDate,
            status is not null
                ? StatusMapper.ToResponse(status)
                : new StatusResponse(project.StatusId, "Unknown", null, "Unknown", 0, false, false),
            priority is not null
                ? PriorityMapper.ToResponse(priority)
                : new PriorityResponse(project.PriorityId, "Unknown", null, null, 0, false),
            project.Budget,
            project.IsArchived,
            project.DateCreated,
            project.TagIds
                .Where(tagLookup.ContainsKey)
                .Select(id => { var t = tagLookup[id]; return new ProjectTagResponse(t.Id, t.Name, t.Slug, t.Color); })
                .ToList(),
            project.Members
                .Where(m => memberLookup.ContainsKey(m.MemberId))
                .Select(m => ProjectMemberMapper.ToResponse(m, memberLookup[m.MemberId]))
                .ToList(),
            project.Teams
                .Where(t => teamLookup.ContainsKey(t.TeamId))
                .Select(t => ProjectTeamMapper.ToResponse(t, teamLookup[t.TeamId]))
                .ToList(),
            project.Milestones.Select(ProjectMilestoneMapper.ToResponse).ToList());
    }
}

public sealed record ProjectTagResponse(
    Guid TagId,
    string Name,
    string Slug,
    string? Color);

public sealed record ProjectResponse(
    Guid ProjectId,
    string Name,
    string Slug,
    string? Description,
    DateTime StartDate,
    DateTime? EndDate,
    StatusResponse Status,
    PriorityResponse Priority,
    int Budget,
    bool IsArchived,
    DateTime DateCreated,
    List<ProjectTagResponse> Tags,
    List<ProjectMemberResponse> Members,
    List<ProjectTeamResponse> Teams,
    List<ProjectMilestoneResponse> Milestones);
