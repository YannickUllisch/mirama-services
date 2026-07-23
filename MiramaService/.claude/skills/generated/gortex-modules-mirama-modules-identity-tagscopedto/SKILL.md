---
name: gortex-modules-mirama-modules-identity-tagscopedto
description: "Work in the Modules/Mirama.Modules.Identity · TagScopeDto area — 42 symbols across 6 files (89% cohesion)"
---

# Modules/Mirama.Modules.Identity · TagScopeDto

42 symbols | 6 files | 89% cohesion

## When to Use

Use this skill when working on files in:
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity.Contracts/Tags/ITagService.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity.Contracts/Tags/TagDto.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity.Contracts/Tags/TagScopeDto.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Domain/Aggregates/Organization/Tag/TagId.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Domain/Aggregates/Organization/Tag/TagScope.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Infrastructure/Services/Tags/TagService.cs`

## Key Files

| File | Symbols |
|------|---------|
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity.Contracts/Tags/ITagService.cs` | GetTagByIdAsync, tagIds, ct, GetTagsAsync, organizationId, ... |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity.Contracts/Tags/TagDto.cs` | TagDto |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity.Contracts/Tags/TagScopeDto.cs` | None, Task, Project, Client, Asset, ... |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Domain/Aggregates/Organization/Tag/TagId.cs` | TagId |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Domain/Aggregates/Organization/Tag/TagScope.cs` | Task, Project, General, Asset, TagScope, ... |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Infrastructure/Services/Tags/TagService.cs` | tagIds, GetTagByIdAsync, ct, tagId, GetTagsByIdsAsync, ... |

## How to Explore

```
get_communities with id: "community-33"
smart_context with task: "understand Modules/Mirama.Modules.Identity · TagScopeDto", format: "gcx"
```

_`format: "gcx"` returns the [GCX1 compact wire format](../../docs/wire-format.md) — round-trippable, ~27% fewer tokens than JSON. Drop it for JSON output; agents using `@gortex/wire` or the Go `github.com/gortexhq/gcx-go` package decode either._
