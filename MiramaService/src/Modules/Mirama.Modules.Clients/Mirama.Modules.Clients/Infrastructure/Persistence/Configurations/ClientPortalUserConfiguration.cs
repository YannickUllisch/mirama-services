using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mirama.Modules.Clients.Domain.Aggregates.Client.ClientPortalUser;

namespace Mirama.Modules.Clients.Infrastructure.Persistence.Configurations;

internal class ClientPortalUserConfiguration : IEntityTypeConfiguration<ClientPortalUser>
{
    public void Configure(EntityTypeBuilder<ClientPortalUser> builder)
    {
        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
            .HasConversion(id => id.Value, v => new ClientPortalUserId(v))
            .IsRequired();

        builder.Property(u => u.ClientId).IsRequired();
        builder.Property(u => u.ContactId).IsRequired();
        builder.Property(u => u.IsActive).IsRequired();
        builder.Property(u => u.LastLogin);

        builder.HasIndex(u => new { u.ClientId, u.ContactId }).IsUnique();
    }
}
