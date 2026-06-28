using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mirama.Modules.PM.Domain.Aggregates.ProjectTemplate;

namespace Mirama.Modules.PM.Infrastructure.Persistence.Configurations.AggregateRoots;

internal class ProjectTemplateConfiguration : IEntityTypeConfiguration<ProjectTemplate>
{
    public void Configure(EntityTypeBuilder<ProjectTemplate> builder)
    {
        builder.HasKey(pt => pt.Id);

        builder.Property(pt => pt.Id)
            .HasConversion(id => id.Value, v => new ProjectTemplateId(v))
            .IsRequired();

        builder.Property(pt => pt.OrganizationId).IsRequired();
        builder.Property(pt => pt.Name).IsRequired().HasMaxLength(200);
        builder.Property(pt => pt.Description).HasMaxLength(2000);
        builder.Property(pt => pt.Category).HasMaxLength(100);
        builder.Property(pt => pt.IsPublic).IsRequired();
        builder.Property(pt => pt.DateCreated).IsRequired();

        builder.HasIndex(pt => pt.OrganizationId);

        builder.HasMany(pt => pt.TaskTemplates)
            .WithOne()
            .HasForeignKey("ProjectTemplateId")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(pt => pt.MilestoneTemplates)
            .WithOne()
            .HasForeignKey("ProjectTemplateId")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(pt => pt.CycleTemplates)
            .WithOne()
            .HasForeignKey("ProjectTemplateId")
            .OnDelete(DeleteBehavior.Cascade);
    }
}
