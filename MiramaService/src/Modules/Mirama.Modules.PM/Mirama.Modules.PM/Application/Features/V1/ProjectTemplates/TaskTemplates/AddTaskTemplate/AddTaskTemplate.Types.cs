using ErrorOr;
using Mirama.Modules.PM.Domain.Aggregates.Task;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;

namespace Mirama.Modules.PM.Application.Features.V1.ProjectTemplates.TaskTemplates.AddTaskTemplate;

public sealed record AddTaskTemplateCommand(
    Guid ProjectTemplateId,
    string Title,
    TaskType Type,
    string? Description,
    int? EstimatedHours,
    Guid? ParentTemplateTaskId) : ICommand<ErrorOr<TaskTemplateResponse>>;
