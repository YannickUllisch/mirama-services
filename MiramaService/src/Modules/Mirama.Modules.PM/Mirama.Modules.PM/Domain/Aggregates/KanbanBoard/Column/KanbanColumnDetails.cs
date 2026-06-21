namespace Mirama.Modules.PM.Domain.Aggregates.KanbanBoard.Column;

public sealed record KanbanColumnDetails(
    Guid StatusId,
    string Name,
    int? WipLimit = null,
    string? Color = null);
