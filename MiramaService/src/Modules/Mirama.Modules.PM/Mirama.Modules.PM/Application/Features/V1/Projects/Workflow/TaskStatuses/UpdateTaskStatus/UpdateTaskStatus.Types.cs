using ErrorOr;
using Mirama.Modules.PM.Application.Features.V1.Projects.Workflow.Statuses;
using Mirama.Modules.PM.Domain.Aggregates.WorkflowConfig.Status;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;

namespace Mirama.Modules.PM.Application.Features.V1.Projects.Workflow.TaskStatuses.UpdateTaskStatus;

public sealed record UpdateTaskStatusCommand(
    Guid ProjectId,
    Guid StatusId,
    string Name,
    StatusCategory Category,
    string? Color = null,
    bool IsTerminal = false) : ICommand<ErrorOr<StatusResponse>>;
