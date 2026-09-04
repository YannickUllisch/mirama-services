using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mirama.Modules.Workspace.Domain.Aggregates.ViewState;

namespace Mirama.Modules.Workspace.Infrastructure.Persistence.Configurations.AggregateRoots;

internal class ViewStateConfiguration : IEntityTypeConfiguration<Domain.Aggregates.ViewState.ViewState>
{
    public void Configure(EntityTypeBuilder<Domain.Aggregates.ViewState.ViewState> builder)
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

        // View configuration only (column order, filters, collapsed groups) - never business
        // rows - so this stays a small jsonb payload no matter how large the underlying
        // table/board grows. Same jsonb convention as Project.TagIds in the PM module.
        builder.Property(v => v.StateJson)
            .HasColumnType("jsonb")
            .IsRequired();

        // One saved state per (user, org, surface). The org half of this is enforced again
        // by the global org query filter, but the index needs it to actually be unique.
        builder.HasIndex(v => new { v.OrganizationId, v.UserId, v.SurfaceKey }).IsUnique();
        builder.HasIndex(v => v.OrganizationId);

        // Guards against a lost update when a user has two tabs open and both fire a save in
        // quick succession (e.g. a drag reorder racing a filter change). Npgsql's system xmin
        // column gives us this for free - no extra column, no application-managed version.
        builder.UseXminAsConcurrencyToken();
    }
}
