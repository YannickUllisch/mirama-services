using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.Identity.Infrastructure.Persistence;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Abstractions.Persistence;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.Identity.Application.Features.V1.Billing;

public class GetBillingUsageController : TenantControllerBase
{
    [HttpGet("billing/usage")]
    public async Task<ActionResult<BillingUsageResponse>> Get()
    {
        var res = await this.Dispatcher.Send(new GetBillingUsageQuery());
        return res.Match(Ok, Problem);
    }
}

public sealed record GetBillingUsageQuery : IQuery<ErrorOr<BillingUsageResponse>>;

internal class GetBillingUsageQueryHandler(
    IdentityDbContext dbContext,
    IRequestContextProvider contextProvider) : IRequestHandler<GetBillingUsageQuery, ErrorOr<BillingUsageResponse>>
{
    public async Task<ErrorOr<BillingUsageResponse>> HandleAsync(GetBillingUsageQuery request, CancellationToken ct)
    {
        var tenantId = contextProvider.TenantId;
        if (tenantId is null)
            return Error.Unauthorized("Tenant.NoContext", "Tenant context required.");

        var organizations = await dbContext.Organizations
            .AsNoTracking()
            .CountAsync(ct);

        var members = await dbContext.Members
            .AsNoTracking()
            .CountAsync(ct);

        return new BillingUsageResponse
        {
            Organizations = organizations,
            Members = members,
            Projects = 0,
        };
    }
}
