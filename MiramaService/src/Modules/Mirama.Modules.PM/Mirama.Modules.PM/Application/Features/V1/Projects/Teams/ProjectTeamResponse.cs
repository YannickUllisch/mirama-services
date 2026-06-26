using Mirama.Modules.PM.Domain.Aggregates.Project.Team;

namespace Mirama.Modules.PM.Application.Features.V1.Projects.Teams;

public sealed record ProjectTeamResponse(
    Guid ProjectTeamId,
    Guid TeamId,
    DateTime DateAdded);

internal static class ProjectTeamMapper
{
    internal static ProjectTeamResponse ToResponse(ProjectTeam team) =>
        new(team.Id.Value, team.TeamId, team.DateAdded);
}
