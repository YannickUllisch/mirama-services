
namespace AccountService.Application.Infrastructure.Persistence.Idempotency;

public sealed class IdempotencyRecord
{
    public Guid Id { get; init; }

    public string Key { get; init; } = string.Empty;

    public DateTime CreatedAtUtc { get; init; } = DateTime.UtcNow;

    public string? Response { get; set; }
    
    public DateTime? ProcessedAtUtc { get; set; }
}