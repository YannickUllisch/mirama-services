using ErrorOr;
using Mirama.Modules.PM.Application.Features.V1.Projects.Workflow.Priorities;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;

namespace Mirama.Modules.PM.Application.Features.V1.Projects.Workflow.TaskPriorities.UpdateTaskPriority;

public sealed record UpdateTaskPriorityCommand(
    Guid ProjectId,
    Guid PriorityId,
    string Name,
    int Level,
    string? Color = null,
    string? Icon = null) : ICommand<ErrorOr<PriorityResponse>>;
