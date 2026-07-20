using ErrorOr;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;

namespace Mirama.Modules.PM.Application.Features.V1.Projects.Workflow.Priorities.AddProjectPriority;

public sealed record AddProjectPriorityCommand(
    Guid ProjectId,
    string Name,
    int Level,
    string? Color = null,
    string? Icon = null,
    bool IsDefault = false) : ICommand<ErrorOr<PriorityResponse>>;
