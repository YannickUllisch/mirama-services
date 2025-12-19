

using AccountService.Application.Domain.Aggregates.Tenant;
using AccountService.Application.Domain.Aggregates.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccountService.Application.Infrastructure.Persistence.Configurations.AggregateRoots;

public class TenantConfiguration : IEntityTypeConfiguration<Tenant>
{
    public void Configure(EntityTypeBuilder<Tenant> builder)
    {
        builder.HasKey(u => u.Id);

        builder.Ignore(u => u.DomainEvents);

        builder.Property(t => t.AdminUserId)
        .HasConversion(
            id => id.Value,
            v => new UserId(v))
        .IsRequired();

        builder.HasIndex(t => t.AdminUserId)
            .IsUnique();

        builder.HasOne<User>()
            .WithOne()
            .HasForeignKey<Tenant>(t => t.AdminUserId)
            .OnDelete(DeleteBehavior.Restrict);

    }
}