namespace Mirama.Modules.Workspace.Domain.Aggregates.ViewState;

/// <summary>
/// Discriminates the shape callers should expect in <see cref="ViewState.StateJson"/>.
/// Adding a new customizable surface (another table, another board layout) means adding
/// a new value here plus a typed DTO validated at the API boundary - never a migration.
/// </summary>
public enum ViewType
{
    Sidebar,
    Table,
    KanbanBoard,
    Gantt
}
