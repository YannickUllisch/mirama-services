using ErrorOr;
using Mirama.Modules.PM.Application.Features.V1.Projects.Teams;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.PM.Application.Features.V1.Projects.Teams.GetProjectTeams;

public sealed record GetProjectTeamsQuery(Guid ProjectId, int? PageNumber, int? PageSize)
    : IQuery<ErrorOr<PaginatedList<ProjectTeamResponse>>>;
