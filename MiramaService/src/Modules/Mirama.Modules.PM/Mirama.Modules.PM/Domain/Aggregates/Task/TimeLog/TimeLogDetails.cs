namespace Mirama.Modules.PM.Domain.Aggregates.Task.TimeLog;

public sealed record TimeLogDetails(
    Guid MemberId,
    int Minutes,
    DateOnly WorkedOn,
    string? Description = null);
