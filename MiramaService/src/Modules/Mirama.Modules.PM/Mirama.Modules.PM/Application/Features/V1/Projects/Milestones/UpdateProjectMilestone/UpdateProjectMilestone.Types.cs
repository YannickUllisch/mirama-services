using ErrorOr;
using Mirama.Modules.PM.Application.Features.V1.Projects.Milestones;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;

namespace Mirama.Modules.PM.Application.Features.V1.Projects.Milestones.UpdateProjectMilestone;

public sealed record UpdateProjectMilestoneCommand(
    Guid ProjectId,
    Guid MilestoneId,
    string Title,
    DateTime DueDate,
    string? Description,
    string? Color) : ICommand<ErrorOr<ProjectMilestoneResponse>>;
