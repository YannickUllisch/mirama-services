

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mirama.Modules.Identity.Domain.Aggregates.Account;
using Mirama.Modules.Identity.Domain.Aggregates.User;

namespace Mirama.Modules.Identity.Infrastructure.Persistence.Configurations.AggregateRoots;

public class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.HasKey(u => u.Id);

        builder.Property(t => t.Id)
            .HasConversion(
                id => id.Value,
                v => new AccountId(v))
            .IsRequired();

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