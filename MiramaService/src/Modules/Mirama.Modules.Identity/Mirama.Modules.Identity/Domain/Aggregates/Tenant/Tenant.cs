
using Mirama.Modules.Identity.Domain.Aggregates.Tenant.Subscription;
using SubscriptionEntity = Mirama.Modules.Identity.Domain.Aggregates.Tenant.Subscription.Subscription;
using Mirama.Modules.Identity.Domain.Aggregates.User;
using Mirama.SharedKernel.Abstractions.Domain.Core;

namespace Mirama.Modules.Identity.Domain.Aggregates.Tenant;

public sealed class Tenant : AggregateRoot<Guid>
{
    public UserId AdminUserId { get; init; } = default!;
    public SubscriptionEntity Subscription { get; private set; } = default!;
    public TenantSettings Settings { get; private set; } = default!;

    private Tenant() { }

    private Tenant(UserId adminUserId, TenantSettings settings, SubscriptionEntity subscription)
    {
        this.AdminUserId = adminUserId;
        this.Settings = settings;
        this.Subscription = subscription;
    }

    public static Tenant Create(Guid adminUserId, TenantSettingsDetails settings, SubscriptionDetails details)
    {
        if (adminUserId == Guid.Empty)
        {
            throw new ArgumentException("Admin required");
        }

        var settingsObj = TenantSettings.Create(settings);
        var subscription = SubscriptionEntity.Create(details);

        return new Tenant(new UserId(adminUserId), settingsObj, subscription);
    }

    public void SetSubscription(SubscriptionDetails details)
    {
        this.Subscription = SubscriptionEntity.Create(details);
    }

    public void UpdateSettings(TenantSettingsDetails details)
    {
        this.Settings.Update(details);
    }
}
