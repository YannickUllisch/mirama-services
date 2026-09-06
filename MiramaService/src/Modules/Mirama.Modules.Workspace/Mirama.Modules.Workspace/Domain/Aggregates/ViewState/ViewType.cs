using System.Text.Json.Serialization;

namespace Mirama.Modules.Workspace.Domain.Aggregates.ViewState;

/// <summary>
/// Discriminates the shape callers should expect in <see cref="ViewState.StateJson"/>.
/// Adding a new customizable surface (another table, another board layout) means adding
/// a new value here plus a typed DTO validated at the API boundary - never a migration.
/// </summary>
// No global JsonStringEnumConverter is registered for the API (this is the only enum bound
// directly from a request body), so without this attribute ASP.NET's model binder rejects
// any request whose JSON carries "Sidebar" etc. as a string instead of the numeric value.
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ViewType
{
    Sidebar,
    Table,
    KanbanBoard,
    Gantt
}
