using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Mirama.Modules.Identity.Domain.Aggregates.Organization;
using Mirama.Modules.Identity.Infrastructure.Persistence;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Abstractions.Persistence;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.Identity.Application.Features.V1.Organizations.CreateOrganization;

public class CreateOrganizationController : TenantControllerBase
{
    [HttpPost("organizations")]
    public async Task<ActionResult<OrganizationResponse>> Create([FromBody] CreateOrganizationCommand command)
    {
        var result = await this.Dispatcher.Send(command);
        return result.Match(r => CreatedAtAction(nameof(Create), new { id = r.Id }, r), Problem);
    }
}

internal class CreateOrganizationCommandHandler(
    IdentityDbContext dbContext,
    IRequestContextProvider contextProvider) : IRequestHandler<CreateOrganizationCommand, ErrorOr<OrganizationResponse>>
{
    public async Task<ErrorOr<OrganizationResponse>> HandleAsync(CreateOrganizationCommand request, CancellationToken ct)
    {
        var tenantId = contextProvider.TenantId;
        if (tenantId is null)
            return Error.Unauthorized("Organization.NoTenant", "Tenant context required.");

        var details = new OrganizationDetails(request.Name, request.Street, request.City, request.Country, request.ZipCode, request.Logo);
        var org = Organization.Create(details);

        dbContext.Organizations.Add(org);

        return org.MapResponse() with { TenantId = tenantId.Value };
    }
}
