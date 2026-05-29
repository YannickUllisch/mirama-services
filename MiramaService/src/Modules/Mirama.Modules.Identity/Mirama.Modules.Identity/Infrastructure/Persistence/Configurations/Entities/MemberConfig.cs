using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mirama.Modules.Identity.Domain.Aggregates.Organization.Member;
using Mirama.Modules.Identity.Domain.Aggregates.Role;
using Mirama.Modules.Identity.Domain.Aggregates.User;

namespace Mirama.Modules.Identity.Infrastructure.Persistence.Configurations.Entities;

public class MemberConfiguration : IEntityTypeConfiguration<Member>
{
    public void Configure(EntityTypeBuilder<Member> builder)
    {
        builder.HasKey(m => m.Id);
        builder.HasIndex(m => m.OrganizationId);

        builder.Property(m => m.Id).HasConversion(
            uid => uid.Value,
            val => new MemberId(val));

        builder.Property(m => m.UserId).HasConversion(
            uid => uid != null ? uid.Value : (Guid?)null,
            val => val.HasValue ? new UserId(val.Value) : null);

        builder.Property(m => m.IamRoleId).HasConversion(
            rid => rid.Value,
            val => new RoleId(val))
            .IsRequired();

        builder.Property(m => m.Name).IsRequired();
        builder.Property(m => m.Email).IsRequired();

    }
}
