using ErrorOr;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;

namespace Mirama.Modules.PM.Application.Features.V1.Projects.Milestones.DeleteProjectMilestone;

public sealed record DeleteProjectMilestoneCommand(Guid ProjectId, Guid MilestoneId)
    : ICommand<ErrorOr<Deleted>>;
