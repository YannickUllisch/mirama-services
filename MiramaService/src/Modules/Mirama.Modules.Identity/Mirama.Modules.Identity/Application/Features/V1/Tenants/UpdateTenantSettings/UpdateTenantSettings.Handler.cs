using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.Identity.Domain.Aggregates.Tenant;
using Mirama.Modules.Identity.Infrastructure.Persistence;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Abstractions.Persistence;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.Identity.Application.Features.V1.Tenants.UpdateTenantSettings;

public class UpdateTenantSettingsController : TenantControllerBase
{
    [HttpPut("settings")]
    public async Task<ActionResult<TenantResponse>> Update([FromBody] UpdateTenantSettingsCommand command)
    {
        var result = await this.Dispatcher.Send(command);
        return result.Match(Ok, Problem);
    }
}

internal class UpdateTenantSettingsCommandHandler(
    IdentityDbContext dbContext,
    IRequestContextProvider contextProvider) : IRequestHandler<UpdateTenantSettingsCommand, ErrorOr<TenantResponse>>
{
    public async Task<ErrorOr<TenantResponse>> HandleAsync(UpdateTenantSettingsCommand request, CancellationToken ct)
    {
        var tenantId = contextProvider.TenantId;
        if (tenantId is null)
            return Error.Unauthorized("Tenant.NoContext", "Tenant context required.");

        var tenant = await dbContext.Tenants
            .FirstOrDefaultAsync(t => t.Id == tenantId.Value, ct);

        if (tenant is null)
            return Error.NotFound("Tenant.NotFound", "Tenant not found.");

        tenant.UpdateSettings(new TenantSettingsDetails(
            request.Name,
            request.ReceiveNotifications,
            request.BrandingColor,
            request.LogoUrl,
            request.Timezone));

        var plan = await dbContext.Plans
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == tenant.Subscription.PlanId, ct);

        if (plan is null)
            return Error.NotFound("Tenant.Plan.NotFound", "Subscription plan not found.");

        return tenant.MapResponse(plan);
    }
}
