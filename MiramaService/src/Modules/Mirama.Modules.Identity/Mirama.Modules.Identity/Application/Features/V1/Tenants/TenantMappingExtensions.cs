using Mirama.Modules.Identity.Domain.Aggregates.Plan;
using Mirama.Modules.Identity.Domain.Aggregates.Tenant;

namespace Mirama.Modules.Identity.Application.Features.V1.Tenants;

internal static class TenantMapper
{
    internal static TenantResponse MapResponse(this Tenant tenant, Plan plan) => new()
    {
        Id = tenant.Id,
        AdminUserId = tenant.AdminUserId.Value,
        Settings = new TenantSettingsResponse
        {
            Name = tenant.Settings.Name,
            IsActive = tenant.Settings.IsActive,
            Timezone = tenant.Settings.Timezone,
            BrandingColor = tenant.Settings.BrandingColor,
            LogoUrl = tenant.Settings.LogoUrl,
            ReceiveNotifications = tenant.Settings.ReceiveNotifications,
        },
        Subscription = new SubscriptionResponse
        {
            Status = tenant.Subscription.Status.ToString(),
            PeriodStart = tenant.Subscription.PeriodStart,
            PeriodEnd = tenant.Subscription.PeriodEnd,
            CancelAtPeriodEnd = tenant.Subscription.CancelAtPeriodEnd,
            Plan = new PlanResponse
            {
                Id = plan.Id.Value,
                Name = plan.Name,
                Description = plan.Description,
                Price = plan.Price,
                Interval = plan.Interval,
                Features = [.. plan.Features],
            },
        },
    };
}
