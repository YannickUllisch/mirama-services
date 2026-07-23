# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Notes 

- In the domain layer when refering to a field of that class or inherited field, ensure it is prefixed by 'this.' for readability
- All GetX list endpoints must return `PaginatedList<T>` (from `Mirama.SharedKernel.Models`). Add `int? PageNumber` and `int? PageSize` to the query record. When both are provided paginate; when omitted return all results wrapped in a single-page `PaginatedList`. Add a validator with `RuleFor(q => q.PageSize).LessThanOrEqualTo(50)`.
- Always use async variants for all DB and IO operations: `ToListAsync`, `FirstOrDefaultAsync`, `AnyAsync`, `CountAsync`, `AddAsync`, `SaveChangesAsync`, etc. Never use synchronous equivalents.

## Feature Slice Structure

Each feature lives under `Application/Features/V1/<Resource>/`. Every file follows the `<FeatureName>.<Kind>.cs` naming pattern (`Handler`, `Types`, `Validation`).

### Response types and mappers

Response types and their mapper live in the **same file**, colocated under the resource folder:

```
Projects/
  ProjectResponse.cs                  # ProjectResponse record + ProjectMapper static class
  Members/
    ProjectMemberResponse.cs          # ProjectMemberResponse record + ProjectMemberMapper static class
    AddProjectMember/
      AddProjectMember.Handler.cs
      AddProjectMember.Types.cs
    ...
  Teams/
    ProjectTeamResponse.cs            # ProjectTeamResponse record + ProjectTeamMapper static class
    ...
  Milestones/
    ProjectMilestoneResponse.cs       # ProjectMilestoneResponse record + ProjectMilestoneMapper static class
    ...
```

Rules:
- One `*Response` record and one `*Mapper` static class per file — no mixing of unrelated types.
- The mapper for the aggregate root (`ProjectMapper`) may call sub-mappers for owned collection entries.
- Shared sub-resource response types (e.g. `ProjectMemberResponse`) live under their own sub-folder (`Members/`), not in the root resource folder and not inside any individual feature folder.
- **Mapper goes first, response record goes below** — always in this order within the file.
- **Every property on every response record must have `[JsonPropertyName("camelCaseName")]`** — use the object-initializer style (`new() { ... }`) in mappers, not positional record constructors, so properties are explicit. See `OrganizationResponse.cs` as the canonical example.
- **The root entity's own ID is always serialized as `"id"`** — never `"projectId"`, `"milestoneId"`, `"tagId"`, etc. Foreign-key references to other resources keep their descriptive name (e.g. `"teamId"`, `"memberId"`).

### Aggregate roots and sub-resource endpoints

The aggregate root owns all mutations. Sub-resource collections (members, teams, milestones, etc.) are managed through the aggregate, never modified directly via their own repository.

REST layout for an aggregate `Project` with owned collection `Members`:

| Method | Route | Notes |
|---|---|---|
| `GET` | `/projects` | paginated list |
| `POST` | `/projects` | create aggregate |
| `GET` | `/projects/{id}` | full aggregate response incl. sub-collections |
| `PUT` | `/projects/{id}` | update core fields + tags only — do NOT reconcile sub-collections here |
| `POST` | `/projects/{id}/archive` | named state transition |
| `GET` | `/projects/{id}/members` | paginated sub-resource list |
| `POST` | `/projects/{id}/members` | add member via aggregate method |
| `PUT` | `/projects/{id}/members/{memberId}` | update member role via aggregate method |
| `DELETE` | `/projects/{id}/members/{memberId}` | remove member via aggregate method |

General rules:
- Sub-resource mutations (add/update/remove) get their own vertical slice under `<Resource>/<SubResource>/<FeatureName>/`.
- Each slice has its own command/query record, handler, and optional validator — never share a command across slices.
- `PUT /resource/{id}` updates only the aggregate's own scalar fields and simple value collections (e.g. tag IDs). Sub-entity collections are managed through dedicated sub-resource endpoints.
- Route params (`projectId`, `memberId`) are injected into the command using `command with { ProjectId = projectId }` — the body is the command record with the route fields overridden.
- Commands that add a sub-entity return the created sub-resource response (`201 Created` with `Location` header). Commands that remove return `204 No Content`.
- Load only the includes needed for the operation — do not eagerly load all collections on every command.

## Commands

```bash
# Build
dotnet build MiramaService.slnx

# Run API (dev)
dotnet run --project src/Mirama.Api/Mirama.Api.csproj

# Add EF migration (replace --project with the target module path)
dotnet ef migrations add <MigrationName> \
  --project src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity \
  --startup-project src/Mirama.Api

dotnet ef migrations add <MigrationName> \
  --project src/Modules/Mirama.Modules.PM/Mirama.Modules.PM \
  --startup-project src/Mirama.Api

dotnet ef migrations add <MigrationName> \
  --project src/Modules/Mirama.Modules.Clients/Mirama.Modules.Clients \
  --startup-project src/Mirama.Api

# Apply migrations manually (replace --project with the target module path)
dotnet ef database update \
  --project src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity \
  --startup-project src/Mirama.Api

dotnet ef database update \
  --project src/Modules/Mirama.Modules.PM/Mirama.Modules.PM \
  --startup-project src/Mirama.Api

dotnet ef database update \
  --project src/Modules/Mirama.Modules.Clients/Mirama.Modules.Clients \
  --startup-project src/Mirama.Api

# Docker build
docker build -t mirama-service .
```

In Development, migrations and seeding run automatically on startup via `Program.cs`.

## Architecture

ASP.NET Core 10 modular monolith. Clean architecture + vertical slices inside each module.

### Project layout

```
src/
  Mirama.Api/                  # Entry point - wires modules, auth, middleware
  Mirama.SharedKernel/         # Cross-cutting abstractions and base types
  Modules/
    Mirama.Modules.Identity/
      Mirama.Modules.Identity/               # Application + Infrastructure
      Mirama.Modules.Identity.Contracts/     # Public events/DTOs for other modules
    Mirama.Modules.PM/
      Mirama.Modules.PM/                     # Application + Infrastructure
      Mirama.Modules.PM.Contracts/           # Public events/DTOs for other modules
    Mirama.Modules.Clients/
      Mirama.Modules.Clients/                     # Application + Infrastructure
      Mirama.Modules.Clients.Contracts/           # Public events/DTOs for other modules
```

Each module registers itself via `AddXxxModule(config)` called from `Program.cs`. Modules own their own `DbContext`, EF migrations (separate schema per module), and repository implementations.

### SharedKernel patterns

| Concept | Location |
|---|---|
| `OrganizationAggregateRoot<TID>`, `OrganizationEntity<TID>`, `ValueObject` | `Abstractions/Domain/Core/` |
| `ICommand<T>` / `IQuery<T>` | `Abstractions/Common/Interfaces/ICQRSRequests.cs` |
| `IRequestHandler<,>` / `INotificationHandler<>` | `Abstractions/Common/Interfaces/` |
| `IDispatcher` → `Dispatcher` | Custom mediator (replaced MediatR) |
| `IUnitOfWork` | Each module's `DbContext` implements this |
| `ApiControllerBase` | Exposes `Dispatcher`, maps `ErrorOr` → `ProblemDetails` |
| `TenantControllerBase` / `OrganizationControllerBase` | Scoped bases for tenant/org-bound controllers |
| `ITenantOwned` / `IOrganizationOwned` | Domain interfaces for multi-tenancy |

### Dispatcher decorator chain (applied in order)

1. `TransactionDecorator` - wraps commands in a DB transaction; applied per-module before SharedKernel decorators
2. `LoggingDecorator` - logs handler start/end
3. `PerformanceDecorator` - logs slow handlers
4. `ValidationDecorator` - runs FluentValidation before handler

Queries bypass the transaction decorator. Decorators are wired via **Scrutor** (`services.Decorate`).

### ErrorOr pattern

All handlers return `ErrorOr<T>`. `ApiControllerBase.Problem(errors)` maps error types to HTTP status codes. Use domain exception classes in `Abstractions/Domain/Exceptions/` to signal invariant violations.

### Multi-tenancy

Resources implement `ITenantOwned` and/or `IOrganizationOwned`, `OrganizationAggregateRoot` and `OrganizationEntity` implement this, unless its specifically asked to be a TenantOwned entity prefer these ofer using the base AggregateRoot or Entity classes. Auth policies `RequireTenantAndOrg` / `RequireTenantOnly` enforce presence of tenant/org claims. `IRequestContextProvider` gives handlers access to the current tenant/org context.

### Outbox / Inbox

`OutboxMessage` and `InboxMessage` models exist in SharedKernel - infrastructure for reliable async messaging between modules (not yet fully wired).

### Permissions

`Mirama.SharedKernel.Models.Permissions.Permissions` - static string constants for all resource actions (e.g. `Permissions.Project.Create`). Used in authorization checks.

<!-- gortex:communities:start -->
## Codebase Overview (generated by Gortex)

- **Languages:** csharp (primary), contract, dockerfile, dotenv, dotnet, editorconfig, gitignore, json, markdown, text
- **Most-referenced symbols:** `IRequestHandler` (131 usages), `INotificationHandler` (125 usages), `Send` (112 usages), `PaginatedList` (105 usages), `ICommand` (72 usages), `HandleAsync` (70 usages), `HandleAsync` (70 usages), `HandleAsync` (70 usages), `HandleAsync` (70 usages), `HandleAsync` (70 usages)
- **Graph size:** 4607 nodes, 37961 edges
- **Breakdown:** 1 config_keys, 82 constants, 111 contracts, 74 docs, 86 enum_members, 414 fields, 678 files, 3 functions, 52 generic_params, 4 images, 30 interfaces, 838 methods, 26 modules, 18 packages, 1120 params, 1 todos, 800 types, 269 variables

## MANDATORY: Use Gortex MCP tools instead of Read/Grep/Glob

Gortex is running as an MCP server. You **MUST** prefer graph queries over file reads on every task in this repo — `search_symbols`, `find_usages`, `get_symbol_source`, `get_editing_context`, `smart_context`, `edit_symbol` / `edit_file` / `rename_symbol` / `batch_edit`. Hook posture is configurable; follow every Gortex hook instruction even when `Read` / `Grep` / `Glob` remain callable. The full per-tool catalog loads via `tools/list` — not restated here.

### Calibration: the graph narrows scope, source confirms behavior

The mandate above stands — but graph queries *narrow scope*, they do not *replace reading the implementation*. The graph tells you **where** the logic lives and **what** connects to it; the source tells you **how** it behaves. For the symbol you are about to change or depend on, read its full body with `get_symbol_source` — do not act on a one-line summary alone.

Be especially deliberate with **behavior-critical code** — database migrations, retry / fallback / error-recovery paths, compatibility shims, concurrency-sensitive sections, and the tests that pin them. For these, call `get_symbol_source` and read the real implementation; never pass `compress_bodies:true`, which elides exactly the branches that carry the risk. Reserve compressed bodies and graph summaries for breadth (surveying many symbols); use full source for the few you are about to commit to.

## Required workflow (every task on this repo)

These are not suggestions — run each step at the trigger.

1. Confirm the daemon is up with `index_health` (cheap liveness + scope). Call `graph_stats` only when you actually need node/edge counts or `per_repo` orientation — it returns a large payload and can block during warmup.
2. If `total_nodes` is 0, **call** `index_repository` with `"."` before anything else.
3. In multi-repo mode, **call** `get_active_project` to check scope; use `set_active_project` to switch.
4. Open a non-trivial task with `smart_context` for orientation. For a single known symbol or file, go straight to `search_symbols` / `get_symbol_source` — don't front-load `smart_context` before every read.
5. Before editing a file, **call** `get_editing_context` on it first.
6. Before changing any function signature, **call** `verify_change` to catch broken callers and interface implementors (cross-repo).
7. For any refactor, **call** `get_edit_plan` then `batch_edit` to apply atomically.
8. Verify with the project's real build/test. Reserve `check_guards` for guard-relevant changes and `get_test_targets` to find the tests covering a substantive change — not mechanically after every edit.

<!-- gortex:skills:start -->
## Community Skills

| Area | Description | Skill |
|------|-------------|-------|
| V1 Billing 109 Dirs | 434 symbols | `/gortex-v1-billing-109-dirs` |
| Mirama Sharedkernel Models 102 Dirs | 343 symbols | `/gortex-mirama-sharedkernel-models-102-dirs` |
| Workflowconfig Status 12 Dirs | 199 symbols | `/gortex-workflowconfig-status-12-dirs` |
| Models Decorators 110 Dirs | 178 symbols | `/gortex-models-decorators-110-dirs` |
| Clients Createclient 72 Dirs | 140 symbols | `/gortex-clients-createclient-72-dirs` |
| Aggregates Task Task | 88 symbols | `/gortex-aggregates-task-task` |
| Domain Core 10 Dirs | 87 symbols | `/gortex-domain-core-10-dirs` |
| V1 Auth 27 Dirs | 75 symbols | `/gortex-v1-auth-27-dirs` |
| Aggregates Asset Asset | 67 symbols | `/gortex-aggregates-asset-asset` |
| Common Interfaces 10 Dirs | 62 symbols | `/gortex-common-interfaces-10-dirs` |
| Aggregates Kanbanboard | 44 symbols | `/gortex-aggregates-kanbanboard` |
| Abstractions Permissions 5 Dirs | 43 symbols | `/gortex-abstractions-permissions-5-dirs` |
| Models Permissions | 42 symbols | `/gortex-models-permissions` |
| Modules Mirama Modules Identity Tagscopedto | 42 symbols | `/gortex-modules-mirama-modules-identity-tagscopedto` |
| Mirama Modules Identity Mirama Modules Identity Tenantsettings | 41 symbols | `/gortex-mirama-modules-identity-mirama-modules-identity-tenantsettings` |
| Mirama Api Auth | 37 symbols | `/gortex-mirama-api-auth` |
| Aggregates Client 3 Dirs | 36 symbols | `/gortex-aggregates-client-3-dirs` |
| Mirama Modules Identity Mirama Modules Identity Identitydbcontext | 34 symbols | `/gortex-mirama-modules-identity-mirama-modules-identity-identitydbcontext` |
| Mirama Modules Identity Mirama Modules Identity Policy | 32 symbols | `/gortex-mirama-modules-identity-mirama-modules-identity-policy` |
| Infrastructure Persistence 2 Dirs | 32 symbols | `/gortex-infrastructure-persistence-2-dirs` |
<!-- gortex:skills:end -->

<!-- gortex:communities:end -->
