
namespace Mirama.SharedKernel.Infrastructure.Messaging.Outbox;

public sealed class OutboxCleanupOptions
{
    public int BatchSize { get; set; } = 500;

    public TimeSpan Interval { get; set; } = TimeSpan.FromMinutes(15);
}
