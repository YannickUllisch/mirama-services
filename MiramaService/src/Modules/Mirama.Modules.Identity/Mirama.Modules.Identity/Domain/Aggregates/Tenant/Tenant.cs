
using Mirama.Modules.Identity.Domain.Aggregates.Tenant.Subscription;
using SubscriptionEntity = Mirama.Modules.Identity.Domain.Aggregates.Tenant.Subscription.Subscription;
using Mirama.Modules.Identity.Domain.Aggregates.User;
using Mirama.SharedKernel.Abstractions.Domain.Core;

namespace Mirama.Modules.Identity.Domain.Aggregates.Tenant;

public sealed class Tenant : AggregateRoot<Guid>
{
    public UserId AdminUserId { get; init; } = default!;
    public SubscriptionEntity Subscription { get; private set; } = default!;
    public string Name { get; private set; } = string.Empty;
    public bool IsActive { get; private set; } = true;

    private Tenant() { }

    private Tenant(UserId adminUserId, string name, SubscriptionEntity subscription)
    {
        this.AdminUserId = adminUserId;
        this.Name = name;
        this.Subscription = subscription;
    }

    public static Tenant Create(Guid adminUserId, string name, SubscriptionDetails details)
    {
        if (adminUserId == Guid.Empty)
        {
            throw new ArgumentException("Admin required");
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Tenant Name cannot be empty.", nameof(name));
        }

        var subscription = SubscriptionEntity.Create(details);

        return new Tenant(new UserId(adminUserId), name.Trim(), subscription);
    }

    public void SetSubscription(SubscriptionDetails details)
    {
        this.Subscription = SubscriptionEntity.Create(details);
    }

    public void UpdateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name is required.");
        }

        this.Name = name.Trim();
    }

    public void SetActive(bool isActive)
    {
        this.IsActive = isActive;
    }
}
