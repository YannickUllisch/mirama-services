using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.Identity.Domain.Aggregates.Organization;
using Mirama.Modules.Identity.Infrastructure.Persistence;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.Identity.Application.Features.V1.Organizations;

public class GetOrganizationByIdController : TenantControllerBase
{
    [HttpGet("organizations/{id:guid}")]
    public async Task<ActionResult<OrganizationResponse>> Get([FromRoute] Guid id)
    {
        var res = await this.Dispatcher.Send(new GetOrganizationByIdQuery(id));
        return res.Match(Ok, Problem);
    }
}

public sealed record GetOrganizationByIdQuery(Guid Id) : IQuery<ErrorOr<OrganizationResponse>>;

internal class GetOrganizationByIdQueryHandler(
    IdentityDbContext dbContext) : IRequestHandler<GetOrganizationByIdQuery, ErrorOr<OrganizationResponse>>
{
    public async Task<ErrorOr<OrganizationResponse>> HandleAsync(GetOrganizationByIdQuery request, CancellationToken ct)
    {
        var org = await dbContext.Organizations
            .AsNoTracking()
            .FirstOrDefaultAsync(o => o.Id == new OrganizationId(request.Id), ct);

        if (org is null)
            return Error.NotFound("Organization.NotFound", "Organization not found.");

        return org.MapResponse();
    }
}
