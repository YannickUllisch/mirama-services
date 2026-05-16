using ErrorOr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.Identity.Domain.Aggregates.User;
using Mirama.Modules.Identity.Infrastructure.Persistence;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.Identity.Application.Features.V1.Auth;

[AllowAnonymous]
public class GetMeController : ApiControllerBase
{
    [HttpGet("auth/me/{externalId:guid}")]
    public async Task<ActionResult<SetupUserResponse>> Get([FromRoute] Guid externalId)
    {
        var res = await this.Dispatcher.Send(new GetMeQuery(externalId));
        return res.Match(Ok, Problem);
    }
}

public sealed record GetMeQuery(Guid ExternalId) : IQuery<ErrorOr<SetupUserResponse>>;

internal class GetMeQueryHandler(
    IdentityDbContext dbContext) : IRequestHandler<GetMeQuery, ErrorOr<SetupUserResponse>>
{
    public async Task<ErrorOr<SetupUserResponse>> HandleAsync(GetMeQuery request, CancellationToken ct)
    {
        var user = await dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == new UserId(request.ExternalId), ct);

        if (user is null)
            return Error.NotFound("User.NotFound", "User not found.");

        var tenant = await dbContext.Tenants
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.AdminUserId == user.Id, ct);

        if (tenant is null)
            return Error.NotFound("Tenant.NotFound", "Tenant not found.");

        var plan = await dbContext.Plans
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == tenant.Subscription.PlanId, ct);

        if (plan is null)
            return Error.NotFound("Plan.NotFound", "Subscription plan not found.");

        return new SetupUserResponse
        {
            UserId = user.Id.Value,
            Name = user.Name,
            Email = user.Email,
            Image = user.Image,
            Role = user.Role.ToString(),
            TenantId = tenant.Id,
            Settings = new SetupTenantSettingsResponse
            {
                Name = tenant.Settings.Name,
                IsActive = tenant.Settings.IsActive,
                Timezone = tenant.Settings.Timezone,
                ReceiveNotifications = tenant.Settings.ReceiveNotifications,
            },
            Subscription = new SetupSubscriptionResponse
            {
                Status = tenant.Subscription.Status.ToString(),
                PlanId = plan.Id.Value,
                PlanName = plan.Name,
                Features = [.. plan.Features],
                PeriodStart = tenant.Subscription.PeriodStart,
                PeriodEnd = tenant.Subscription.PeriodEnd,
            },
        };
    }
}
