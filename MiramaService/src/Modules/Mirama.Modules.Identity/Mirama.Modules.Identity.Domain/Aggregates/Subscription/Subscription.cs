using Mirama.Modules.Identity.Domain.Aggregates.Plan;
using Mirama.SharedKernel.Abstractions.Domain.Core;

namespace Mirama.Modules.Identity.Domain.Aggregates.Subscription;

public sealed class Subscription : AggregateRoot<SubscriptionId>
{
    public Guid TenantId { get; private set; }
    public PlanId PlanId { get; private set; } = default!;
    public SubscriptionStatus Status { get; private set; }
    public string? StripeSubscriptionId { get; private set; }
    public DateTime PeriodStart { get; private set; }
    public DateTime PeriodEnd { get; private set; }
    public bool CancelAtPeriodEnd { get; private set; }

    private Subscription() { }

    public static Subscription Create(Guid tenantId, PlanId planId, DateTime periodStart, DateTime periodEnd)
    {
        return new Subscription
        {
            Id = new SubscriptionId(Guid.NewGuid()),
            TenantId = tenantId,
            PlanId = planId,
            Status = SubscriptionStatus.Trialing,
            PeriodStart = periodStart,
            PeriodEnd = periodEnd,
            CancelAtPeriodEnd = false
        };
    }

    public void Activate(string? stripeSubscriptionId = null)
    {
        Status = SubscriptionStatus.Active;
        StripeSubscriptionId = stripeSubscriptionId ?? StripeSubscriptionId;
    }

    public void UpdateStatus(SubscriptionStatus status)
    {
        Status = status;
    }

    public void ScheduleCancellation()
    {
        CancelAtPeriodEnd = true;
    }

    public void Renew(DateTime newPeriodStart, DateTime newPeriodEnd)
    {
        PeriodStart = newPeriodStart;
        PeriodEnd = newPeriodEnd;
        Status = SubscriptionStatus.Active;
        CancelAtPeriodEnd = false;
    }
}
