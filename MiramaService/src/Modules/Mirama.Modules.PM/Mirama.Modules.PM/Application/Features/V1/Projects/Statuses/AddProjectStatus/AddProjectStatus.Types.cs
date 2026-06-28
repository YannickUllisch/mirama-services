using ErrorOr;
using Mirama.Modules.PM.Domain.Aggregates.WorkflowConfig.Status;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;

namespace Mirama.Modules.PM.Application.Features.V1.Projects.Statuses.AddProjectStatus;

public sealed record AddProjectStatusCommand(
    Guid ProjectId,
    string Name,
    StatusCategory Category,
    string? Color = null,
    bool IsDefault = false,
    bool IsTerminal = false) : ICommand<ErrorOr<StatusResponse>>;
