using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mirama.Modules.PM.Domain.Aggregates.ProjectTemplate.CycleTemplate;

namespace Mirama.Modules.PM.Infrastructure.Persistence.Configurations.Entities;

internal class CycleTemplateConfiguration : IEntityTypeConfiguration<CycleTemplate>
{
    public void Configure(EntityTypeBuilder<CycleTemplate> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .HasConversion(id => id.Value, v => new CycleTemplateId(v))
            .IsRequired();

        builder.Property(c => c.Name).IsRequired().HasMaxLength(200);
        builder.Property(c => c.Goal).HasMaxLength(1000);
        builder.Property(c => c.DurationDays);
        builder.Property(c => c.Position).IsRequired();

        builder.HasIndex("ProjectTemplateId");
    }
}
