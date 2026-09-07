
namespace Mirama.SharedKernel.Infrastructure.Messaging.Inbox;

public sealed class InboxMessage
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

    public string HandlerName { get; init; } = string.Empty;

    public DateTime OccurredAtUtc { get; init; }

    public DateTime AvailableAtUtc { get; set; }

    public DateTime? LockedUntilUtc { get; set; }

    public string? LockedBy { get; set; }

    public int RetryCount { get; set; }

    public string? Error { get; set; }
}
