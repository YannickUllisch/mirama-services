using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mirama.Modules.Clients.Domain.Aggregates.Client;

namespace Mirama.Modules.Clients.Infrastructure.Persistence.Configurations;

internal class ClientConfiguration : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .HasConversion(id => id.Value, v => new ClientId(v))
            .IsRequired();

        builder.Property(c => c.Name).IsRequired().HasMaxLength(150);
        builder.Property(c => c.Type).IsRequired();
        builder.Property(c => c.Status).IsRequired();
        builder.Property(c => c.Website).HasMaxLength(500);
        builder.Property(c => c.Industry).HasMaxLength(100);
        builder.Property(c => c.Notes).HasMaxLength(2000);
        builder.Property(c => c.TenantId).IsRequired();
        builder.Property(c => c.OrganizationId).IsRequired();

        builder.HasIndex(c => c.TenantId);
        builder.HasIndex(c => c.OrganizationId);

        builder.HasMany(c => c.Contacts)
            .WithOne()
            .HasForeignKey(ct => ct.ClientId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(c => c.PortalInvitations)
            .WithOne()
            .HasForeignKey(i => i.ClientId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(c => c.PortalUsers)
            .WithOne()
            .HasForeignKey(u => u.ClientId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
