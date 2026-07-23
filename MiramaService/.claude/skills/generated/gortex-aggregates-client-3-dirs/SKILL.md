---
name: gortex-aggregates-client-3-dirs
description: "Work in the Aggregates/Client +3 dirs area — 36 symbols across 6 files (83% cohesion)"
---

# Aggregates/Client +3 dirs

36 symbols | 6 files | 83% cohesion

## When to Use

Use this skill when working on files in:
- `src/Mirama.SharedKernel/Abstractions/Domain/Core/AggregateRoot.cs`
- `src/Modules/Mirama.Modules.Clients/Mirama.Modules.Clients/Domain/Aggregates/Client/Client.cs`
- `src/Modules/Mirama.Modules.Clients/Mirama.Modules.Clients/Domain/Aggregates/Client/ClientDetails.cs`
- `src/Modules/Mirama.Modules.Clients/Mirama.Modules.Clients/Domain/Enums/ClientStatus.cs`
- `src/Modules/Mirama.Modules.Clients/Mirama.Modules.Clients/Domain/Enums/ClientType.cs`
- `src/Modules/Mirama.Modules.Clients/Mirama.Modules.Clients/Infrastructure/Persistence/Configurations/ClientConfiguration.cs`

## Key Files

| File | Symbols |
|------|---------|
| `src/Mirama.SharedKernel/Abstractions/Domain/Core/AggregateRoot.cs` | @event, AddDomainEvent |
| `src/Modules/Mirama.Modules.Clients/Mirama.Modules.Clients/Domain/Aggregates/Client/Client.cs` | Website, Name, PortalUsers, Contacts, Industry, ... |
| `src/Modules/Mirama.Modules.Clients/Mirama.Modules.Clients/Domain/Aggregates/Client/ClientDetails.cs` | ClientDetails |
| `src/Modules/Mirama.Modules.Clients/Mirama.Modules.Clients/Domain/Enums/ClientStatus.cs` | Active, Prospect, Archived, ClientStatus |
| `src/Modules/Mirama.Modules.Clients/Mirama.Modules.Clients/Domain/Enums/ClientType.cs` | ClientType, Organization, Individual |
| `src/Modules/Mirama.Modules.Clients/Mirama.Modules.Clients/Infrastructure/Persistence/Configurations/ClientConfiguration.cs` | Configure, ClientConfiguration, builder |

## How to Explore

```
get_communities with id: "community-24"
smart_context with task: "understand Aggregates/Client +3 dirs", format: "gcx"
```

_`format: "gcx"` returns the [GCX1 compact wire format](../../docs/wire-format.md) — round-trippable, ~27% fewer tokens than JSON. Drop it for JSON output; agents using `@gortex/wire` or the Go `github.com/gortexhq/gcx-go` package decode either._
