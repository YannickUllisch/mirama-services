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
        Name = tenant.Name,
        IsActive = tenant.IsActive,
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

    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;

    [JsonPropertyName("isActive")]
    public bool IsActive { get; init; }

    [JsonPropertyName("subscription")]
    public SubscriptionResponse Subscription { get; init; } = default!;
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
