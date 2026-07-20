using ErrorOr;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;

namespace Mirama.Modules.PM.Application.Features.V1.Projects.Workflow.TaskPriorities.RemoveTaskPriority;

public sealed record RemoveTaskPriorityCommand(
    Guid ProjectId,
    Guid PriorityId) : ICommand<ErrorOr<Deleted>>;
