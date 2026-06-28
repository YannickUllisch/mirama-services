using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mirama.Modules.PM.Domain.Aggregates.Project;
using Mirama.Modules.PM.Domain.Aggregates.Project.Member;
using Mirama.Modules.PM.Domain.Aggregates.Project.Milestone;
using Mirama.Modules.PM.Domain.Aggregates.Project.Team;

namespace Mirama.Modules.PM.Infrastructure.Persistence.Configurations.AggregateRoots;

internal class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasConversion(id => id.Value, v => new ProjectId(v))
            .IsRequired();

        builder.Property(p => p.OrganizationId).IsRequired();
        builder.Property(p => p.Name).IsRequired().HasMaxLength(200);
        builder.Property(p => p.Slug).IsRequired().HasMaxLength(200);
        builder.Property(p => p.Description).HasMaxLength(2000);
        builder.Property(p => p.StartDate).IsRequired();
        builder.Property(p => p.EndDate);
        builder.Property(p => p.StatusId).IsRequired();
        builder.Property(p => p.PriorityId).IsRequired();
        builder.Property(p => p.Budget).IsRequired();
        builder.Property(p => p.IsArchived).IsRequired();
        builder.Property(p => p.DateCreated).IsRequired();

        builder.Property(p => p.TagIds)
            .HasColumnType("jsonb")
            .IsRequired();

        builder.HasIndex(p => p.OrganizationId);
        builder.HasIndex(p => new { p.OrganizationId, p.Slug }).IsUnique();

        builder.HasMany(p => p.Members)
            .WithOne()
            .HasForeignKey("ProjectId")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.Teams)
            .WithOne()
            .HasForeignKey("ProjectId")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.Milestones)
            .WithOne()
            .HasForeignKey("ProjectId")
            .OnDelete(DeleteBehavior.Cascade);
    }
}
