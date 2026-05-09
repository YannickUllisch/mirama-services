using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.Identity.Domain.Aggregates.Organization.Team;
using Mirama.Modules.Identity.Infrastructure.Persistence;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Abstractions.Persistence;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.Identity.Application.Features.V1.Organizations.Teams.DeleteTeam;

public class DeleteTeamController : OrganizationControllerBase
{
    [HttpDelete("teams/{teamId:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid teamId)
    {
        var result = await this.Dispatcher.Send(new DeleteTeamCommand(teamId));
        return result.Match(_ => NoContent(), Problem);
    }
}

public sealed record DeleteTeamCommand(Guid TeamId) : ICommand<ErrorOr<Deleted>>;

internal class DeleteTeamCommandHandler(
    IdentityDbContext dbContext,
    IRequestContextProvider contextProvider) : IRequestHandler<DeleteTeamCommand, ErrorOr<Deleted>>
{
    public async Task<ErrorOr<Deleted>> HandleAsync(DeleteTeamCommand request, CancellationToken ct)
    {
        var organizationId = contextProvider.OrganizationId;
        if (organizationId is null)
            return Error.Unauthorized("Team.NoOrg", "Organization context required.");

        var team = await dbContext.Teams
            .FirstOrDefaultAsync(t => t.Id == new TeamId(request.TeamId) && t.OrganizationId == organizationId.Value, ct);

        if (team is null)
            return Error.NotFound("Team.NotFound", "Team not found.");

        dbContext.Teams.Remove(team);
        return Result.Deleted;
    }
}
