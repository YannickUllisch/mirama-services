
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Mirama.SharedKernel.Infrastructure.Messaging.Outbox;

public sealed class OutboxCleanupWorker<TDbContext>(
    IServiceScopeFactory scopeFactory,
    IOptionsMonitor<OutboxCleanupOptions> optionsMonitor,
    ILogger<OutboxCleanupWorker<TDbContext>> logger,
    string moduleName) : BackgroundService
    where TDbContext : DbContext
{
    private readonly IServiceScopeFactory _scopeFactory = scopeFactory;
    private readonly IOptionsMonitor<OutboxCleanupOptions> _optionsMonitor = optionsMonitor;
    private readonly ILogger<OutboxCleanupWorker<TDbContext>> _logger = logger;
    private readonly string _moduleName = moduleName;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var opts = _optionsMonitor.Get(_moduleName);
            try
            {
                var archived = await ArchiveBatchAsync(opts, stoppingToken);
                if (archived > 0)
                {
                    _logger.LogInformation(
                        "Archived {Count} processed outbox message(s) to history in module {Module}",
                        archived, _moduleName);
                }
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Outbox history archival failed for module {Module}", _moduleName);
            }

            try
            {
                await Task.Delay(opts.Interval, stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
        }
    }

    private async Task<int> ArchiveBatchAsync(OutboxCleanupOptions opts, CancellationToken ct)
    {
        await using var scope = _scopeFactory.CreateAsyncScope();
        var db = scope.ServiceProvider.GetRequiredService<TDbContext>();

        var outboxEntityType = db.Model.FindEntityType(typeof(OutboxMessage))
            ?? throw new InvalidOperationException($"{typeof(TDbContext).Name} does not map OutboxMessage.");
        var historyEntityType = db.Model.FindEntityType(typeof(OutboxHistory))
            ?? throw new InvalidOperationException(
                $"{typeof(TDbContext).Name} does not map OutboxHistory. Did AuditableUnitOfWorkDbContext " +
                "get updated to register it alongside OutboxMessage?");

        var qualifiedOutbox = LeasedRowClaimer.QualifyTable(outboxEntityType);
        var qualifiedHistory = LeasedRowClaimer.QualifyTable(historyEntityType);

        // {0}/{1} below are ExecuteSqlRawAsync's own positional placeholders, not C# string
        // interpolation — kept in plain (non-$) literals so they never collide with the
        // genuine C# interpolation used for the schema-qualified table names.
        var sql =
            $"WITH moved AS (" +
            $"    DELETE FROM {qualifiedOutbox} " +
            "     WHERE \"Id\" IN (" +
            $"         SELECT \"Id\" FROM {qualifiedOutbox} " +
            "          WHERE \"ProcessedAtUtc\" IS NOT NULL " +
            "          ORDER BY \"ProcessedAtUtc\" " +
            "          LIMIT {0} " +
            "          FOR UPDATE SKIP LOCKED" +
            "     )" +
            "     RETURNING \"Id\", \"AggregateId\", \"OrganizationId\", \"TenantId\", \"TraceId\", " +
            "               \"CorrelationId\", \"Headers\", \"Type\", \"Content\", \"OccurredAtUtc\", " +
            "               \"RetryCount\", \"Error\", \"ProcessedAtUtc\"" +
            ") " +
            $"INSERT INTO {qualifiedHistory} " +
            "    (\"Id\", \"AggregateId\", \"OrganizationId\", \"TenantId\", \"TraceId\", " +
            "     \"CorrelationId\", \"Headers\", \"Type\", \"Content\", \"OccurredAtUtc\", " +
            "     \"RetryCount\", \"Error\", \"ProcessedAtUtc\", \"ArchivedAtUtc\") " +
            "SELECT \"Id\", \"AggregateId\", \"OrganizationId\", \"TenantId\", \"TraceId\", " +
            "       \"CorrelationId\", \"Headers\", \"Type\", \"Content\", \"OccurredAtUtc\", " +
            "       \"RetryCount\", \"Error\", \"ProcessedAtUtc\", {1} " +
            "FROM moved";

        return await db.Database.ExecuteSqlRawAsync(sql, new object[] { opts.BatchSize, DateTime.UtcNow }, ct);
    }
}
