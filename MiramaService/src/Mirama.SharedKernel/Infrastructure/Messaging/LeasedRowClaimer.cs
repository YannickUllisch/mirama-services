
using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Npgsql;

namespace Mirama.SharedKernel.Infrastructure.Messaging;

/// <summary>
/// The claim step shared by OutboxProcessor and InboxProcessor: select eligible,
/// currently-unleased rows from a lease-based queue table with FOR UPDATE SKIP
/// LOCKED, lease them to this instance, and return their ids. OutboxMessage and
/// InboxMessage share the same Id/AvailableAtUtc/LockedUntilUtc/LockedBy shape, so
/// this SQL is identical regardless of which table or schema it targets — the
/// two processors differ only in what they do with a claimed id afterward.
/// </summary>
internal static class LeasedRowClaimer
{
    public static string QualifyTable(IEntityType entityType)
    {
        var schema = entityType.GetSchema();
        var table = entityType.GetTableName();
        return schema is null ? $"\"{table}\"" : $"\"{schema}\".\"{table}\"";
    }

    public static async Task<List<Guid>> ClaimAsync(
        NpgsqlConnection conn,
        string qualifiedTable,
        int batchSize,
        TimeSpan leaseDuration,
        string instanceId,
        CancellationToken ct)
    {
        var ownsConnection = conn.State != ConnectionState.Open;
        if (ownsConnection) await conn.OpenAsync(ct);

        var ids = new List<Guid>();
        var utcNow = DateTime.UtcNow;
        var lockUntil = utcNow.Add(leaseDuration);

        try
        {
            await using var tx = await conn.BeginTransactionAsync(ct);

            await using (var select = conn.CreateCommand())
            {
                select.Transaction = tx;
                // Params, not now()/CURRENT_TIMESTAMP, deliberately: keeps this in step
                // with whatever DateTime.Kind convention EF+Npgsql already uses for these
                // columns elsewhere in the codebase, rather than guessing at timestamp vs
                // timestamptz semantics for a raw SQL function call.
                select.CommandText = $"""
                    SELECT "Id" FROM {qualifiedTable}
                    WHERE "AvailableAtUtc" <= @now
                      AND ("LockedUntilUtc" IS NULL OR "LockedUntilUtc" < @now)
                    ORDER BY "OccurredAtUtc"
                    LIMIT @batchSize
                    FOR UPDATE SKIP LOCKED
                    """;
                select.Parameters.AddWithValue("now", utcNow);
                select.Parameters.AddWithValue("batchSize", batchSize);
                await using var reader = await select.ExecuteReaderAsync(ct);
                while (await reader.ReadAsync(ct)) ids.Add(reader.GetGuid(0));
            }

            if (ids.Count > 0)
            {
                await using var update = conn.CreateCommand();
                update.Transaction = tx;
                update.CommandText = $"""
                    UPDATE {qualifiedTable}
                    SET "LockedUntilUtc" = @lockedUntil, "LockedBy" = @lockedBy
                    WHERE "Id" = ANY(@ids)
                    """;
                update.Parameters.AddWithValue("lockedUntil", lockUntil);
                update.Parameters.AddWithValue("lockedBy", instanceId);
                update.Parameters.AddWithValue("ids", ids.ToArray());
                await update.ExecuteNonQueryAsync(ct);
            }

            await tx.CommitAsync(ct);
        }
        finally
        {
            if (ownsConnection) await conn.CloseAsync();
        }

        return ids;
    }
}
