

using AccountService.Application.Domain.Aggregates.Organization;
using AccountService.Application.Domain.Aggregates.Tenant;
using AccountService.Application.Infrastructure.Persistence;

namespace AccountService.Application.Infrastructure.Seeding;

public static class OrganizationSeed
{
    public static async Task SeedDataAsync(ApplicationDbContext dbContext)
    {
        if (!dbContext.Tenants.Any())
        {
            dbContext.Tenants.Add(Tenant.Create(Guid.NewGuid(), BillingPlanType.Free).Value);
            dbContext.Organizations.Add(Organization.Create(
                "Mirama",
                "Street1",
                "Copenhagen",
                "Denmark",
                "24000").Value
            );

            await dbContext.SaveChangesAsync();
        }
    }
}