using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mirama.Modules.PM.Domain.Aggregates.WorkflowConfig.Status;

namespace Mirama.Modules.PM.Infrastructure.Persistence.Configurations.Entities;

internal class StatusConfigConfiguration : IEntityTypeConfiguration<StatusConfig>
{
    public void Configure(EntityTypeBuilder<StatusConfig> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id)
            .HasConversion(id => id.Value, v => new StatusConfigId(v))
            .IsRequired();

        builder.Property(s => s.Name).IsRequired().HasMaxLength(100);
        builder.Property(s => s.Color).HasMaxLength(20);
        builder.Property(s => s.Category).IsRequired();
        builder.Property(s => s.Position).IsRequired();
        builder.Property(s => s.IsDefault).IsRequired();
        builder.Property(s => s.IsTerminal).IsRequired();

        builder.HasIndex("WorkflowConfigId");
    }
}
