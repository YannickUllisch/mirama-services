
using AccountService.Application.Domain.Abstractions.Core;
using AccountService.Application.Domain.Aggregates.User;

namespace AccountService.Application.Domain.Aggregates.Tenant;

public sealed class Tenant : AggregateRoot<Guid>
{
    public string Name { get; private set;} = string.Empty;
    
    public UserId AdminUserId { get; init; } = default!;

    public int MaxOrganizations { get; private set; } = 1;

    public int MaxUsers { get; private set; } = 3;

    public BillingPlan BillingPlan { get; private set; } = BillingPlan.Free;
    
    public bool IsActive { get; private set; } = true;

    public bool IsDeleted { get; private set; } = false;

    private Tenant(UserId admin)
    {
        AdminUserId = admin;
    }

    private Tenant() { }

    public static Tenant Create(Guid adminUserId)
    {
        var userId = new UserId(adminUserId);

        return new Tenant(userId);
    }
}