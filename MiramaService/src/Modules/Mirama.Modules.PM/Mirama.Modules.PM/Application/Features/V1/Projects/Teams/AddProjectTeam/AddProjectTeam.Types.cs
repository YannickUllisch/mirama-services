using ErrorOr;
using Mirama.Modules.PM.Application.Features.V1.Projects.Teams;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;

namespace Mirama.Modules.PM.Application.Features.V1.Projects.Teams.AddProjectTeam;

public sealed record AddProjectTeamCommand(Guid ProjectId, Guid TeamId)
    : ICommand<ErrorOr<ProjectTeamResponse>>;
