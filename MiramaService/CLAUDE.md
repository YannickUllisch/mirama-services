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

## CodeGraph

CodeGraph builds a semantic knowledge graph of codebases for faster, smarter code exploration.

### If `.codegraph/codegraph.db` exists in the project

**NEVER call `codegraph_explore` or `codegraph_context` directly in the main session.** These tools return large amounts of source code that fills up main session context. Instead, ALWAYS spawn an Explore agent for any exploration question (e.g., "how does X work?", "explain the Y system", "where is Z implemented?").

**When spawning Explore agents**, include this instruction in the prompt:

> This project has CodeGraph initialized (.codegraph/ exists). Use `codegraph_explore` as your PRIMARY tool - it returns full source code sections from all relevant files in one call.
>
> **Rules:**
>
> 1. Follow the explore call budget in the `codegraph_explore` tool description - it scales automatically based on project size.
> 2. Do NOT re-read files that codegraph_explore already returned source code for. The source sections are complete and authoritative.
> 3. Only fall back to grep/glob/read for files listed under "Additional relevant files" if you need more detail, or if codegraph returned no results.

**The main session may only use these lightweight tools directly** (for targeted lookups before making edits, not for exploration):

| Tool | Use For |
| ------ | --------- |
| `codegraph_search` | Find symbols by name |
| `codegraph_callers` / `codegraph_callees` | Trace call flow |
| `codegraph_impact` | Check what's affected before editing |
| `codegraph_node` | Get a single symbol's details |

### If `.codegraph/codegraph.db` does NOT exist

At the start of a session, ask the user if they'd like to initialize CodeGraph:

"I notice this project doesn't have CodeGraph initialized. Would you like me to run `codegraph init -i` to build a code knowledge graph?"