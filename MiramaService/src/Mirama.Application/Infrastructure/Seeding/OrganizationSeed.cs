

using Mirama.Application.Domain.Aggregates.Organization;
using Mirama.Application.Domain.Aggregates.Tenant;
using Mirama.Application.Infrastructure.Persistence;

namespace Mirama.Application.Infrastructure.Seeding;

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