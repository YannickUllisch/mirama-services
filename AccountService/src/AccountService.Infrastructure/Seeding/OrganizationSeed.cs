

using AccountService.Domain.Organization;
using AccountService.Domain.Organization.ValueObjects;
using AccountService.Infrastructure.Persistence;

namespace AccountService.Infrastructure.Seeding;

public static class OrganizationSeed
{
    public static async Task SeedDataAsync(ApplicationDbContext ctx)
    {
        if (!ctx.Organizations.Any())
        {
            ctx.Organizations.Add(Organization.Create(
                "Mirama",
                new Address("Street1", "Copenhagen", "Denmark", "2400"),
                "system")
            );

            await ctx.SaveChangesAsync();
        }
    }
}