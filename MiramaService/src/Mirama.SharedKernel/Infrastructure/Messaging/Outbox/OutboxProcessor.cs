
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Npgsql;

namespace Mirama.SharedKernel.Infrastructure.Messaging.Outbox;

public sealed class OutboxProcessor<TDbContext>(
    IServiceScopeFactory scopeFactory,
    IOptionsMonitor<OutboxOptions> optionsMonitor,
    IEventTypeResolver typeResolver,
    IModuleSchemaRegistry schemaRegistry,
    ILogger<OutboxProcessor<TDbContext>> logger,
    string moduleName) : BackgroundService
    where TDbContext : DbContext
{
    private readonly IServiceScopeFactory _scopeFactory = scopeFactory;
    private readonly IOptionsMonitor<OutboxOptions> _optionsMonitor = optionsMonitor;
    private readonly IEventTypeResolver _typeResolver = typeResolver;
    private readonly IModuleSchemaRegistry _schemaRegistry = schemaRegistry;
    private readonly ILogger<OutboxProcessor<TDbContext>> _logger = logger;
    private readonly string _moduleName = moduleName;
    private readonly string _instanceId = Guid.NewGuid().ToString("N");

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var opts = _optionsMonitor.Get(_moduleName);
            bool processedAny;
            try
            {
                processedAny = await ProcessBatchAsync(opts, stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Outbox fan-out poll failed for module {Module}", _moduleName);
                processedAny = false;
            }

            try
            {
                await Task.Delay(processedAny ? opts.BusyPollInterval : opts.IdlePollInterval, stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
        }
    }

    private async Task<bool> ProcessBatchAsync(OutboxOptions opts, CancellationToken ct)
    {
        List<Guid> claimedIds;
        await using (var scope = _scopeFactory.CreateAsyncScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<TDbContext>();
            var entityType = db.Model.FindEntityType(typeof(OutboxMessage))
                ?? throw new InvalidOperationException($"{typeof(TDbContext).Name} does not map OutboxMessage.");
            var qualified = LeasedRowClaimer.QualifyTable(entityType);
            var conn = (NpgsqlConnection)db.Database.GetDbConnection();
            claimedIds = await LeasedRowClaimer.ClaimAsync(
                conn, qualified, opts.BatchSize, opts.LeaseDuration, _instanceId, ct,
                additionalWhereClause: "\"ProcessedAtUtc\" IS NULL");
        }

        if (claimedIds.Count == 0) return false;

        await Parallel.ForEachAsync(
            claimedIds,
            new ParallelOptions { MaxDegreeOfParallelism = opts.MaxDegreeOfParallelism, CancellationToken = ct },
            async (id, token) => await FanOutOneAsync(id, opts, token));

        return true;
    }

    private async Task FanOutOneAsync(Guid messageId, OutboxOptions opts, CancellationToken ct)
    {
        Exception? failure = null;

        await using (var scope = _scopeFactory.CreateAsyncScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<TDbContext>();
            var message = await db.Set<OutboxMessage>().FirstOrDefaultAsync(m => m.Id == messageId, ct);
            if (message is null) return; // already handled via another path; nothing to do

            await using var transaction = await db.Database.BeginTransactionAsync(ct);
            try
            {
                var clrType = _typeResolver.Resolve(message.Type);
                var handlerInterface = typeof(INotificationHandler<>).MakeGenericType(clrType);
                var handlers = scope.ServiceProvider.GetServices(handlerInterface).Where(h => h is not null).ToList();

                foreach (var handler in handlers)
                {
                    var handlerType = handler!.GetType();
                    var schema = _schemaRegistry.ResolveSchema(handlerType.Assembly);

                    var insertSql =
                        $"INSERT INTO \"{schema}\".\"InboxMessages\" " +
                        "(\"Id\", \"OutboxMessageId\", \"AggregateId\", \"OrganizationId\", \"TenantId\", " +
                        "\"TraceId\", \"CorrelationId\", \"Headers\", \"Type\", \"Content\", \"HandlerName\", " +
                        "\"OccurredAtUtc\", \"AvailableAtUtc\", \"RetryCount\") " +
                        "VALUES ({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12}, {13})";

                    await db.Database.ExecuteSqlRawAsync(
                        insertSql,
                        new object[]
                        {
                            Guid.NewGuid(),
                            message.Id,
                            (object?)message.AggregateId ?? DBNull.Value,
                            (object?)message.OrganizationId ?? DBNull.Value,
                            (object?)message.TenantId ?? DBNull.Value,
                            (object?)message.TraceId ?? DBNull.Value,
                            (object?)message.CorrelationId ?? DBNull.Value,
                            (object?)message.Headers ?? DBNull.Value,
                            clrType.AssemblyQualifiedName!,
                            message.Content,
                            handlerType.FullName!,
                            message.OccurredAtUtc,
                            message.OccurredAtUtc,
                            0,
                        },
                        ct);
                }

                // Mark processed rather than delete: OutboxCleanupWorker archives it to
                // OutboxHistory later, on its own longer-interval schedule, keeping this
                // table's row count (and hence claim-query cost) small without losing the
                // record of what was published.
                message.ProcessedAtUtc = DateTime.UtcNow;
                await db.SaveChangesAsync(ct);
                await transaction.CommitAsync(ct);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(ct);
                failure = ex;
            }
        }

        if (failure is null) return; // fan-out succeeded

        await using var failureScope = _scopeFactory.CreateAsyncScope();
        var failureDb = failureScope.ServiceProvider.GetRequiredService<TDbContext>();
        await RecordFanOutFailureAsync(failureDb, messageId, opts, failure, ct);
    }

    private async Task RecordFanOutFailureAsync(TDbContext db, Guid messageId, OutboxOptions opts, Exception ex, CancellationToken ct)
    {
        var message = await db.Set<OutboxMessage>().FirstOrDefaultAsync(m => m.Id == messageId, ct);
        if (message is null) return; // handled via another path meanwhile

        message.RetryCount++;
        message.LockedUntilUtc = null;
        message.Error = ex.ToString();
        message.AvailableAtUtc = DateTime.UtcNow + opts.RetryBackoff(message.RetryCount);

        if (message.RetryCount >= opts.MaxRetries)
        {
            db.Add(new OutboxDeadLetter
            {
                Id = Guid.NewGuid(),
                AggregateId = message.AggregateId,
                OrganizationId = message.OrganizationId,
                TenantId = message.TenantId,
                TraceId = message.TraceId,
                CorrelationId = message.CorrelationId,
                Headers = message.Headers,
                Type = message.Type,
                Content = message.Content,
                RetryCount = message.RetryCount,
                Error = message.Error,
                DeadLetteredAtUtc = DateTime.UtcNow,
            });
            db.Remove(message);
            _logger.LogError(
                "Outbox message {MessageId} ({Type}) dead-lettered in module {Module} after {Retries} fan-out attempts: {Error}",
                message.Id, message.Type, _moduleName, message.RetryCount, ex.Message);
        }
        else
        {
            _logger.LogError(ex,
                "Fan-out failed for outbox message {MessageId} ({Type}) in module {Module}, attempt {Retry}",
                message.Id, message.Type, _moduleName, message.RetryCount);
        }

        await db.SaveChangesAsync(ct);
    }
}
