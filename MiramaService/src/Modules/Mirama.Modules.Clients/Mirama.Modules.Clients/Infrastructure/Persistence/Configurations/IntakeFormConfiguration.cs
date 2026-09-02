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
        // convention-based discovery skips properties without a setter. Same
        // goes for the nested owned types (Validation/Presentation/VisibleWhen).
        builder.OwnsMany(f => f.Fields, fb =>
        {
            fb.ToJson();
            fb.Property(x => x.Key).IsRequired();
            fb.Property(x => x.Label).IsRequired();
            fb.Property(x => x.Type).IsRequired();
            fb.Property(x => x.IsRequired).IsRequired();
            fb.Property(x => x.Options);
            fb.Property(x => x.SectionKey);

            fb.OwnsOne(x => x.Validation, vb =>
            {
                vb.Property(v => v.MinLength);
                vb.Property(v => v.MaxLength);
                vb.Property(v => v.Pattern);
                vb.Property(v => v.MinValue);
                vb.Property(v => v.MaxValue);
                vb.Property(v => v.MinDate);
                vb.Property(v => v.MaxDate);
                vb.Property(v => v.MinSelections);
                vb.Property(v => v.MaxSelections);
            });

            fb.OwnsOne(x => x.Presentation, pb =>
            {
                pb.Property(p => p.Placeholder);
                pb.Property(p => p.HelpText);
                pb.Property(p => p.Width).IsRequired();
            });

            fb.OwnsOne(x => x.VisibleWhen, cb =>
            {
                cb.Property(c => c.DependsOnFieldKey).IsRequired();
                cb.Property(c => c.Operator).IsRequired();
                cb.Property(c => c.Value);
            });
        });

        // Ordering is positional in the JSON array, mirroring Sections'
        // in-memory list - no separate Order column to drift out of sync.
        builder.OwnsMany(f => f.Sections, sb =>
        {
            sb.ToJson();
            sb.Property(s => s.Key).IsRequired();
            sb.Property(s => s.Title).IsRequired();
            sb.Property(s => s.Description);
        });
    }
}
