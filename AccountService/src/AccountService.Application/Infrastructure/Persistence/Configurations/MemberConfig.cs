
using AccountService.Application.Domain.Organization;
using AccountService.Application.Domain.Organization.ValueObjects;
using AccountService.Application.Domain.User.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccountService.Application.Infrastructure.Persistence.Configurations;

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