namespace Mirama.Modules.Identity.Domain.Aggregates.Subscription;

public enum SubscriptionStatus
{
    Trialing,
    Active,
    Canceled,
    PastDue,
    Unpaid,
}
