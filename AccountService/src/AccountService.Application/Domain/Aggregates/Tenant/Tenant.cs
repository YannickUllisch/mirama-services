
using AccountService.Application.Domain.Abstractions.Core;
using AccountService.Application.Domain.Aggregates.User;
using ErrorOr;

namespace AccountService.Application.Domain.Aggregates.Tenant;

public sealed class Tenant : AggregateRoot<Guid>
{
    public string Name { get; private set;} = string.Empty;
    
    public UserId AdminUserId { get; init; } = default!;

    public BillingPlan BillingPlan { get; private set; } = BillingPlan.Free;

    public TenantSettings Settings { get; private set; } = default!;

    public bool IsActive { get; private set; } = true;

    public bool IsDeleted { get; private set; } = false;

    private Tenant(UserId admin)
    {
        AdminUserId = admin;
    }

    private Tenant() { }

    public static ErrorOr<Tenant> Create(Guid adminUserId, BillingPlanType planType)
    {
        var userId = new UserId(adminUserId);
        var tenant = new Tenant(userId);
        
        var setPlanResult = tenant.SetBillingPlan(planType);
        if (setPlanResult.IsError)
            return setPlanResult.Errors;

        return tenant;
    }

    public ErrorOr<Success> SetBillingPlan(BillingPlanType planType)
    {
        var plan = BillingPlanFactory.FromType(planType);

        if (plan.IsError)
        {
            return plan.Errors;
        }

        BillingPlan = plan.Value;
        return Result.Success;
    }
}