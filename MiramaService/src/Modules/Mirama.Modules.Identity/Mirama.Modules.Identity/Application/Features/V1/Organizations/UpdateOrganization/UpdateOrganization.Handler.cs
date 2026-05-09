using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.Identity.Domain.Aggregates.Organization;
using Mirama.Modules.Identity.Infrastructure.Persistence;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Abstractions.Persistence;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.Identity.Application.Features.V1.Organizations.UpdateOrganization;

public class UpdateOrganizationController : TenantControllerBase
{
    [HttpPut("organizations/{id:guid}")]
    public async Task<ActionResult<OrganizationResponse>> Update([FromRoute] Guid id, [FromBody] UpdateOrganizationCommand command)
    {
        var result = await this.Dispatcher.Send(command with { OrganizationId = id });
        return result.Match(Ok, Problem);
    }
}

internal class UpdateOrganizationCommandHandler(
    IdentityDbContext dbContext,
    IRequestContextProvider contextProvider) : IRequestHandler<UpdateOrganizationCommand, ErrorOr<OrganizationResponse>>
{
    public async Task<ErrorOr<OrganizationResponse>> HandleAsync(UpdateOrganizationCommand request, CancellationToken ct)
    {
        var tenantId = contextProvider.TenantId;
        if (tenantId is null)
            return Error.Unauthorized("Organization.NoTenant", "Tenant context required.");

        var org = await dbContext.Organizations
            .FirstOrDefaultAsync(o => o.Id == new OrganizationId(request.OrganizationId), ct);

        if (org is null)
            return Error.NotFound("Organization.NotFound", "Organization not found.");

        var details = new OrganizationDetails(request.Name, request.Street, request.City, request.Country, request.ZipCode, request.Logo);
        org.Update(details);

        return org.MapResponse();
    }
}
