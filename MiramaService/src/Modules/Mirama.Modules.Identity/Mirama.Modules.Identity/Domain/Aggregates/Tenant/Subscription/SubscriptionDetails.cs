using Mirama.Modules.Identity.Domain.Aggregates.Plan;

namespace Mirama.Modules.Identity.Domain.Aggregates.Tenant.Subscription;

public sealed record SubscriptionDetails(
    PlanId PlanId,
    DateTime PeriodStart,
    DateTime PeriodEnd
);
