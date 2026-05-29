using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.Identity.Infrastructure.Persistence;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.Identity.Application.Features.V1.Organizations;

public class GetOrganizationsController : TenantControllerBase
{
    [HttpGet("organizations")]
    public async Task<ActionResult<List<OrganizationResponse>>> Get()
    {
        var res = await this.Dispatcher.Send(new GetOrganizationsQuery());
        return res.Match(Ok, Problem);
    }
}

public sealed record GetOrganizationsQuery : IQuery<ErrorOr<List<OrganizationResponse>>>;

internal class GetOrganizationsQueryHandler(
    IdentityDbContext dbContext) : IRequestHandler<GetOrganizationsQuery, ErrorOr<List<OrganizationResponse>>>
{
    public async Task<ErrorOr<List<OrganizationResponse>>> HandleAsync(GetOrganizationsQuery request, CancellationToken ct)
    {
        var orgs = await dbContext.Organizations
            .AsNoTracking()
            .OrderBy(o => o.DateCreated)
            .ToListAsync(ct);

        return orgs.ConvertAll(o => o.MapResponse());
    }
}
