
namespace Mirama.SharedKernel.Infrastructure.Messaging.Outbox;

/// <summary>
/// Durable archive of successfully fanned-out <see cref="OutboxMessage"/> rows. OutboxCleanupWorker
/// moves a message here once its <c>ProcessedAtUtc</c> is set, so the live OutboxMessages table
/// stays small for claiming performance while a record of what was published is kept for
/// operational visibility. A message that instead exhausts its fan-out retries goes to
/// <see cref="OutboxDeadLetter"/> — a given message is archived to exactly one of the two,
/// never both.
/// </summary>
public sealed class OutboxHistory
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

    public int RetryCount { get; init; }

    public string? Error { get; init; }

    public DateTime ProcessedAtUtc { get; init; }

    public DateTime ArchivedAtUtc { get; init; }
}
