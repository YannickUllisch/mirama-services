using ErrorOr;
using Mirama.Modules.PM.Domain.Aggregates.KanbanBoard.Column;
using Mirama.SharedKernel.Abstractions.Domain.Core;

namespace Mirama.Modules.PM.Domain.Aggregates.KanbanBoard;

public sealed class KanbanBoard : OrganizationAggregateRoot<KanbanBoardId>
{
    public Guid ProjectId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public bool IsDefault { get; private set; }
    public BoardGroupBy? GroupBy { get; private set; }

    public List<KanbanColumn> Columns { get; private set; } = [];

    private KanbanBoard(KanbanBoardDetails details)
    {
        this.ProjectId = details.ProjectId;
        this.Name = details.Name.Trim();
        this.Description = details.Description?.Trim();
        this.GroupBy = details.GroupBy;
        this.IsDefault = false;
    }

    private KanbanBoard() { }

    public static KanbanBoard Create(KanbanBoardDetails details) =>
        new KanbanBoard(details) { Id = new KanbanBoardId(Guid.NewGuid()) };

    public void Update(KanbanBoardDetails details)
    {
        this.Name = details.Name.Trim();
        this.Description = details.Description?.Trim();
        this.GroupBy = details.GroupBy;
    }

    internal void SetDefault(bool isDefault) => this.IsDefault = isDefault;

    public ErrorOr<KanbanColumn> AddColumn(KanbanColumnDetails details)
    {
        if (this.Columns.Any(c => c.StatusId == details.StatusId))
            return Error.Conflict("KanbanBoard.Column.Duplicate", "A column for this status already exists.");
        var column = KanbanColumn.Create(details, this.Columns.Count);
        this.Columns.Add(column);
        return column;
    }

    public ErrorOr<Deleted> RemoveColumn(KanbanColumnId id)
    {
        var column = this.Columns.Find(c => c.Id == id);
        if (column is null)
            return Error.NotFound("KanbanBoard.Column.NotFound", "Column not found.");
        this.Columns.Remove(column);
        return Result.Deleted;
    }

    public ErrorOr<Success> UpdateColumn(KanbanColumnId id, KanbanColumnDetails details)
    {
        var column = this.Columns.Find(c => c.Id == id);
        if (column is null)
            return Error.NotFound("KanbanBoard.Column.NotFound", "Column not found.");
        if (this.Columns.Any(c => c.Id != id && c.StatusId == details.StatusId))
            return Error.Conflict("KanbanBoard.Column.Duplicate", "A column for this status already exists.");
        column.Update(details);
        return Result.Success;
    }

    public ErrorOr<Success> ReorderColumns(IReadOnlyList<KanbanColumnId> orderedIds)
    {
        if (orderedIds.Count != this.Columns.Count)
            return Error.Validation("KanbanBoard.Column.Reorder", "Ordered list must include every column exactly once.");
        for (var i = 0; i < orderedIds.Count; i++)
        {
            var column = this.Columns.Find(c => c.Id == orderedIds[i]);
            if (column is null)
                return Error.NotFound("KanbanBoard.Column.NotFound", $"Column {orderedIds[i].Value} not found.");
            column.SetPosition(i);
        }
        return Result.Success;
    }

    public ErrorOr<Success> SetWipLimit(KanbanColumnId id, int? limit)
    {
        if (limit is < 1)
            return Error.Validation("KanbanBoard.Column.WipLimit", "WIP limit must be at least 1.");
        var column = this.Columns.Find(c => c.Id == id);
        if (column is null)
            return Error.NotFound("KanbanBoard.Column.NotFound", "Column not found.");
        column.SetWipLimit(limit);
        return Result.Success;
    }

    public ErrorOr<Success> CollapseColumn(KanbanColumnId id)
    {
        var column = this.Columns.Find(c => c.Id == id);
        if (column is null)
            return Error.NotFound("KanbanBoard.Column.NotFound", "Column not found.");
        column.SetCollapsed(true);
        return Result.Success;
    }

    public ErrorOr<Success> ExpandColumn(KanbanColumnId id)
    {
        var column = this.Columns.Find(c => c.Id == id);
        if (column is null)
            return Error.NotFound("KanbanBoard.Column.NotFound", "Column not found.");
        column.SetCollapsed(false);
        return Result.Success;
    }
}
