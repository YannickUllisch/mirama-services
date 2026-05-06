
using Mirama.Modules.Identity.Domain.Aggregates.Organization;
using Mirama.Modules.Identity.Domain.Aggregates.Tenant;
using Mirama.Modules.Identity.Infrastructure.Persistence;

namespace Mirama.Modules.Identity.Infrastructure.Seeding;

public static class OrganizationSeed
{
    public static async Task SeedDataAsync(IdentityDbContext dbContext)
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