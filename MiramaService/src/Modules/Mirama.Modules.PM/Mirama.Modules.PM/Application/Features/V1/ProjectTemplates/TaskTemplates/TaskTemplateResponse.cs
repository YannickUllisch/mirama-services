using Mirama.Modules.PM.Domain.Aggregates.ProjectTemplate.TaskTemplate;

namespace Mirama.Modules.PM.Application.Features.V1.ProjectTemplates.TaskTemplates;

public sealed record TaskTemplateResponse(
    Guid TaskTemplateId,
    string Title,
    string? Description,
    string Type,
    int? EstimatedHours,
    Guid? ParentTemplateTaskId,
    int Position);

internal static class TaskTemplateMapper
{
    internal static TaskTemplateResponse ToResponse(TaskTemplate task) =>
        new(task.Id.Value, task.Title, task.Description, task.Type.ToString(), task.EstimatedHours, task.ParentTemplateTaskId?.Value, task.Position);
}
