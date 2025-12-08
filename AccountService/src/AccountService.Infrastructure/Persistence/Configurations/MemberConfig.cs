
using AccountService.Domain.Organization;
using AccountService.Domain.Organization.ValueObjects;
using AccountService.Domain.User.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccountService.Infrastructure.Persistence.Configurations;

public class MemberConfiguration : IEntityTypeConfiguration<Member>
{
    public void Configure(EntityTypeBuilder<Member> builder)
    {
        builder.HasKey(m => m.Id);

        builder.Property(m => m.Id).HasConversion(
            memId => memId.Value,
            val => new MemberId(val));

        builder.Property(m => m.UserId).HasConversion(
            uid => uid.Value,
            val => new UserId(val));

        builder.Property(m => m.OrganizationId).HasConversion(
            oid => oid.Value,
            val => new OrganizationId(val));
    }
}