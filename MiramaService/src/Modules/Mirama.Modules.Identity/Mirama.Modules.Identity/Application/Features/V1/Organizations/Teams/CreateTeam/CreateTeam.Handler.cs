using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.Identity.Domain.Aggregates.Organization.Team;
using Mirama.Modules.Identity.Infrastructure.Persistence;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Abstractions.Persistence;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.Identity.Application.Features.V1.Organizations.Teams.CreateTeam;

public class CreateTeamController : OrganizationControllerBase
{
    [HttpPost("teams")]
    public async Task<ActionResult<TeamResponse>> Create([FromBody] CreateTeamCommand command)
    {
        var result = await this.Dispatcher.Send(command);
        return result.Match(r => CreatedAtAction(nameof(Create), new { teamId = r.Id }, r), Problem);
    }
}

internal class CreateTeamCommandHandler(
    IdentityDbContext dbContext,
    IRequestContextProvider contextProvider) : IRequestHandler<CreateTeamCommand, ErrorOr<TeamResponse>>
{
    public async Task<ErrorOr<TeamResponse>> HandleAsync(CreateTeamCommand request, CancellationToken ct)
    {
        var organizationId = contextProvider.OrganizationId;
        if (organizationId is null)
            return Error.Unauthorized("Team.NoOrg", "Organization context required.");

        var duplicate = await dbContext.Teams
            .AsNoTracking()
            .AnyAsync(t => t.OrganizationId == organizationId.Value && t.Name == request.Name.Trim(), ct);

        if (duplicate)
            return Error.Conflict("Team.Duplicate", "A team with this name already exists in the organization.");

        var team = Team.Create(new TeamDetails(request.Name, organizationId.Value));
        dbContext.Teams.Add(team);

        return team.MapResponse();
    }
}
