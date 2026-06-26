using ErrorOr;
using Mirama.Modules.PM.Application.Features.V1.Projects.Milestones;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;

namespace Mirama.Modules.PM.Application.Features.V1.Projects.Milestones.CreateProjectMilestone;

public sealed record CreateProjectMilestoneCommand(
    Guid ProjectId,
    string Title,
    DateTime DueDate,
    string? Description,
    string? Color) : ICommand<ErrorOr<ProjectMilestoneResponse>>;
