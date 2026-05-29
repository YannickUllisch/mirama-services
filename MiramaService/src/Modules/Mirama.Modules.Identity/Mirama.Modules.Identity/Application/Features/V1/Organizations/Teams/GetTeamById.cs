using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.Identity.Domain.Aggregates.Organization.Team;
using Mirama.Modules.Identity.Infrastructure.Persistence;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Abstractions.Persistence;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.Identity.Application.Features.V1.Organizations.Teams;

public class GetTeamByIdController : OrganizationControllerBase
{
    [HttpGet("teams/{teamId:guid}")]
    public async Task<ActionResult<TeamResponse>> Get([FromRoute] Guid teamId)
    {
        var res = await this.Dispatcher.Send(new GetTeamByIdQuery(teamId));
        return res.Match(Ok, Problem);
    }
}

public sealed record GetTeamByIdQuery(Guid TeamId) : IQuery<ErrorOr<TeamResponse>>;

internal class GetTeamByIdQueryHandler(
    IdentityDbContext dbContext,
    IRequestContextProvider contextProvider) : IRequestHandler<GetTeamByIdQuery, ErrorOr<TeamResponse>>
{
    public async Task<ErrorOr<TeamResponse>> HandleAsync(GetTeamByIdQuery request, CancellationToken ct)
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

        return team.MapResponse();
    }
}
