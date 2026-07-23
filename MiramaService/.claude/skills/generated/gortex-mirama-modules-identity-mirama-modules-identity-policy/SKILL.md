---
name: gortex-mirama-modules-identity-mirama-modules-identity-policy
description: "Work in the Mirama.Modules.Identity/Mirama.Modules.Identity · Policy area — 32 symbols across 5 files (80% cohesion)"
---

# Mirama.Modules.Identity/Mirama.Modules.Identity · Policy

32 symbols | 5 files | 80% cohesion

## When to Use

Use this skill when working on files in:
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/Policies/PolicyResponse.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Domain/Aggregates/Policy/Policy.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Domain/Aggregates/Policy/PolicyDetails.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Infrastructure/Persistence/Configurations/AggregateRoots/PolicyConfig.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Infrastructure/Persistence/Seeding/PolicySeed.cs`

## Key Files

| File | Symbols |
|------|---------|
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/Policies/PolicyResponse.cs` | PolicyStatementResponse, PolicyMapper, MapResponse, policy |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Domain/Aggregates/Policy/Policy.cs` | details, Policy.<init>, action, Scope, resource, ... |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Domain/Aggregates/Policy/PolicyDetails.cs` | PolicyDetails |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Infrastructure/Persistence/Configurations/AggregateRoots/PolicyConfig.cs` | builder, PolicyConfiguration, Configure |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Infrastructure/Persistence/Seeding/PolicySeed.cs` | PolicySeed, dbContext, SeedPolicy, Seeds, SeedDataAsync |

## How to Explore

```
get_communities with id: "community-67"
smart_context with task: "understand Mirama.Modules.Identity/Mirama.Modules.Identity · Policy", format: "gcx"
```

_`format: "gcx"` returns the [GCX1 compact wire format](../../docs/wire-format.md) — round-trippable, ~27% fewer tokens than JSON. Drop it for JSON output; agents using `@gortex/wire` or the Go `github.com/gortexhq/gcx-go` package decode either._
