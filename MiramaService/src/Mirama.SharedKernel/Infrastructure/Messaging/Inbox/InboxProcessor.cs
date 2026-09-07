
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Abstractions.Domain.Events;
using Mirama.SharedKernel.Abstractions.Persistence;
using Npgsql;

namespace Mirama.SharedKernel.Infrastructure.Messaging.Inbox;

public sealed class InboxProcessor<TDbContext> : BackgroundService
    where TDbContext : DbContext
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IOptionsMonitor<InboxOptions> _optionsMonitor;
    private readonly ILogger<InboxProcessor<TDbContext>> _logger;
    private readonly string _moduleName;
    private readonly string _instanceId = Guid.NewGuid().ToString("N");

    public InboxProcessor(
        IServiceScopeFactory scopeFactory,
        IOptionsMonitor<InboxOptions> optionsMonitor,
        ILogger<InboxProcessor<TDbContext>> logger,
        string moduleName)
    {
        _scopeFactory = scopeFactory;
        _optionsMonitor = optionsMonitor;
        _logger = logger;
        _moduleName = moduleName;
    }

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
                _logger.LogError(ex, "Inbox poll failed for module {Module}", _moduleName);
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

    private async Task<bool> ProcessBatchAsync(InboxOptions opts, CancellationToken ct)
    {
        List<ClaimedItem> claimed;
        await using (var scope = _scopeFactory.CreateAsyncScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<TDbContext>();
            claimed = await ClaimBatchAsync(db, opts, ct);
        }

        if (claimed.Count == 0) return false;

        var orderedGroups = claimed.Where(m => m.RequiresOrdering).GroupBy(m => m.AggregateId ?? m.Id);
        var unordered = claimed.Where(m => !m.RequiresOrdering).ToList();

        var tasks = new List<Task>();

        foreach (var group in orderedGroups)
        {
            var sequence = group.OrderBy(m => m.OccurredAtUtc).ToList();
            tasks.Add(ProcessSequentiallyAsync(sequence, opts, ct));
        }

        if (unordered.Count > 0)
        {
            tasks.Add(Parallel.ForEachAsync(
                unordered,
                new ParallelOptions { MaxDegreeOfParallelism = opts.MaxDegreeOfParallelism, CancellationToken = ct },
                async (item, token) => await ProcessOneAsync(item.Id, opts, token)));
        }

        await Task.WhenAll(tasks);
        return true;
    }

    private async Task ProcessSequentiallyAsync(List<ClaimedItem> items, InboxOptions opts, CancellationToken ct)
    {
        foreach (var item in items)
        {
            await ProcessOneAsync(item.Id, opts, ct);
        }
    }

    private async Task ProcessOneAsync(Guid itemId, InboxOptions opts, CancellationToken ct)
    {
        await using var scope = _scopeFactory.CreateAsyncScope();
        var db = scope.ServiceProvider.GetRequiredService<TDbContext>();

        var item = await db.Set<InboxMessage>().FirstOrDefaultAsync(i => i.Id == itemId, ct);
        if (item is null) return; // already handled via another path; nothing to do

        scope.ServiceProvider.GetRequiredService<IAmbientMessageContext>()
            .Set(item.OrganizationId, item.TenantId, item.TraceId, item.CorrelationId, item.Headers);

        Type clrType;
        object @event;
        object handler;
        try
        {
            clrType = Type.GetType(item.Type, throwOnError: true)!;
            @event = JsonSerializer.Deserialize(item.Content, clrType)
                     ?? throw new InvalidOperationException($"Deserializing inbox item {item.Id} produced null.");

            var handlerInterface = typeof(INotificationHandler<>).MakeGenericType(clrType);
            handler = scope.ServiceProvider.GetServices(handlerInterface)
                .FirstOrDefault(h => h is not null && h.GetType().FullName == item.HandlerName)
                ?? throw new InvalidOperationException(
                    $"No registered handler '{item.HandlerName}' found for {clrType.Name} in module {_moduleName}. " +
                    "It may have been renamed or removed after this item was fanned out here.");
        }
        catch (Exception ex)
        {
            await FailAsync(db, item, opts, ex, ct);
            return;
        }

        try
        {
            dynamic h = handler;
            await h.HandleAsync((dynamic)@event, ct);
            db.Remove(item);
            await db.SaveChangesAsync(ct);
        }
        catch (Exception ex)
        {
            await FailAsync(db, item, opts, ex, ct);
        }
    }

    private async Task FailAsync(TDbContext db, InboxMessage item, InboxOptions opts, Exception ex, CancellationToken ct)
    {
        item.RetryCount++;
        item.LockedUntilUtc = null;
        item.Error = ex.ToString();
        item.AvailableAtUtc = DateTime.UtcNow + opts.RetryBackoff(item.RetryCount);

        if (item.RetryCount >= opts.MaxRetries)
        {
            db.Add(new InboxDeadLetter
            {
                Id = Guid.NewGuid(),
                AggregateId = item.AggregateId,
                OrganizationId = item.OrganizationId,
                TenantId = item.TenantId,
                TraceId = item.TraceId,
                CorrelationId = item.CorrelationId,
                Headers = item.Headers,
                Type = item.Type,
                Content = item.Content,
                HandlerName = item.HandlerName,
                RetryCount = item.RetryCount,
                Error = item.Error,
                DeadLetteredAtUtc = DateTime.UtcNow,
            });
            db.Remove(item);
            _logger.LogError(
                "Inbox item {ItemId} (handler {Handler}) dead-lettered in module {Module} after {Retries} attempts: {Error}",
                item.Id, item.HandlerName, _moduleName, item.RetryCount, ex.Message);
        }
        else
        {
            _logger.LogError(ex,
                "Handler {Handler} failed for inbox item {ItemId} in module {Module}, attempt {Retry}",
                item.HandlerName, item.Id, _moduleName, item.RetryCount);
        }

        await db.SaveChangesAsync(ct);
    }

    private async Task<List<ClaimedItem>> ClaimBatchAsync(TDbContext db, InboxOptions opts, CancellationToken ct)
    {
        var entityType = db.Model.FindEntityType(typeof(InboxMessage))
            ?? throw new InvalidOperationException($"{typeof(TDbContext).Name} does not map InboxMessage.");
        var qualified = LeasedRowClaimer.QualifyTable(entityType);

        var conn = (NpgsqlConnection)db.Database.GetDbConnection();
        var ids = await LeasedRowClaimer.ClaimAsync(conn, qualified, opts.BatchSize, opts.LeaseDuration, _instanceId, ct);

        if (ids.Count == 0) return [];

        var rows = await db.Set<InboxMessage>()
            .AsNoTracking()
            .Where(i => ids.Contains(i.Id))
            .Select(i => new { i.Id, i.AggregateId, i.Type, i.OccurredAtUtc })
            .ToListAsync(ct);

        var claimed = new List<ClaimedItem>(rows.Count);
        foreach (var row in rows)
        {
            Type? clrType = null;
            try { clrType = Type.GetType(row.Type); } catch { /* resolved again, safely, in ProcessOneAsync */ }
            var requiresOrdering = clrType is not null && typeof(IOrderedIntegrationEvent).IsAssignableFrom(clrType);
            claimed.Add(new ClaimedItem(row.Id, row.AggregateId, row.OccurredAtUtc, requiresOrdering));
        }
        return claimed;
    }

    private sealed record ClaimedItem(Guid Id, Guid? AggregateId, DateTime OccurredAtUtc, bool RequiresOrdering);
}
