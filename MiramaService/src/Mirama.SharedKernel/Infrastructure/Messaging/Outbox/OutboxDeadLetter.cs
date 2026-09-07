
namespace Mirama.SharedKernel.Infrastructure.Messaging.Outbox;

public sealed class OutboxDeadLetter
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
    public int RetryCount { get; init; }
    public string? Error { get; init; }
    public DateTime DeadLetteredAtUtc { get; init; }
}
