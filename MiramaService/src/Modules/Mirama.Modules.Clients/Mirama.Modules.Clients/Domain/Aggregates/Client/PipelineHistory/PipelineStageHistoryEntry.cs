using Mirama.Modules.Clients.Domain.Aggregates.Client;
using Mirama.Modules.Clients.Domain.Enums;
using Mirama.SharedKernel.Abstractions.Domain.Core;

namespace Mirama.Modules.Clients.Domain.Aggregates.Client.PipelineHistory;

// Same reasoning as ClientActivityLogEntry, this is a log of what happened to
// the pipeline stage over time, not a mutable projection of the current stage.
// FromStatus is nullable to represent the very first entry, created from nothing.
public class PipelineStageHistoryEntry : OrganizationEntity<PipelineStageHistoryEntryId>
{
    public ClientId ClientId { get; init; } = null!;
    public ClientStatus? FromStatus { get; init; }
    public ClientStatus ToStatus { get; init; }
    public Guid? ChangedByMemberId { get; init; }
    public DateTime ChangedAt { get; init; }

    private PipelineStageHistoryEntry() { }

    private PipelineStageHistoryEntry(ClientStatus? fromStatus, ClientStatus toStatus, Guid? changedByMemberId)
    {
        this.FromStatus = fromStatus;
        this.ToStatus = toStatus;
        this.ChangedByMemberId = changedByMemberId;
        this.ChangedAt = DateTime.UtcNow;
    }

    public static PipelineStageHistoryEntry Create(ClientStatus? fromStatus, ClientStatus toStatus, Guid? changedByMemberId)
    {
        return new PipelineStageHistoryEntry(fromStatus, toStatus, changedByMemberId)
        {
            Id = new PipelineStageHistoryEntryId(Guid.NewGuid())
        };
    }
}
