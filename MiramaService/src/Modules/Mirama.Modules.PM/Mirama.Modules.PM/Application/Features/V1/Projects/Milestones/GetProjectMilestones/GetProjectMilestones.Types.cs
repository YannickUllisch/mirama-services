using ErrorOr;
using Mirama.Modules.PM.Application.Features.V1.Projects.Milestones;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.PM.Application.Features.V1.Projects.Milestones.GetProjectMilestones;

public sealed record GetProjectMilestonesQuery(Guid ProjectId, int? PageNumber, int? PageSize)
    : IQuery<ErrorOr<PaginatedList<ProjectMilestoneResponse>>>;
