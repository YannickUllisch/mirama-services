using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.Identity.Infrastructure.Persistence;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Abstractions.Persistence;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.Identity.Application.Features.V1.Organizations.Teams;

public class GetTeamsController : OrganizationControllerBase
{
    [HttpGet("teams")]
    public async Task<ActionResult<List<TeamResponse>>> Get()
    {
        var res = await this.Dispatcher.Send(new GetTeamsQuery());
        return res.Match(Ok, Problem);
    }
}

public sealed record GetTeamsQuery : IQuery<ErrorOr<List<TeamResponse>>>;

internal class GetTeamsQueryHandler(
    IdentityDbContext dbContext,
    IRequestContextProvider contextProvider) : IRequestHandler<GetTeamsQuery, ErrorOr<List<TeamResponse>>>
{
    public async Task<ErrorOr<List<TeamResponse>>> HandleAsync(GetTeamsQuery request, CancellationToken ct)
    {
        var organizationId = contextProvider.OrganizationId;
        if (organizationId is null)
            return Error.Unauthorized("Team.NoOrg", "Organization context required.");

        var teams = await dbContext.Teams
            .AsNoTracking()
            .Include(t => t.Members)
            .Where(t => t.OrganizationId == organizationId.Value)
            .OrderBy(t => t.Name)
            .ToListAsync(ct);

        return teams.ConvertAll(t => t.MapResponse());
    }
}
