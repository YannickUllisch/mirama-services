

using AccountService.Application.Domain.Abstractions.Core;

namespace AccountService.Application.Domain.Aggregates.Tenant;

public sealed class BillingPlan : ValueObject
{
    public string Name { get; } = string.Empty;
    public int QuotaTeams { get; }
    public int QuotaUsers { get; }
    public int QuotaOrganizations { get; }

    private BillingPlan() {}

    private BillingPlan(string planName, int quotaTeams, int quotaUsers, int quotaOrganizations)
    {
        Name = planName;
        QuotaTeams = quotaTeams;
        QuotaUsers = quotaUsers;
        QuotaOrganizations = quotaOrganizations;
    }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Name;
        yield return QuotaTeams;
        yield return QuotaUsers;
        yield return QuotaOrganizations;
    }

    public static BillingPlan Free => new("Free", 3, 3, 1);
    public static BillingPlan Basic => new("Basic", 5, 15, 1);
    public static BillingPlan Standard => new("Standard", 10, 25, 3);
    public static BillingPlan Premium => new("Premium", 25, 100, 5);
    public static BillingPlan Enterprise => new("Enterprise", 100, 1000, 10);
}