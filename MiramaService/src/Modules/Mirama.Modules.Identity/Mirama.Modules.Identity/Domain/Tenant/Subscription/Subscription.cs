using Mirama.Modules.Identity.Domain.Aggregates.Plan;
using Mirama.SharedKernel.Abstractions.Domain.Core;

namespace Mirama.Modules.Identity.Domain.Aggregates.Tenant.Subscription;

public sealed class Subscription : Entity<SubscriptionId>
{
    public PlanId PlanId { get; private set; } = default!;
    public SubscriptionStatus Status { get; private set; }
    public string? StripeSubscriptionId { get; private set; }
    public DateTime PeriodStart { get; private set; }
    public DateTime PeriodEnd { get; private set; }
    public bool CancelAtPeriodEnd { get; private set; }

    private Subscription(SubscriptionDetails details)
    {
        PlanId = details.PlanId;
        PeriodStart = details.PeriodStart;
        PeriodEnd = details.PeriodEnd;
        Status = SubscriptionStatus.Trialing;
        CancelAtPeriodEnd = false;
    }

    private Subscription() { }

    internal static Subscription Create(SubscriptionDetails details)
    {
        return new Subscription(details);
    }

    public void Activate(string? stripeSubscriptionId = null)
    {
        this.Status = SubscriptionStatus.Active;
        this.StripeSubscriptionId = stripeSubscriptionId ?? StripeSubscriptionId;
    }

    public void UpdateStatus(SubscriptionStatus status)
    {
        this.Status = status;
    }

    public void ScheduleCancellation()
    {
        this.CancelAtPeriodEnd = true;
    }

    public void Renew(DateTime newPeriodStart, DateTime newPeriodEnd)
    {
        this.PeriodStart = newPeriodStart;
        this.PeriodEnd = newPeriodEnd;
        this.Status = SubscriptionStatus.Active;
        this.CancelAtPeriodEnd = false;
    }
}
