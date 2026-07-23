---
name: gortex-mirama-api-auth
description: "Work in the Mirama.Api/Auth area — 37 symbols across 1 files (98% cohesion)"
---

# Mirama.Api/Auth

37 symbols | 1 files | 98% cohesion

## When to Use

Use this skill when working on files in:
- `src/Mirama.Api/Auth/AuthJsTokenHandler.cs`

## Key Files

| File | Symbols |
|------|---------|
| `src/Mirama.Api/Auth/AuthJsTokenHandler.cs` | DecryptAesCbc, ValidateClaims, ValidFrom, DecryptAndValidate, ValidateTokenAsync, ... |

## How to Explore

```
get_communities with id: "community-0"
smart_context with task: "understand Mirama.Api/Auth", format: "gcx"
```

_`format: "gcx"` returns the [GCX1 compact wire format](../../docs/wire-format.md) — round-trippable, ~27% fewer tokens than JSON. Drop it for JSON output; agents using `@gortex/wire` or the Go `github.com/gortexhq/gcx-go` package decode either._
