using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mirama.Modules.Clients.Domain.Aggregates.Client;
using Mirama.Modules.Clients.Domain.Aggregates.Client.Contact;

namespace Mirama.Modules.Clients.Infrastructure.Persistence.Configurations;

internal class ContactConfiguration : IEntityTypeConfiguration<Contact>
{
    public void Configure(EntityTypeBuilder<Contact> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .HasConversion(id => id.Value, v => new ContactId(v))
            .IsRequired();

        builder.Property(c => c.ClientId)
            .HasConversion(id => id.Value, v => new ClientId(v))
            .IsRequired();
        builder.Property(c => c.FirstName).IsRequired().HasMaxLength(100);
        builder.Property(c => c.LastName).IsRequired().HasMaxLength(100);
        builder.Property(c => c.Email).IsRequired().HasMaxLength(254);
        builder.Property(c => c.Phone).HasMaxLength(50);
        builder.Property(c => c.JobTitle).HasMaxLength(100);
        builder.Property(c => c.IsPrimary).IsRequired();

        builder.HasIndex(c => new { c.ClientId, c.Email }).IsUnique();
        builder.Ignore(c => c.FullName);
    }
}
