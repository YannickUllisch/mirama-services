using ErrorOr;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;

namespace Mirama.Modules.PM.Application.Features.V1.Projects.Teams.RemoveProjectTeam;

public sealed record RemoveProjectTeamCommand(Guid ProjectId, Guid TeamId)
    : ICommand<ErrorOr<Deleted>>;
