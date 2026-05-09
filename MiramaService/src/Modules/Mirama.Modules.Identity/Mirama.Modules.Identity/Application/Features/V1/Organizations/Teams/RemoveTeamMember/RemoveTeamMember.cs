using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.Identity.Domain.Aggregates.Organization.Member;
using Mirama.Modules.Identity.Domain.Aggregates.Organization.Team;
using Mirama.Modules.Identity.Infrastructure.Persistence;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Abstractions.Persistence;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.Identity.Application.Features.V1.Organizations.Teams.RemoveTeamMember;

public class RemoveTeamMemberController : OrganizationControllerBase
{
    [HttpDelete("teams/{teamId:guid}/members/{memberId:guid}")]
    public async Task<IActionResult> Remove([FromRoute] Guid teamId, [FromRoute] Guid memberId)
    {
        var result = await this.Dispatcher.Send(new RemoveTeamMemberCommand(teamId, memberId));
        return result.Match(_ => NoContent(), Problem);
    }
}

public sealed record RemoveTeamMemberCommand(Guid TeamId, Guid MemberId) : ICommand<ErrorOr<Deleted>>;

internal class RemoveTeamMemberCommandHandler(
    IdentityDbContext dbContext,
    IRequestContextProvider contextProvider) : IRequestHandler<RemoveTeamMemberCommand, ErrorOr<Deleted>>
{
    public async Task<ErrorOr<Deleted>> HandleAsync(RemoveTeamMemberCommand request, CancellationToken ct)
    {
        var organizationId = contextProvider.OrganizationId;
        if (organizationId is null)
            return Error.Unauthorized("Team.NoOrg", "Organization context required.");

        var team = await dbContext.Teams
            .Include(t => t.Members)
            .FirstOrDefaultAsync(t => t.Id == new TeamId(request.TeamId) && t.OrganizationId == organizationId.Value, ct);

        if (team is null)
            return Error.NotFound("Team.NotFound", "Team not found.");

        return team.RemoveMember(new MemberId(request.MemberId));
    }
}
