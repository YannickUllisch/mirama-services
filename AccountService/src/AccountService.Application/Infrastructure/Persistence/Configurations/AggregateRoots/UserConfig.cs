
using AccountService.Application.Domain.Aggregates.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccountService.Application.Infrastructure.Persistence.Configurations.AggregateRoots;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);
        
        builder.Property(m => m.Role).IsRequired();

        builder.Property(m => m.Id).HasConversion(
            uid => uid.Value,
            val => new UserId(val));

        builder.OwnsOne(o => o.Contact, a =>
        {
            a.Property(x => x.ContactEmail).HasColumnName("ContactEmail");
            a.Property(x => x.ContactPhoneNumber).HasColumnName("ContactPhone");
        });
    }
}