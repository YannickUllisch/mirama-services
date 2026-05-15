using Mirama.Modules.Identity.Domain.Aggregates.Plan;
using Microsoft.EntityFrameworkCore;

namespace Mirama.Modules.Identity.Infrastructure.Persistence.Seeding;

public static class PlanSeed
{
    private record SeedPlan(
        string Name,
        string Description,
        int Price,
        string Interval,
        IReadOnlyList<string> Features);

    private static readonly IReadOnlyList<SeedPlan> Seeds =
    [
        new(
            Name: "Free",
            Description: "For solo designers exploring the platform.",
            Price: 0,
            Interval: "month",
            Features:
            [
                "max_organizations:1",
                "max_projects_per_org:3",
                "storage_gb:5",
                "max_members_per_org:2",
                "approval_flows:false",
                "custom_branding:false",
            ]),

        new(
            Name: "Freelancer",
            Description: "For active freelancers managing multiple clients with professional sign-offs.",
            Price: 199,
            Interval: "month",
            Features:
            [
                "max_organizations:1",
                "max_projects_per_org:15",
                "storage_gb:50",
                "max_members_per_org:5",
                "approval_flows:true",
                "custom_branding:false",
            ]),

        new(
            Name: "Studio",
            Description: "For growing design studios and collaborative teams.",
            Price: 999,
            Interval: "month",
            Features:
            [
                "max_organizations:1",
                "max_projects_per_org:100",
                "storage_gb:300",
                "max_members_per_org:20",
                "approval_flows:true",
                "custom_branding:true",
            ]),

        new(
            Name: "Agency",
            Description: "For large design agencies operating at scale.",
            Price: 9999,
            Interval: "month",
            Features:
            [
                "max_organizations:3",
                "max_projects_per_org:999",
                "storage_gb:1000",
                "max_members_per_org:999",
                "approval_flows:true",
                "custom_branding:true",
            ]),
    ];

    public static async Task SeedDataAsync(IdentityDbContext dbContext)
    {
        var existingNames = await dbContext.Plans
            .Select(p => p.Name)
            .ToHashSetAsync();

        var toAdd = Seeds
            .Where(s => !existingNames.Contains(s.Name))
            .ToList();

        if (toAdd.Count == 0)
            return;

        foreach (var seed in toAdd)
        {
            var plan = Plan.Create(new PlanDetails(
                seed.Name,
                seed.Price,
                seed.Interval,
                seed.Features,
                seed.Description));

            dbContext.Plans.Add(plan);
        }

        await dbContext.SaveChangesAsync();
    }
}
