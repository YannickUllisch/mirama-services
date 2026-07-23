---
name: gortex-abstractions-permissions-5-dirs
description: "Work in the Abstractions/Permissions +5 dirs area — 43 symbols across 8 files (82% cohesion)"
---

# Abstractions/Permissions +5 dirs

43 symbols | 8 files | 82% cohesion

## When to Use

Use this skill when working on files in:
- `src/Mirama.SharedKernel/Abstractions/Permissions/IPermissionService.cs`
- `src/Mirama.SharedKernel/Abstractions/Permissions/IProjectRoleProvider.cs`
- `src/Mirama.SharedKernel/Models/Permissions/NullProjectRoleProvider.cs`
- `src/Mirama.SharedKernel/Models/Permissions/PermissionMatcher.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Common/PermissionCacheKeys.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Domain/Aggregates/Role/RoleId.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Infrastructure/Persistence/Configurations/Entities/MemberConfig.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Infrastructure/Services/PermissionService.cs`

## Key Files

| File | Symbols |
|------|---------|
| `src/Mirama.SharedKernel/Abstractions/Permissions/IPermissionService.cs` | IPermissionService |
| `src/Mirama.SharedKernel/Abstractions/Permissions/IProjectRoleProvider.cs` | userId, projectId, IProjectRoleProvider, GetProjectRoleIdAsync, ct |
| `src/Mirama.SharedKernel/Models/Permissions/NullProjectRoleProvider.cs` | ct, GetProjectRoleIdAsync, NullProjectRoleProvider, userId, projectId |
| `src/Mirama.SharedKernel/Models/Permissions/PermissionMatcher.cs` | required, PermissionMatcher, effective, IsGranted |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Common/PermissionCacheKeys.cs` | PermissionCacheKeys, roleId, RolePerms, orgId, MemberRoles, ... |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Domain/Aggregates/Role/RoleId.cs` | RoleId |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Infrastructure/Persistence/Configurations/Entities/MemberConfig.cs` | MemberConfiguration, builder, Configure |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Infrastructure/Services/PermissionService.cs` | ct, RoleCacheTtl, HasPermissionAsync, ct, UnionPermissionsAsync, ... |

## How to Explore

```
get_communities with id: "community-85"
smart_context with task: "understand Abstractions/Permissions +5 dirs", format: "gcx"
```

_`format: "gcx"` returns the [GCX1 compact wire format](../../docs/wire-format.md) — round-trippable, ~27% fewer tokens than JSON. Drop it for JSON output; agents using `@gortex/wire` or the Go `github.com/gortexhq/gcx-go` package decode either._
