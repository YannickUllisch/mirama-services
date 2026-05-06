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
            var seedSettings = new TenantSettingsDetails("Mirama", true, null, null);
            dbContext.Tenants.Add(Tenant.Create(Guid.NewGuid(), seedSettings));
            dbContext.Organizations.Add(Organization.Create(
                new OrganizationDetails("Mirama", "Street1", "Copenhagen", "Denmark", "24000")));

            await dbContext.SaveChangesAsync();
        }
    }
}
