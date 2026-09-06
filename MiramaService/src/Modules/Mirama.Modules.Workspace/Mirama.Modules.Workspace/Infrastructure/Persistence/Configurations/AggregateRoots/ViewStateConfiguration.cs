using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mirama.Modules.Workspace.Domain.Aggregates.ViewState;

namespace Mirama.Modules.Workspace.Infrastructure.Persistence.Configurations.AggregateRoots;

internal class ViewStateConfiguration : IEntityTypeConfiguration<ViewState>
{
    public void Configure(EntityTypeBuilder<ViewState> builder)
    {
        builder.HasKey(v => v.Id);

        builder.Property(v => v.Id)
            .HasConversion(id => id.Value, v => new ViewStateId(v))
            .IsRequired();

        builder.Property(v => v.OrganizationId).IsRequired();
        builder.Property(v => v.UserId).IsRequired();

        builder.Property(v => v.SurfaceKey).IsRequired().HasMaxLength(200);

        builder.Property(v => v.ViewType)
            .HasConversion<string>()
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(v => v.StateJson)
            .HasColumnType("jsonb")
            .IsRequired();

        // One saved state per (user, org, surface). The org half of this is enforced again
        // by the global org query filter, but the index needs it to actually be unique.
        builder.HasIndex(v => new { v.OrganizationId, v.UserId, v.SurfaceKey }).IsUnique();
        builder.HasIndex(v => v.OrganizationId);

        builder.Property<uint>("xmin")
            .HasColumnName("xmin")
            .IsRowVersion()
            .ValueGeneratedOnAddOrUpdate();
    }
}
