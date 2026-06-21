using Mirama.SharedKernel.Abstractions.Domain.Core;

namespace Mirama.Modules.PM.Domain.Aggregates.Project.Team;

public sealed class ProjectTeam : OrganizationEntity<ProjectTeamId>
{
    public Guid TeamId { get; private set; }
    public DateTime DateAdded { get; private set; }

    private ProjectTeam(Guid teamId)
    {
        this.TeamId = teamId;
        this.DateAdded = DateTime.UtcNow;
    }

    private ProjectTeam() { }

    internal static ProjectTeam Create(Guid teamId) =>
        new ProjectTeam(teamId) { Id = new ProjectTeamId(Guid.NewGuid()) };
}
