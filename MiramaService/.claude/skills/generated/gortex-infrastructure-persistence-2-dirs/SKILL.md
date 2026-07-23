---
name: gortex-infrastructure-persistence-2-dirs
description: "Work in the Infrastructure/Persistence +2 dirs area — 32 symbols across 4 files (73% cohesion)"
---

# Infrastructure/Persistence +2 dirs

32 symbols | 4 files | 73% cohesion

## When to Use

Use this skill when working on files in:
- `src/Mirama.SharedKernel/Abstractions/Persistence/IRequestContextProvider.cs`
- `src/Mirama.SharedKernel/Infrastructure/Services/RequestContextProvider.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Infrastructure/Persistence/PMDbContext.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Infrastructure/Persistence/PMDbContextFactory.cs`

## Key Files

| File | Symbols |
|------|---------|
| `src/Mirama.SharedKernel/Abstractions/Persistence/IRequestContextProvider.cs` | TenantId, IRequestContextProvider, ProjectId, UserId, OrganizationId |
| `src/Mirama.SharedKernel/Infrastructure/Services/RequestContextProvider.cs` | _httpContextAccessor, TenantId, RequestContextProvider, OrganizationId, ExternalUserId, ... |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Infrastructure/Persistence/PMDbContext.cs` | _contextProvider, PMDbContext, PMDbContext.<init>, _dispatcher, cancellationToken, ... |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Infrastructure/Persistence/PMDbContextFactory.cs` | args, PMDbContextFactory, CreateDbContext |

## How to Explore

```
get_communities with id: "community-147"
smart_context with task: "understand Infrastructure/Persistence +2 dirs", format: "gcx"
```

_`format: "gcx"` returns the [GCX1 compact wire format](../../docs/wire-format.md) — round-trippable, ~27% fewer tokens than JSON. Drop it for JSON output; agents using `@gortex/wire` or the Go `github.com/gortexhq/gcx-go` package decode either._
