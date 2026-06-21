using Mirama.Modules.PM.Domain.Aggregates.Project;

namespace Mirama.Modules.PM.Domain.Aggregates.Task;

public sealed record TaskDetails(
    string TaskCode,
    string Title,
    TaskType Type,
    ProjectId ProjectId,
    Guid StatusId,
    Guid PriorityId,
    string? Description = null,
    DateTime? StartDate = null,
    DateTime? DueDate = null,
    int? EstimatedHours = null,
    Guid? AssignedToMemberId = null,
    TaskId? ParentTaskId = null);
