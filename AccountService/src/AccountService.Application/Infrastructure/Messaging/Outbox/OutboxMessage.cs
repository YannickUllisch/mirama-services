
namespace AccountService.Application.Infrastructure.Messaging.Outbox;

public sealed class OutboxMessage
{
    public Guid Id { get; init; }

    public string Type { get; init; } = string.Empty;

    public string Content { get; init; } = string.Empty;

    public DateTime OccurredAtUtc { get; init; }

    public DateTime? ProcessedAtUtc { get; set; }

    public string? Error { get; set; }
}