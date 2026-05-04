
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mirama.Modules.Identity.Domain.Aggregates.Organization;
using Mirama.Modules.Identity.Domain.Aggregates.Organization.Member;
using Mirama.Modules.Identity.Domain.Aggregates.User;

namespace Mirama.Modules.Identity.Infrastructure.Persistence.Configurations.Entities;

public class MemberConfiguration : IEntityTypeConfiguration<Member>
{
    public void Configure(EntityTypeBuilder<Member> builder)
    {
        builder.HasKey(m => m.Id);

        builder.HasIndex(i => i.OrganizationId);

        builder.Property(m => m.UserId).IsRequired();
        builder.Property(m => m.Role).IsRequired();

        builder.Property(i => i.OrganizationId).HasConversion(
            orgId => orgId,
            val => val);

        builder.Property(m => m.Id).HasConversion(
            uid => uid.Value,
            val => new MemberId(val));

        builder.Property(m => m.UserId).HasConversion(
            uid => uid.Value,
            val => new UserId(val));
    }
}