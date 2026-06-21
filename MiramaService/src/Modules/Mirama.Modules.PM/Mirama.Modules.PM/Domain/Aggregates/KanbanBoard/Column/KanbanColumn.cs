using Mirama.SharedKernel.Abstractions.Domain.Core;

namespace Mirama.Modules.PM.Domain.Aggregates.KanbanBoard.Column;

public sealed class KanbanColumn : Entity<KanbanColumnId>
{
    public Guid StatusId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public int Position { get; private set; }
    public int? WipLimit { get; private set; }
    public string? Color { get; private set; }
    public bool IsCollapsed { get; private set; }

    private KanbanColumn(KanbanColumnDetails details, int position)
    {
        this.StatusId = details.StatusId;
        this.Name = details.Name.Trim();
        this.Position = position;
        this.WipLimit = details.WipLimit;
        this.Color = details.Color?.Trim();
        this.IsCollapsed = false;
    }

    private KanbanColumn() { }

    internal static KanbanColumn Create(KanbanColumnDetails details, int position) =>
        new KanbanColumn(details, position) { Id = new KanbanColumnId(Guid.NewGuid()) };

    internal void Update(KanbanColumnDetails details)
    {
        this.Name = details.Name.Trim();
        this.WipLimit = details.WipLimit;
        this.Color = details.Color?.Trim();
    }

    internal void SetPosition(int position) => this.Position = position;
    internal void SetWipLimit(int? limit) => this.WipLimit = limit;
    internal void SetCollapsed(bool collapsed) => this.IsCollapsed = collapsed;
}
