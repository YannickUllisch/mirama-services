using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.Identity.Application.Features.V1.Organizations.Members;
using Mirama.Modules.Identity.Domain.Aggregates.Organization.Team;
using Mirama.Modules.Identity.Infrastructure.Persistence;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Abstractions.Persistence;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.Identity.Application.Features.V1.Organizations.Teams;

public class GetTeamMembersController : OrganizationControllerBase
{
    [HttpGet("teams/{teamId:guid}/members")]
    public async Task<ActionResult<List<MemberResponse>>> Get([FromRoute] Guid teamId)
    {
        var result = await this.Dispatcher.Send(new GetTeamMembersQuery(teamId));
        return result.Match(Ok, Problem);
    }
}

public sealed record GetTeamMembersQuery(Guid TeamId) : IQuery<ErrorOr<List<MemberResponse>>>;

internal class GetTeamMembersQueryHandler(
    IdentityDbContext dbContext,
    IRequestContextProvider contextProvider) : IRequestHandler<GetTeamMembersQuery, ErrorOr<List<MemberResponse>>>
{
    public async Task<ErrorOr<List<MemberResponse>>> HandleAsync(GetTeamMembersQuery request, CancellationToken ct)
    {
        var organizationId = contextProvider.OrganizationId;
        if (organizationId is null)
            return Error.Unauthorized("Team.NoOrg", "Organization context required.");

        var team = await dbContext.Teams
            .AsNoTracking()
            .Include(t => t.Members)
            .FirstOrDefaultAsync(t => t.Id == new TeamId(request.TeamId) && t.OrganizationId == organizationId.Value, ct);

        if (team is null)
            return Error.NotFound("Team.NotFound", "Team not found.");

        var memberIds = team.Members.Select(m => m.MemberId).ToList();

        var members = await dbContext.Members
            .AsNoTracking()
            .Where(m => memberIds.Contains(m.Id))
            .OrderBy(m => m.Name)
            .ToListAsync(ct);

        return members.ConvertAll(m => m.MapResponse());
    }
}
