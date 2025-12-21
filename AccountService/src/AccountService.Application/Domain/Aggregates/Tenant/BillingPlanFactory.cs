
using ErrorOr;

namespace AccountService.Application.Domain.Aggregates.Tenant;

public static class BillingPlanFactory
{
    public static ErrorOr<BillingPlan> FromType(BillingPlanType type) => type switch
    {
        BillingPlanType.Free => BillingPlan.Free,
        BillingPlanType.Basic => BillingPlan.Basic,
        BillingPlanType.Standard => BillingPlan.Standard,
        BillingPlanType.Premium => BillingPlan.Premium,
        BillingPlanType.Enterprise => BillingPlan.Enterprise,
        _ => Error.NotFound("Billing plan was not found")
    };
}