
using AccountService.Application.Domain.User;
using AccountService.Application.Domain.User.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccountService.Application.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);

        builder.Property(m => m.Id).HasConversion(
            uid => uid.Value,
            val => new UserId(val));

        builder.Ignore(e => e.DomainEvents);

        builder.OwnsOne(o => o.Contact, a =>
        {
            a.Property(x => x.ContactEmail).HasColumnName("ContactEmail");
            a.Property(x => x.ContactPhoneNumber).HasColumnName("ContactPhone");
        });
    }
}