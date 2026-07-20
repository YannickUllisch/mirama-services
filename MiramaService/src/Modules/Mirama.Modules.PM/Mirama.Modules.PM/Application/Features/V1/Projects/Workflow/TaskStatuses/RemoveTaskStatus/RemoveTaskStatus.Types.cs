using ErrorOr;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;

namespace Mirama.Modules.PM.Application.Features.V1.Projects.Workflow.TaskStatuses.RemoveTaskStatus;

public sealed record RemoveTaskStatusCommand(
    Guid ProjectId,
    Guid StatusId) : ICommand<ErrorOr<Deleted>>;
