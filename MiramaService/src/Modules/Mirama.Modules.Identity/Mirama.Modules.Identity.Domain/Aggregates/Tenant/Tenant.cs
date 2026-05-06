using Mirama.Modules.Identity.Domain.Aggregates.Subscription;
using Mirama.Modules.Identity.Domain.Aggregates.User;
using Mirama.SharedKernel.Abstractions.Domain.Core;

namespace Mirama.Modules.Identity.Domain.Aggregates.Tenant;

public sealed class Tenant : AggregateRoot<Guid>
{
    public UserId AdminUserId { get; private set; } = default!;
    public SubscriptionId? SubscriptionId { get; private set; }
    public TenantSettings? Settings { get; private set; }

    private Tenant() { }

    public static Tenant Create(Guid adminUserId)
    {
        return new Tenant
        {
            Id = Guid.NewGuid(),
            AdminUserId = new UserId(adminUserId)
        };
    }

    public void SetSubscription(SubscriptionId subscriptionId)
    {
        SubscriptionId = subscriptionId;
    }

    public void ConfigureSettings(string name, string? brandingColor, string? logoUrl, bool receiveNotifications, string timezone = "UTC")
    {
        Settings = TenantSettings.Create(Id, name, brandingColor, logoUrl, receiveNotifications, timezone);
    }
}
