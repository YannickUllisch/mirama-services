---
name: gortex-aggregates-asset-asset
description: "Work in the Aggregates/Asset · Asset area — 67 symbols across 9 files (89% cohesion)"
---

# Aggregates/Asset · Asset

67 symbols | 9 files | 89% cohesion

## When to Use

Use this skill when working on files in:
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Domain/Aggregates/Asset/Asset.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Domain/Aggregates/Asset/AssetDetails.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Domain/Aggregates/Asset/AssetId.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Domain/Aggregates/Asset/AssetType.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Domain/Aggregates/Asset/Connection/AssetConnection.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Domain/Aggregates/Asset/Connection/AssetConnectionDetails.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Domain/Aggregates/Asset/Connection/AssetConnectionId.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Domain/Aggregates/Asset/Connection/AssetConnectionTarget.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Domain/Aggregates/Asset/Version/AssetVersionId.cs`

## Key Files

| File | Symbols |
|------|---------|
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Domain/Aggregates/Asset/Asset.cs` | ProjectId, Update, SetCurrentVersion, RemoveConnection, AddConnection, ... |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Domain/Aggregates/Asset/AssetDetails.cs` | AssetDetails |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Domain/Aggregates/Asset/AssetId.cs` | AssetId |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Domain/Aggregates/Asset/AssetType.cs` | Font, Other, Archive, AssetType, SourceFile, ... |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Domain/Aggregates/Asset/Connection/AssetConnection.cs` | AssetConnection.<init>, AssetConnection.<init>, AddedByMemberId, Create, details, ... |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Domain/Aggregates/Asset/Connection/AssetConnectionDetails.cs` | AssetConnectionDetails |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Domain/Aggregates/Asset/Connection/AssetConnectionId.cs` | AssetConnectionId |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Domain/Aggregates/Asset/Connection/AssetConnectionTarget.cs` | Task, Milestone, AssetConnectionTarget, Project, Cycle |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Domain/Aggregates/Asset/Version/AssetVersionId.cs` | AssetVersionId |

## How to Explore

```
get_communities with id: "community-118"
smart_context with task: "understand Aggregates/Asset · Asset", format: "gcx"
```

_`format: "gcx"` returns the [GCX1 compact wire format](../../docs/wire-format.md) — round-trippable, ~27% fewer tokens than JSON. Drop it for JSON output; agents using `@gortex/wire` or the Go `github.com/gortexhq/gcx-go` package decode either._
