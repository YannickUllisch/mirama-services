using System.Text.Json.Serialization;
using Mirama.Modules.PM.Application.Features.V1.Projects.Workflow.Priorities;
using Mirama.Modules.PM.Application.Features.V1.Projects.Workflow.Statuses;
using Mirama.Modules.PM.Domain.Aggregates.WorkflowConfig;

namespace Mirama.Modules.PM.Application.Features.V1.Projects.Workflow;

internal static class WorkflowMapper
{
    internal static WorkflowResponse ToResponse(WorkflowConfig workflow) => new()
    {
        Id = workflow.Id.Value,
        ProjectId = workflow.ProjectId.Value,
        ProjectStatuses = workflow.Statuses.OrderBy(s => s.Position).Select(StatusMapper.ToResponse).ToList(),
        ProjectPriorities = workflow.Priorities.OrderBy(p => p.Level).Select(PriorityMapper.ToResponse).ToList(),
        TaskStatuses = workflow.TaskStatuses.OrderBy(s => s.Position).Select(StatusMapper.ToResponse).ToList(),
        TaskPriorities = workflow.TaskPriorities.OrderBy(p => p.Level).Select(PriorityMapper.ToResponse).ToList()
    };
}

public sealed record WorkflowResponse
{
    [JsonPropertyName("id")]
    public Guid Id { get; init; }

    [JsonPropertyName("projectId")]
    public Guid ProjectId { get; init; }

    [JsonPropertyName("projectStatuses")]
    public List<StatusResponse> ProjectStatuses { get; init; } = [];

    [JsonPropertyName("projectPriorities")]
    public List<PriorityResponse> ProjectPriorities { get; init; } = [];

    [JsonPropertyName("taskStatuses")]
    public List<StatusResponse> TaskStatuses { get; init; } = [];

    [JsonPropertyName("taskPriorities")]
    public List<PriorityResponse> TaskPriorities { get; init; } = [];
}
