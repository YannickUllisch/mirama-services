using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mirama.Modules.Clients.Domain.Aggregates.Client;
using Mirama.Modules.Clients.Domain.Aggregates.Client.ActivityLog;

namespace Mirama.Modules.Clients.Infrastructure.Persistence.Configurations;

internal class ClientActivityLogEntryConfiguration : IEntityTypeConfiguration<ClientActivityLogEntry>
{
    public void Configure(EntityTypeBuilder<ClientActivityLogEntry> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasConversion(id => id.Value, v => new ClientActivityLogEntryId(v))
            .IsRequired();

        builder.Property(e => e.ClientId)
            .HasConversion(id => id.Value, v => new ClientId(v))
            .IsRequired();

        builder.Property(e => e.Type).IsRequired();
        builder.Property(e => e.Body).IsRequired().HasMaxLength(2000);
        builder.Property(e => e.LoggedByMemberId).IsRequired();
        builder.Property(e => e.LoggedAt).IsRequired();
    }
}
