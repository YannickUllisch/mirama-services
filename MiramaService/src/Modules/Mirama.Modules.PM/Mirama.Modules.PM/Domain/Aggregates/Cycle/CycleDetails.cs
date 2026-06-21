namespace Mirama.Modules.PM.Domain.Aggregates.Cycle;

public sealed record CycleDetails(
    string Name,
    Guid ProjectId,
    string? Goal = null,
    DateTime? StartDate = null,
    DateTime? EndDate = null);
