using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mirama.Modules.PM.Domain.Aggregates.Project.Milestone;

namespace Mirama.Modules.PM.Infrastructure.Persistence.Configurations.Entities;

internal class ProjectMilestoneConfiguration : IEntityTypeConfiguration<ProjectMilestone>
{
    public void Configure(EntityTypeBuilder<ProjectMilestone> builder)
    {
        builder.HasKey(pm => pm.Id);

        builder.Property(pm => pm.Id)
            .HasConversion(id => id.Value, v => new ProjectMilestoneId(v))
            .IsRequired();

        builder.Property(pm => pm.OrganizationId).IsRequired();
        builder.Property(pm => pm.Title).IsRequired().HasMaxLength(200);
        builder.Property(pm => pm.Description).HasMaxLength(2000);
        builder.Property(pm => pm.DueDate).IsRequired();
        builder.Property(pm => pm.Status).IsRequired();
        builder.Property(pm => pm.Color).HasMaxLength(20);
        builder.Property(pm => pm.DateCreated).IsRequired();

        builder.HasIndex("ProjectId");
    }
}
