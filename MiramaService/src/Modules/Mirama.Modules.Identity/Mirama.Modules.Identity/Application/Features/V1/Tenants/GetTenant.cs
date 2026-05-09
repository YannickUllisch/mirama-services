using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.Identity.Infrastructure.Persistence;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Abstractions.Persistence;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.Identity.Application.Features.V1.Tenants;

public class GetTenantController : TenantControllerBase
{
    [HttpGet("")]
    public async Task<ActionResult<TenantResponse>> Get()
    {
        var res = await this.Dispatcher.Send(new GetTenantQuery());
        return res.Match(Ok, Problem);
    }
}

public sealed record GetTenantQuery : IQuery<ErrorOr<TenantResponse>>;

internal class GetTenantQueryHandler(
    IdentityDbContext dbContext,
    IRequestContextProvider contextProvider) : IRequestHandler<GetTenantQuery, ErrorOr<TenantResponse>>
{
    public async Task<ErrorOr<TenantResponse>> HandleAsync(GetTenantQuery request, CancellationToken ct)
    {
        var tenantId = contextProvider.TenantId;
        if (tenantId is null)
            return Error.Unauthorized("Tenant.NoContext", "Tenant context required.");

        var tenant = await dbContext.Tenants
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == tenantId.Value, ct);

        if (tenant is null)
            return Error.NotFound("Tenant.NotFound", "Tenant not found.");

        var plan = await dbContext.Plans
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == tenant.Subscription.PlanId, ct);

        if (plan is null)
            return Error.NotFound("Tenant.Plan.NotFound", "Subscription plan not found.");

        return tenant.MapResponse(plan);
    }
}
