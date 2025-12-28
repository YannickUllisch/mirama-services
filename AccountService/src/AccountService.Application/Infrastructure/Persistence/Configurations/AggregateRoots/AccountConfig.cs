

using AccountService.Application.Domain.Aggregates.Account;
using AccountService.Application.Domain.Aggregates.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccountService.Application.Infrastructure.Persistence.Configurations.AggregateRoots;

public class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.HasKey(u => u.Id);

        builder.Property(t => t.UserId)
            .HasConversion(
                id => id.Value,
                v => new UserId(v))
            .IsRequired();

        builder.HasIndex(a => new { a.Provider, a.ProviderUserId }).IsUnique();

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}