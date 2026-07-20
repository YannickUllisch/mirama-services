using ErrorOr;
using Mirama.Modules.PM.Application.Features.V1.Projects.Workflow.Priorities;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;

namespace Mirama.Modules.PM.Application.Features.V1.Projects.Workflow.TaskPriorities.AddTaskPriority;

public sealed record AddTaskPriorityCommand(
    Guid ProjectId,
    string Name,
    int Level,
    string? Color = null,
    string? Icon = null,
    bool IsDefault = false) : ICommand<ErrorOr<PriorityResponse>>;
