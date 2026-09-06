using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mirama.Modules.Identity.Domain.Aggregates.Organization;

namespace Mirama.Modules.Identity.Infrastructure.Persistence.Configurations.AggregateRoots;

public class OrganizationConfiguration : IEntityTypeConfiguration<Organization>
{
    public void Configure(EntityTypeBuilder<Organization> builder)
    {
        builder.HasKey(o => o.Id);
        builder.HasIndex(o => o.TenantId);

        // Slugs are the organization's URL segment (/organization/{slug}/...), so they must
        // be globally unique - not just per-tenant - or two organizations in different
        // tenants could resolve to the same route.
        builder.HasIndex(o => o.Slug).IsUnique();

        builder.Property(o => o.Id).HasConversion(
            id => id.Value,
            v => new OrganizationId(v));

        builder.Property(o => o.TenantId).IsRequired();
        builder.Property(o => o.Name).IsRequired();
        builder.Property(o => o.Slug).IsRequired().HasMaxLength(63);
        builder.Property(o => o.Street).IsRequired();
        builder.Property(o => o.City).IsRequired();
        builder.Property(o => o.Country).IsRequired();
        builder.Property(o => o.ZipCode).IsRequired();
        builder.Property(o => o.Region).IsRequired();
        builder.Property(o => o.PrimaryColor).HasMaxLength(7);
        builder.Property(o => o.AccentColor).HasMaxLength(7);
        builder.Property(o => o.DateCreated).IsRequired();
    }
}
