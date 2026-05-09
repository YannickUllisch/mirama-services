using System.Text.Json.Serialization;

namespace Mirama.Modules.Identity.Application.Features.V1.Tenants;

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

public sealed record PlanResponse
{
    [JsonPropertyName("id")]
    public Guid Id { get; init; }

    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;

    [JsonPropertyName("description")]
    public string? Description { get; init; }

    [JsonPropertyName("price")]
    public int Price { get; init; }

    [JsonPropertyName("interval")]
    public string Interval { get; init; } = string.Empty;

    [JsonPropertyName("features")]
    public List<string> Features { get; init; } = [];
}
