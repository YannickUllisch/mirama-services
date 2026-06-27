using Mirama.Modules.Identity.Contracts.Organizations;
using Mirama.Modules.PM.Domain.Aggregates.Project.Team;

namespace Mirama.Modules.PM.Application.Features.V1.Projects.Teams;

public sealed record ProjectTeamResponse(
    Guid ProjectTeamId,
    Guid TeamId,
    string Name,
    string Slug,
    IReadOnlyList<Guid> MemberIds,
    DateTime DateAdded);

internal static class ProjectTeamMapper
{
    internal static ProjectTeamResponse ToResponse(ProjectTeam projectTeam, TeamDto team) =>
        new(projectTeam.Id.Value, projectTeam.TeamId, team.Name, team.Slug, team.MemberIds, projectTeam.DateAdded);
}
