using System.Text.Json.Serialization;
using Mirama.Modules.Identity.Application.Features.V1.Billing;
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

public sealed record TenantResponse
{
    [JsonPropertyName("id")]
    public Guid Id { get; init; }

    [JsonPropertyName("adminUserId")]
    public Guid AdminUserId { get; init; }

    [JsonPropertyName("settings")]
    public TenantSettingsResponse Settings { get; init; } = default!;

    [JsonPropertyName("subscription")]
    public SubscriptionResponse Subscription { get; init; } = default!;
}

public sealed record TenantSettingsResponse
{
    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;

    [JsonPropertyName("isActive")]
    public bool IsActive { get; init; }

    [JsonPropertyName("timezone")]
    public string Timezone { get; init; } = string.Empty;

    [JsonPropertyName("brandingColor")]
    public string? BrandingColor { get; init; }

    [JsonPropertyName("logoUrl")]
    public string? LogoUrl { get; init; }

    [JsonPropertyName("receiveNotifications")]
    public bool ReceiveNotifications { get; init; }
}

public sealed record SubscriptionResponse
{
    [JsonPropertyName("status")]
    public string Status { get; init; } = string.Empty;

    [JsonPropertyName("periodStart")]
    public DateTime PeriodStart { get; init; }

    [JsonPropertyName("periodEnd")]
    public DateTime PeriodEnd { get; init; }

    [JsonPropertyName("cancelAtPeriodEnd")]
    public bool CancelAtPeriodEnd { get; init; }

    [JsonPropertyName("plan")]
    public PlanResponse Plan { get; init; } = default!;
}
