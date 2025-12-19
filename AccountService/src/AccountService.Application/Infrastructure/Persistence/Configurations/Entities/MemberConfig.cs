
using AccountService.Application.Domain.Aggregates.Organization.Member;
using AccountService.Application.Domain.Aggregates.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccountService.Application.Infrastructure.Persistence.Configurations.Entities;

public class MemberConfiguration : IEntityTypeConfiguration<Member>
{
    public void Configure(EntityTypeBuilder<Member> builder)
    {
        builder.HasKey(m => m.Id);

        builder.HasIndex(i => i.OrganizationId);

        builder.Property(m => m.Id).HasConversion(
            uid => uid.Value,
            val => new MemberId(val));

        builder.Property(m => m.UserId).HasConversion(
            uid => uid.Value,
            val => new UserId(val));
    }
}