using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mirama.Modules.PM.Domain.Aggregates.Project.Team;

namespace Mirama.Modules.PM.Infrastructure.Persistence.Configurations.Entities;

internal class ProjectTeamConfiguration : IEntityTypeConfiguration<ProjectTeam>
{
    public void Configure(EntityTypeBuilder<ProjectTeam> builder)
    {
        builder.HasKey(pt => pt.Id);

        builder.Property(pt => pt.Id)
            .HasConversion(id => id.Value, v => new ProjectTeamId(v))
            .IsRequired();

        builder.Property(pt => pt.OrganizationId).IsRequired();
        builder.Property(pt => pt.TeamId).IsRequired();
        builder.Property(pt => pt.DateAdded).IsRequired();

        builder.HasIndex("ProjectId", nameof(ProjectTeam.TeamId)).IsUnique();
        builder.HasIndex("ProjectId");
        builder.HasIndex(pt => pt.TeamId);
    }
}
