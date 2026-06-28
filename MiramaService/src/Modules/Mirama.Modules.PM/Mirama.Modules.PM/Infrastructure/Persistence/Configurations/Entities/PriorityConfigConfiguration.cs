using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mirama.Modules.PM.Domain.Aggregates.WorkflowConfig.Priority;

namespace Mirama.Modules.PM.Infrastructure.Persistence.Configurations.Entities;

internal class PriorityConfigConfiguration : IEntityTypeConfiguration<PriorityConfig>
{
    public void Configure(EntityTypeBuilder<PriorityConfig> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasConversion(id => id.Value, v => new PriorityConfigId(v))
            .IsRequired();

        builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
        builder.Property(p => p.Color).HasMaxLength(20);
        builder.Property(p => p.Icon).HasMaxLength(100);
        builder.Property(p => p.Level).IsRequired();
        builder.Property(p => p.IsDefault).IsRequired();

        builder.HasIndex("WorkflowConfigId");
    }
}
