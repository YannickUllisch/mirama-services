namespace Mirama.Modules.PM.Domain.Aggregates.Project;

public sealed record ProjectDetails(
    string Name,
    DateTime StartDate,
    Guid StatusId,
    Guid PriorityId,
    string? Description = null,
    DateTime? EndDate = null,
    int Budget = 0);
