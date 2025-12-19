

using AccountService.Application.Domain.Aggregates.Organization;
using AccountService.Application.Infrastructure.Persistence;

namespace AccountService.Application.Infrastructure.Seeding;

public static class OrganizationSeed
{
    public static async Task SeedDataAsync(ApplicationDbContext ctx)
    {
        if (!ctx.Organizations.Any())
        {
            ctx.Organizations.Add(Organization.Create(
                "Mirama",
                "Street1",
                "Copenhagen",
                "Denmark",
                "24000").Value
            );

            await ctx.SaveChangesAsync();
        }
    }
}