using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mirama.Modules.PM.Domain.Aggregates.Project;
using Mirama.Modules.PM.Domain.Aggregates.WorkflowConfig;

namespace Mirama.Modules.PM.Infrastructure.Persistence.Configurations.AggregateRoots;

internal class WorkflowConfigConfiguration : IEntityTypeConfiguration<WorkflowConfig>
{
    public void Configure(EntityTypeBuilder<WorkflowConfig> builder)
    {
        builder.HasKey(wc => wc.Id);

        builder.Property(wc => wc.Id)
            .HasConversion(id => id.Value, v => new WorkflowConfigId(v))
            .IsRequired();

        builder.Property(wc => wc.OrganizationId).IsRequired();
        builder.Property(wc => wc.ProjectId)
            .HasConversion(id => id.Value, v => new ProjectId(v))
            .IsRequired();

        builder.HasIndex(wc => wc.ProjectId).IsUnique();
        builder.HasIndex(wc => wc.OrganizationId);

        builder.HasOne<Project>()
            .WithOne()
            .HasForeignKey<WorkflowConfig>(wc => wc.ProjectId)
            .HasPrincipalKey<Project>(p => p.Id)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(wc => wc.Statuses)
            .WithOne()
            .HasForeignKey("WorkflowConfigId")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(wc => wc.Priorities)
            .WithOne()
            .HasForeignKey("WorkflowConfigId")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(wc => wc.TaskStatuses)
            .WithOne()
            .HasForeignKey("WorkflowConfigIdForTaskStatus")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(wc => wc.TaskPriorities)
            .WithOne()
            .HasForeignKey("WorkflowConfigIdForTaskPriority")
            .OnDelete(DeleteBehavior.Cascade);
    }
}
