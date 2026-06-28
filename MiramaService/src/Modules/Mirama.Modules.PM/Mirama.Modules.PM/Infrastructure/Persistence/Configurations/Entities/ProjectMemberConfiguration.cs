using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mirama.Modules.PM.Domain.Aggregates.Project.Member;

namespace Mirama.Modules.PM.Infrastructure.Persistence.Configurations.Entities;

internal class ProjectMemberConfiguration : IEntityTypeConfiguration<ProjectMember>
{
    public void Configure(EntityTypeBuilder<ProjectMember> builder)
    {
        builder.HasKey(pm => pm.Id);

        builder.Property(pm => pm.Id)
            .HasConversion(id => id.Value, v => new ProjectMemberId(v))
            .IsRequired();

        builder.Property(pm => pm.OrganizationId).IsRequired();
        builder.Property(pm => pm.MemberId).IsRequired();
        builder.Property(pm => pm.RoleId).IsRequired();
        builder.Property(pm => pm.IsInherited).IsRequired();
        builder.Property(pm => pm.TeamId);

        builder.HasIndex("ProjectId", nameof(ProjectMember.MemberId)).IsUnique();
        builder.HasIndex("ProjectId");
        builder.HasIndex(pm => pm.MemberId);
    }
}
