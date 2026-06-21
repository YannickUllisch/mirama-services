namespace Mirama.Modules.PM.Domain.Aggregates.KanbanBoard;

public sealed record KanbanBoardDetails(
    string Name,
    Guid ProjectId,
    string? Description = null,
    BoardGroupBy? GroupBy = null);
