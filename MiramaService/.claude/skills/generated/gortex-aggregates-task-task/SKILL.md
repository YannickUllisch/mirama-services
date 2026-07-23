---
name: gortex-aggregates-task-task
description: "Work in the Aggregates/Task · Task area — 88 symbols across 8 files (94% cohesion)"
---

# Aggregates/Task · Task

88 symbols | 8 files | 94% cohesion

## When to Use

Use this skill when working on files in:
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Domain/Aggregates/Task/Dependency/DependencyType.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Domain/Aggregates/Task/Dependency/TaskDependency.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Domain/Aggregates/Task/Dependency/TaskDependencyDetails.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Domain/Aggregates/Task/Dependency/TaskDependencyId.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Domain/Aggregates/Task/Task.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Domain/Aggregates/Task/TaskDetails.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Domain/Aggregates/Task/TaskId.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Domain/Aggregates/Task/TaskType.cs`

## Key Files

| File | Symbols |
|------|---------|
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Domain/Aggregates/Task/Dependency/DependencyType.cs` | StartToStart, StartToFinish, FinishToStart, DependencyType, FinishToFinish |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Domain/Aggregates/Task/Dependency/TaskDependency.cs` | TaskDependency.<init>, Type, TaskDependency, details, details, ... |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Domain/Aggregates/Task/Dependency/TaskDependencyDetails.cs` | TaskDependencyDetails |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Domain/Aggregates/Task/Dependency/TaskDependencyId.cs` | TaskDependencyId |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Domain/Aggregates/Task/Task.cs` | details, ProjectId, parentId, memberId, EstimatedHours, ... |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Domain/Aggregates/Task/TaskDetails.cs` | TaskDetails |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Domain/Aggregates/Task/TaskId.cs` | TaskId |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Domain/Aggregates/Task/TaskType.cs` | Epic, Story, Issue, TaskType, Task, ... |

## How to Explore

```
get_communities with id: "community-140"
smart_context with task: "understand Aggregates/Task · Task", format: "gcx"
```

_`format: "gcx"` returns the [GCX1 compact wire format](../../docs/wire-format.md) — round-trippable, ~27% fewer tokens than JSON. Drop it for JSON output; agents using `@gortex/wire` or the Go `github.com/gortexhq/gcx-go` package decode either._
