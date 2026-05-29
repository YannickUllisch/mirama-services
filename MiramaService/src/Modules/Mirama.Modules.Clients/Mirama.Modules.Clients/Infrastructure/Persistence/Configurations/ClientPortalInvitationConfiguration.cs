using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mirama.Modules.Clients.Domain.Aggregates.Client;
using Mirama.Modules.Clients.Domain.Aggregates.Client.ClientPortalInvitation;

namespace Mirama.Modules.Clients.Infrastructure.Persistence.Configurations;

internal class ClientPortalInvitationConfiguration : IEntityTypeConfiguration<ClientPortalInvitation>
{
    public void Configure(EntityTypeBuilder<ClientPortalInvitation> builder)
    {
        builder.HasKey(i => i.Id);

        builder.Property(i => i.Id)
            .HasConversion(id => id.Value, v => new ClientPortalInvitationId(v))
            .IsRequired();

        builder.Property(i => i.ClientId)
            .HasConversion(id => id.Value, v => new ClientId(v))
            .IsRequired();
        builder.Property(i => i.ContactId).IsRequired();
        builder.Property(i => i.Token).IsRequired();
        builder.Property(i => i.Status).IsRequired();
        builder.Property(i => i.ExpiresAt).IsRequired();
        builder.Property(i => i.SentAt).IsRequired();

        builder.HasIndex(i => i.Token).IsUnique();
    }
}
