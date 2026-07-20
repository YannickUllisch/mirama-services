using System.Text.Json.Serialization;
using Mirama.Modules.Identity.Contracts.Organizations;
using Mirama.Modules.Identity.Contracts.Tags;
using Mirama.Modules.PM.Application.Features.V1.Projects.Members;
using Mirama.Modules.PM.Application.Features.V1.Projects.Milestones;
using Mirama.Modules.PM.Application.Features.V1.Projects.Workflow.Priorities;
using Mirama.Modules.PM.Application.Features.V1.Projects.Workflow.Statuses;
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

        return new ProjectResponse
        {
            Id = project.Id.Value,
            Name = project.Name,
            Slug = project.Slug,
            Description = project.Description,
            StartDate = project.StartDate,
            EndDate = project.EndDate,
            Status = status is not null
                ? StatusMapper.ToResponse(status)
                : new StatusResponse { Id = project.StatusId, Name = "Unknown", Color = null, Category = "Unknown", Position = 0, IsDefault = false, IsTerminal = false },
            Priority = priority is not null
                ? PriorityMapper.ToResponse(priority)
                : new PriorityResponse { Id = project.PriorityId, Name = "Unknown", Color = null, Icon = null, Level = 0, IsDefault = false },
            Budget = project.Budget,
            IsArchived = project.IsArchived,
            DateCreated = project.DateCreated,
            Tags = project.TagIds
                .Where(tagLookup.ContainsKey)
                .Select(id => { var t = tagLookup[id]; return new ProjectTagResponse { Id = t.Id, Name = t.Name, Slug = t.Slug, Color = t.Color }; })
                .ToList(),
            Members = project.Members
                .Where(m => memberLookup.ContainsKey(m.MemberId))
                .Select(m => ProjectMemberMapper.ToResponse(m, memberLookup[m.MemberId]))
                .ToList(),
            Teams = project.Teams
                .Where(t => teamLookup.ContainsKey(t.TeamId))
                .Select(t => ProjectTeamMapper.ToResponse(t, teamLookup[t.TeamId]))
                .ToList(),
            Milestones = project.Milestones.Select(ProjectMilestoneMapper.ToResponse).ToList()
        };
    }
}

public sealed record ProjectTagResponse
{
    [JsonPropertyName("id")]
    public Guid Id { get; init; }

    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;

    [JsonPropertyName("slug")]
    public string Slug { get; init; } = string.Empty;

    [JsonPropertyName("color")]
    public string? Color { get; init; }
}

public sealed record ProjectResponse
{
    [JsonPropertyName("id")]
    public Guid Id { get; init; }

    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;

    [JsonPropertyName("slug")]
    public string Slug { get; init; } = string.Empty;

    [JsonPropertyName("description")]
    public string? Description { get; init; }

    [JsonPropertyName("startDate")]
    public DateTime StartDate { get; init; }

    [JsonPropertyName("endDate")]
    public DateTime? EndDate { get; init; }

    [JsonPropertyName("status")]
    public StatusResponse Status { get; init; } = null!;

    [JsonPropertyName("priority")]
    public PriorityResponse Priority { get; init; } = null!;

    [JsonPropertyName("budget")]
    public int Budget { get; init; }

    [JsonPropertyName("isArchived")]
    public bool IsArchived { get; init; }

    [JsonPropertyName("dateCreated")]
    public DateTime DateCreated { get; init; }

    [JsonPropertyName("tags")]
    public List<ProjectTagResponse> Tags { get; init; } = [];

    [JsonPropertyName("members")]
    public List<ProjectMemberResponse> Members { get; init; } = [];

    [JsonPropertyName("teams")]
    public List<ProjectTeamResponse> Teams { get; init; } = [];

    [JsonPropertyName("milestones")]
    public List<ProjectMilestoneResponse> Milestones { get; init; } = [];
}
