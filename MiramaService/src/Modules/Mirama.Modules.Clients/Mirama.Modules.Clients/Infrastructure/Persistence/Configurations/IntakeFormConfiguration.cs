using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mirama.Modules.Clients.Domain.Aggregates.IntakeForm;

namespace Mirama.Modules.Clients.Infrastructure.Persistence.Configurations;

internal class IntakeFormConfiguration : IEntityTypeConfiguration<IntakeForm>
{
    public void Configure(EntityTypeBuilder<IntakeForm> builder)
    {
        builder.HasKey(f => f.Id);

        builder.Property(f => f.Id)
            .HasConversion(id => id.Value, v => new IntakeFormId(v))
            .IsRequired();

        builder.Property(f => f.Name).IsRequired().HasMaxLength(150);
        builder.Property(f => f.Version).IsRequired();
        builder.Property(f => f.IsActive).IsRequired();
        builder.Property(f => f.OrganizationId).IsRequired();

        builder.HasIndex(f => f.OrganizationId);

        // Value object collection with no identity of its own, stored inline
        // as jsonb rather than a joined table. Properties are get-only
        // (constructor-bound), so each one must be configured explicitly -
        // convention-based discovery skips properties without a setter.
        builder.OwnsMany(f => f.Fields, fb =>
        {
            fb.ToJson();
            fb.Property(x => x.Label).IsRequired();
            fb.Property(x => x.Type).IsRequired();
            fb.Property(x => x.IsRequired).IsRequired();
            fb.Property(x => x.Options);
        });
    }
}
