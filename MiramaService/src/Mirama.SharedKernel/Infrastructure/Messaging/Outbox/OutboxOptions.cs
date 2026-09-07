
namespace Mirama.SharedKernel.Infrastructure.Messaging.Outbox;

public sealed class OutboxOptions
{
    public int BatchSize { get; set; } = 50;

    public int MaxRetries { get; set; } = 10;

    public int MaxDegreeOfParallelism { get; set; } = 4;

    public TimeSpan LeaseDuration { get; set; } = TimeSpan.FromSeconds(30);

    public TimeSpan BusyPollInterval { get; set; } = TimeSpan.FromMilliseconds(200);

    public TimeSpan IdlePollInterval { get; set; } = TimeSpan.FromSeconds(5);

    private static TimeSpan Backoff(int attempt) =>
        TimeSpan.FromSeconds(Math.Min(300, Math.Pow(2, attempt)));

    public TimeSpan RetryBackoff(int retryCount) => Backoff(retryCount);
}
