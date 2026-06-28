using ErrorOr;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;

namespace Mirama.Modules.PM.Application.Features.V1.Projects.Priorities.RemoveProjectPriority;

public sealed record RemoveProjectPriorityCommand(
    Guid ProjectId,
    Guid PriorityId) : ICommand<ErrorOr<Deleted>>;
