---
name: gortex-common-interfaces-10-dirs
description: "Work in the Common/Interfaces +10 dirs area — 62 symbols across 17 files (83% cohesion)"
---

# Common/Interfaces +10 dirs

62 symbols | 17 files | 83% cohesion

## When to Use

Use this skill when working on files in:
- `src/Mirama.SharedKernel/Abstractions/Common/Interfaces/IModuleMigrator.cs`
- `src/Mirama.SharedKernel/Abstractions/Common/Interfaces/IModuleService.cs`
- `src/Mirama.SharedKernel/Infrastructure/Options/InfrastructureOptions.cs`
- `src/Modules/Mirama.Modules.Clients/Mirama.Modules.Clients/Infrastructure/Persistence/Repositories/ClientsQueryRepository.cs`
- `src/Modules/Mirama.Modules.Clients/Mirama.Modules.Clients/Infrastructure/Persistence/Repositories/IClientsQueryRepository.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Interfaces/IIdentityCommandRepository.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Interfaces/IIdentityQueryRepository.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Infrastructure/ConfigureServices.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Infrastructure/Persistence/IdentityModuleMigrator.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Infrastructure/Persistence/Repositories/IdentityCommandRepository.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Infrastructure/Persistence/Repositories/IdentityQueryRepository.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Common/Interfaces/IPMCommandRepository.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Common/Interfaces/IPMQueryRepository.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Infrastructure/ConfigureServices.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Infrastructure/Persistence/PMModuleMigrator.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Infrastructure/Persistence/Repositories/PMCommandRepository.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Infrastructure/Persistence/Repositories/PMQueryRepository.cs`

## Key Files

| File | Symbols |
|------|---------|
| `src/Mirama.SharedKernel/Abstractions/Common/Interfaces/IModuleMigrator.cs` | ModuleName, IModuleMigrator |
| `src/Mirama.SharedKernel/Abstractions/Common/Interfaces/IModuleService.cs` | IModuleService |
| `src/Mirama.SharedKernel/Infrastructure/Options/InfrastructureOptions.cs` | Key, InfrastructureOptions, DatabaseConnection |
| `src/Modules/Mirama.Modules.Clients/Mirama.Modules.Clients/Infrastructure/Persistence/Repositories/ClientsQueryRepository.cs` | TEntity, TId, Query, ClientsQueryRepository |
| `src/Modules/Mirama.Modules.Clients/Mirama.Modules.Clients/Infrastructure/Persistence/Repositories/IClientsQueryRepository.cs` | TEntity, TId, Query, IClientsQueryRepository |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Interfaces/IIdentityCommandRepository.cs` | T, TID, Query, IIdentityCommandRepository |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Interfaces/IIdentityQueryRepository.cs` | TID, IIdentityQueryRepository, Query, T |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Infrastructure/ConfigureServices.cs` | AddInfrastructure, services, config |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Infrastructure/Persistence/IdentityModuleMigrator.cs` | _db, IdentityModuleMigrator, db, IdentityModuleMigrator.<init>, ModuleName |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Infrastructure/Persistence/Repositories/IdentityCommandRepository.cs` | Query, _dbContext, T, TID, IdentityCommandRepository |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Infrastructure/Persistence/Repositories/IdentityQueryRepository.cs` | T, Query, TID, IdentityQueryRepository, _dbContext |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Common/Interfaces/IPMCommandRepository.cs` | IPMCommandRepository, TID, Query, T |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Common/Interfaces/IPMQueryRepository.cs` | IPMQueryRepository, T, Query, TID |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Infrastructure/ConfigureServices.cs` | AddInfrastructure, config, services |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Infrastructure/Persistence/PMModuleMigrator.cs` | _db, ModuleName, PMModuleMigrator |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Infrastructure/Persistence/Repositories/PMCommandRepository.cs` | Query, TID, PMCommandRepository, T |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Infrastructure/Persistence/Repositories/PMQueryRepository.cs` | T, TID, PMQueryRepository, Query |

## How to Explore

```
get_communities with id: "community-99"
smart_context with task: "understand Common/Interfaces +10 dirs", format: "gcx"
```

_`format: "gcx"` returns the [GCX1 compact wire format](../../docs/wire-format.md) — round-trippable, ~27% fewer tokens than JSON. Drop it for JSON output; agents using `@gortex/wire` or the Go `github.com/gortexhq/gcx-go` package decode either._
