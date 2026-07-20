using ErrorOr;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;

namespace Mirama.Modules.PM.Application.Features.V1.Projects.Workflow.Priorities.UpdateProjectPriority;

public sealed record UpdateProjectPriorityCommand(
    Guid ProjectId,
    Guid PriorityId,
    string Name,
    int Level,
    string? Color = null,
    string? Icon = null) : ICommand<ErrorOr<PriorityResponse>>;
