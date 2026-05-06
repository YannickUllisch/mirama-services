using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mirama.Modules.Identity.Domain.Aggregates.Organization.Member;
using Mirama.Modules.Identity.Domain.Aggregates.Organization.Team;

namespace Mirama.Modules.Identity.Infrastructure.Persistence.Configurations.Entities;

public class TeamMemberConfiguration : IEntityTypeConfiguration<TeamMember>
{
    public void Configure(EntityTypeBuilder<TeamMember> builder)
    {
        builder.HasKey(tm => tm.Id);

        builder.Property(tm => tm.Id)
            .HasConversion(
                id => id.Value,
                v => new TeamMemberId(v))
            .IsRequired();

        builder.Property(tm => tm.TeamId)
            .HasConversion(
                id => id.Value,
                v => new TeamId(v))
            .IsRequired();

        builder.Property(tm => tm.MemberId)
            .HasConversion(
                id => id.Value,
                v => new MemberId(v))
            .IsRequired();

        builder.Property(tm => tm.OrganizationId).IsRequired();

        builder.HasIndex(tm => new { tm.TeamId, tm.MemberId }).IsUnique();
        builder.HasIndex(tm => tm.TeamId);
        builder.HasIndex(tm => tm.MemberId);
    }
}
