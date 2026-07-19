using Microsoft.EntityFrameworkCore;
using Mirama.Modules.Identity.Contracts.Organizations;
using Mirama.Modules.Identity.Domain.Aggregates.Organization.Team;
using Mirama.Modules.Identity.Infrastructure.Persistence;

namespace Mirama.Modules.Identity.Infrastructure.Services.Organizations;

internal sealed class TeamService(IdentityDbContext db) : ITeamService
{
    public async Task<IReadOnlyList<TeamDto>> GetTeamsByIdsAsync(
        IEnumerable<Guid> teamIds, CancellationToken ct = default)
    {
        var ids = teamIds.Select(id => new TeamId(id)).ToList();

        var teams = await db.Teams.AsNoTracking()
            .Include(t => t.Members)
            .Where(t => ids.Contains(t.Id))
            .ToListAsync(ct);

        return [..teams.Select(Map)];
    }

    public async Task<TeamDto?> GetTeamByIdAsync(Guid teamId, CancellationToken ct = default)
    {
        var team = await db.Teams.AsNoTracking()
            .Include(t => t.Members)
            .FirstOrDefaultAsync(t => t.Id == new TeamId(teamId), ct);

        return team is null ? null : Map(team);
    }

    private static TeamDto Map(Domain.Aggregates.Organization.Team.Team t) => new(
        t.Id.Value,
        t.OrganizationId,
        t.Name,
        t.Slug,
        [..t.Members.Select(m => m.MemberId.Value)]);
}
