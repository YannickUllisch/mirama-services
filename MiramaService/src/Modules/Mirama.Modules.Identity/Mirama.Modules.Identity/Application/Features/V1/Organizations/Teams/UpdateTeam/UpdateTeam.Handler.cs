using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.Identity.Domain.Aggregates.Organization.Team;
using Mirama.Modules.Identity.Infrastructure.Persistence;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Abstractions.Persistence;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.Identity.Application.Features.V1.Organizations.Teams.UpdateTeam;

public class UpdateTeamController : OrganizationControllerBase
{
    [HttpPut("teams/{teamId:guid}")]
    public async Task<ActionResult<TeamResponse>> Update([FromRoute] Guid teamId, [FromBody] UpdateTeamCommand command)
    {
        var result = await this.Dispatcher.Send(command with { TeamId = teamId });
        return result.Match(Ok, Problem);
    }
}

internal class UpdateTeamCommandHandler(
    IdentityDbContext dbContext,
    IRequestContextProvider contextProvider) : IRequestHandler<UpdateTeamCommand, ErrorOr<TeamResponse>>
{
    public async Task<ErrorOr<TeamResponse>> HandleAsync(UpdateTeamCommand request, CancellationToken ct)
    {
        var organizationId = contextProvider.OrganizationId;
        if (organizationId is null)
            return Error.Unauthorized("Team.NoOrg", "Organization context required.");

        var team = await dbContext.Teams
            .Include(t => t.Members)
            .FirstOrDefaultAsync(t => t.Id == new TeamId(request.TeamId) && t.OrganizationId == organizationId.Value, ct);

        if (team is null)
            return Error.NotFound("Team.NotFound", "Team not found.");

        team.Update(request.Name);

        return team.MapResponse();
    }
}
