using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.Identity.Infrastructure.Persistence;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Abstractions.Persistence;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.Identity.Application.Features.V1.Tenants.CancelSubscription;

public class CancelSubscriptionController : TenantControllerBase
{
    [HttpPost("subscription/cancel")]
    public async Task<ActionResult<TenantResponse>> Cancel()
    {
        var result = await this.Dispatcher.Send(new CancelSubscriptionCommand());
        return result.Match(Ok, Problem);
    }
}

public sealed record CancelSubscriptionCommand : ICommand<ErrorOr<TenantResponse>>;

internal class CancelSubscriptionCommandHandler(
    IdentityDbContext dbContext,
    IRequestContextProvider contextProvider) : IRequestHandler<CancelSubscriptionCommand, ErrorOr<TenantResponse>>
{
    public async Task<ErrorOr<TenantResponse>> HandleAsync(CancelSubscriptionCommand request, CancellationToken ct)
    {
        var tenantId = contextProvider.TenantId;
        if (tenantId is null)
            return Error.Unauthorized("Tenant.NoContext", "Tenant context required.");

        var tenant = await dbContext.Tenants
            .FirstOrDefaultAsync(t => t.Id == tenantId.Value, ct);

        if (tenant is null)
            return Error.NotFound("Tenant.NotFound", "Tenant not found.");

        if (tenant.Subscription.CancelAtPeriodEnd)
            return Error.Conflict("Tenant.Subscription.AlreadyCanceled", "Subscription is already scheduled for cancellation.");

        tenant.Subscription.ScheduleCancellation();

        var plan = await dbContext.Plans
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == tenant.Subscription.PlanId, ct);

        if (plan is null)
            return Error.NotFound("Tenant.Plan.NotFound", "Subscription plan not found.");

        return tenant.MapResponse(plan);
    }
}
