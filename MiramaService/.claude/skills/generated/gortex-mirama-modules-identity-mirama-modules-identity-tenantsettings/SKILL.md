---
name: gortex-mirama-modules-identity-mirama-modules-identity-tenantsettings
description: "Work in the Mirama.Modules.Identity/Mirama.Modules.Identity · TenantSettings area — 41 symbols across 7 files (82% cohesion)"
---

# Mirama.Modules.Identity/Mirama.Modules.Identity · TenantSettings

41 symbols | 7 files | 82% cohesion

## When to Use

Use this skill when working on files in:
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Domain/Aggregates/Tenant/Subscription/SubscriptionDetails.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Domain/Aggregates/Tenant/Subscription/SubscriptionId.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Domain/Aggregates/Tenant/Tenant.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Domain/Aggregates/Tenant/TenantSettings.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Domain/Aggregates/Tenant/TenantSettingsId.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Domain/Aggregates/User/UserId.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Infrastructure/Persistence/Configurations/AggregateRoots/TenantConfig.cs`

## Key Files

| File | Symbols |
|------|---------|
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Domain/Aggregates/Tenant/Subscription/SubscriptionDetails.cs` | SubscriptionDetails |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Domain/Aggregates/Tenant/Subscription/SubscriptionId.cs` | SubscriptionId |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Domain/Aggregates/Tenant/Tenant.cs` | adminUserId, settings, settings, Tenant, subscription, ... |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Domain/Aggregates/Tenant/TenantSettings.cs` | name, timezone, brandingColor, ReceiveNotifications, BrandingColor, ... |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Domain/Aggregates/Tenant/TenantSettingsId.cs` | TenantSettingsId |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Domain/Aggregates/User/UserId.cs` | UserId |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Infrastructure/Persistence/Configurations/AggregateRoots/TenantConfig.cs` | TenantConfiguration, builder, Configure |

## How to Explore

```
get_communities with id: "community-71"
smart_context with task: "understand Mirama.Modules.Identity/Mirama.Modules.Identity · TenantSettings", format: "gcx"
```

_`format: "gcx"` returns the [GCX1 compact wire format](../../docs/wire-format.md) — round-trippable, ~27% fewer tokens than JSON. Drop it for JSON output; agents using `@gortex/wire` or the Go `github.com/gortexhq/gcx-go` package decode either._
