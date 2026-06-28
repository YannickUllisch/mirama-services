using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mirama.Modules.PM.Domain.Aggregates.ProjectTemplate.TaskTemplate;

namespace Mirama.Modules.PM.Infrastructure.Persistence.Configurations.Entities;

internal class TaskTemplateConfiguration : IEntityTypeConfiguration<TaskTemplate>
{
    public void Configure(EntityTypeBuilder<TaskTemplate> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id)
            .HasConversion(id => id.Value, v => new TaskTemplateId(v))
            .IsRequired();

        builder.Property(t => t.Title).IsRequired().HasMaxLength(200);
        builder.Property(t => t.Description).HasMaxLength(2000);
        builder.Property(t => t.Type).IsRequired();
        builder.Property(t => t.EstimatedHours);
        builder.Property(t => t.Position).IsRequired();

        builder.Property(t => t.ParentTemplateTaskId)
            .HasConversion(
                id => id == null ? (Guid?)null : id.Value,
                v => v == null ? null : new TaskTemplateId(v.Value));

        builder.HasOne<TaskTemplate>()
            .WithMany()
            .HasForeignKey(t => t.ParentTemplateTaskId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex("ProjectTemplateId");
        builder.HasIndex(t => t.ParentTemplateTaskId);
    }
}
