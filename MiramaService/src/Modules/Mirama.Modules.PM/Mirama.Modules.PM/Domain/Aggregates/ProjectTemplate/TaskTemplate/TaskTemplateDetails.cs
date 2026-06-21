using Mirama.Modules.PM.Domain.Aggregates.Task;

namespace Mirama.Modules.PM.Domain.Aggregates.ProjectTemplate.TaskTemplate;

public sealed record TaskTemplateDetails(
    string Title,
    TaskType Type,
    string? Description = null,
    int? EstimatedHours = null,
    TaskTemplateId? ParentTemplateTaskId = null);
