using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mirama.Modules.PM.Domain.Aggregates.ProjectTemplate.MilestoneTemplate;

namespace Mirama.Modules.PM.Infrastructure.Persistence.Configurations.Entities;

internal class MilestoneTemplateConfiguration : IEntityTypeConfiguration<MilestoneTemplate>
{
    public void Configure(EntityTypeBuilder<MilestoneTemplate> builder)
    {
        builder.HasKey(m => m.Id);

        builder.Property(m => m.Id)
            .HasConversion(id => id.Value, v => new MilestoneTemplateId(v))
            .IsRequired();

        builder.Property(m => m.Title).IsRequired().HasMaxLength(200);
        builder.Property(m => m.Description).HasMaxLength(2000);
        builder.Property(m => m.DayOffset).IsRequired();
        builder.Property(m => m.Color).HasMaxLength(20);

        builder.HasIndex("ProjectTemplateId");
    }
}
