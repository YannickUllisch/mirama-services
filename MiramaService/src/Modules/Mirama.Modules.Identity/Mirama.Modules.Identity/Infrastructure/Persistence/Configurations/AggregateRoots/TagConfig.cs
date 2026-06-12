using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mirama.Modules.Identity.Domain.Aggregates.Organization.Tag;

namespace Mirama.Modules.Identity.Infrastructure.Persistence.Configurations.AggregateRoots;

public class TagConfiguration : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        builder.HasKey(t => t.Id);
        builder.HasIndex(t => t.OrganizationId);
        builder.HasIndex(t => new { t.OrganizationId, t.Slug }).IsUnique();

        builder.Property(t => t.Id)
            .HasConversion(
                id => id.Value,
                v => new TagId(v))
            .IsRequired();

        builder.Property(t => t.Name).IsRequired().HasMaxLength(100);
        builder.Property(t => t.Slug).IsRequired().HasMaxLength(120);
        builder.Property(t => t.Color).HasMaxLength(7);
        builder.Property(t => t.Description).HasMaxLength(500);
        builder.Property(t => t.Scope).IsRequired();
        builder.Property(t => t.DateCreated).IsRequired();
    }
}
