
namespace Mirama.Modules.Identity.Domain.Aggregates.Tenant.Subscription;

public enum SubscriptionStatus
{
    Trialing,
    Active,
    Canceled,
    PastDue,
    Unpaid,
}
