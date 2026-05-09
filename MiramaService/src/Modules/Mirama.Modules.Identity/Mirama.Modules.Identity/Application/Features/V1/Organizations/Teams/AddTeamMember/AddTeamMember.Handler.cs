using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.Identity.Domain.Aggregates.Organization.Member;
using Mirama.Modules.Identity.Domain.Aggregates.Organization.Team;
using Mirama.Modules.Identity.Infrastructure.Persistence;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Abstractions.Persistence;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.Identity.Application.Features.V1.Organizations.Teams.AddTeamMember;

public class AddTeamMemberController : OrganizationControllerBase
{
    [HttpPost("teams/{teamId:guid}/members")]
    public async Task<ActionResult<TeamResponse>> Add([FromRoute] Guid teamId, [FromBody] AddTeamMemberCommand command)
    {
        var result = await this.Dispatcher.Send(command with { TeamId = teamId });
        return result.Match(Ok, Problem);
    }
}

internal class AddTeamMemberCommandHandler(
    IdentityDbContext dbContext,
    IRequestContextProvider contextProvider) : IRequestHandler<AddTeamMemberCommand, ErrorOr<TeamResponse>>
{
    public async Task<ErrorOr<TeamResponse>> HandleAsync(AddTeamMemberCommand request, CancellationToken ct)
    {
        var organizationId = contextProvider.OrganizationId;
        if (organizationId is null)
            return Error.Unauthorized("Team.NoOrg", "Organization context required.");

        var team = await dbContext.Teams
            .Include(t => t.Members)
            .FirstOrDefaultAsync(t => t.Id == new TeamId(request.TeamId) && t.OrganizationId == organizationId.Value, ct);

        if (team is null)
            return Error.NotFound("Team.NotFound", "Team not found.");

        var memberExists = await dbContext.Members
            .AsNoTracking()
            .AnyAsync(m => m.Id == new MemberId(request.MemberId), ct);

        if (!memberExists)
            return Error.NotFound("Team.Member.NotFound", "Member not found in this organization.");

        team.AddMember(new MemberId(request.MemberId));

        return team.MapResponse();
    }
}
