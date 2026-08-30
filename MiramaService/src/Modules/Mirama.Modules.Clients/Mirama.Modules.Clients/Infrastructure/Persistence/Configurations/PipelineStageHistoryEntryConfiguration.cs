using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mirama.Modules.Clients.Domain.Aggregates.Client;
using Mirama.Modules.Clients.Domain.Aggregates.Client.PipelineHistory;

namespace Mirama.Modules.Clients.Infrastructure.Persistence.Configurations;

internal class PipelineStageHistoryEntryConfiguration : IEntityTypeConfiguration<PipelineStageHistoryEntry>
{
    public void Configure(EntityTypeBuilder<PipelineStageHistoryEntry> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasConversion(id => id.Value, v => new PipelineStageHistoryEntryId(v))
            .IsRequired();

        builder.Property(e => e.ClientId)
            .HasConversion(id => id.Value, v => new ClientId(v))
            .IsRequired();

        builder.Property(e => e.FromStatus);
        builder.Property(e => e.ToStatus).IsRequired();
        builder.Property(e => e.ChangedByMemberId);
        builder.Property(e => e.ChangedAt).IsRequired();
    }
}
