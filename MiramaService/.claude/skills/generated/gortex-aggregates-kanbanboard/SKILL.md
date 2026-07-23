---
name: gortex-aggregates-kanbanboard
description: "Work in the Aggregates/KanbanBoard area — 44 symbols across 6 files (94% cohesion)"
---

# Aggregates/KanbanBoard

44 symbols | 6 files | 94% cohesion

## When to Use

Use this skill when working on files in:
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Domain/Aggregates/KanbanBoard/BoardGroupBy.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Domain/Aggregates/KanbanBoard/Column/KanbanColumn.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Domain/Aggregates/KanbanBoard/Column/KanbanColumnId.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Domain/Aggregates/KanbanBoard/KanbanBoard.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Domain/Aggregates/KanbanBoard/KanbanBoardDetails.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Domain/Aggregates/KanbanBoard/KanbanBoardId.cs`

## Key Files

| File | Symbols |
|------|---------|
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Domain/Aggregates/KanbanBoard/BoardGroupBy.cs` | Assignee, Priority, Milestone, TaskType, BoardGroupBy |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Domain/Aggregates/KanbanBoard/Column/KanbanColumn.cs` | position, SetCollapsed, SetPosition, collapsed |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Domain/Aggregates/KanbanBoard/Column/KanbanColumnId.cs` | KanbanColumnId |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Domain/Aggregates/KanbanBoard/KanbanBoard.cs` | id, details, KanbanBoard.<init>, GroupBy, ExpandColumn, ... |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Domain/Aggregates/KanbanBoard/KanbanBoardDetails.cs` | KanbanBoardDetails |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Domain/Aggregates/KanbanBoard/KanbanBoardId.cs` | KanbanBoardId |

## How to Explore

```
get_communities with id: "community-129"
smart_context with task: "understand Aggregates/KanbanBoard", format: "gcx"
```

_`format: "gcx"` returns the [GCX1 compact wire format](../../docs/wire-format.md) — round-trippable, ~27% fewer tokens than JSON. Drop it for JSON output; agents using `@gortex/wire` or the Go `github.com/gortexhq/gcx-go` package decode either._
