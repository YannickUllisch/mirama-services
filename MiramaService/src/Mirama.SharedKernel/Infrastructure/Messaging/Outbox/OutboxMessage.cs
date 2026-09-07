
namespace Mirama.SharedKernel.Infrastructure.Messaging.Outbox;

public sealed class OutboxMessage
{
    public Guid Id { get; init; }

    public Guid? AggregateId { get; init; }

    public Guid? OrganizationId { get; init; }

    public Guid? TenantId { get; init; }

    public string? TraceId { get; init; }

    public string? CorrelationId { get; init; }

    public string? Headers { get; init; }

    public string Type { get; init; } = string.Empty;

    public string Content { get; init; } = string.Empty;

    public DateTime OccurredAtUtc { get; init; }

    /// <summary>Earliest time this message is eligible to be claimed again. Equals
    /// OccurredAtUtc initially; pushed forward on each fan-out retry (exponential backoff).</summary>
    public DateTime AvailableAtUtc { get; set; }

    /// <summary>Lease expiry set by whichever running instance currently owns this message.
    /// Lets a crashed instance's claim expire so another instance can pick it back up —
    /// this, plus claiming via FOR UPDATE SKIP LOCKED, is what makes running this
    /// processor in every instance safe with zero coordination.</summary>
    public DateTime? LockedUntilUtc { get; set; }

    public string? LockedBy { get; set; }

    public int RetryCount { get; set; }

    public string? Error { get; set; }
}
