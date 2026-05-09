# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Commands

```bash
# Build
dotnet build MiramaService.slnx

# Run API (dev)
dotnet run --project src/Mirama.Api/Mirama.Api.csproj

# Add EF migration (target a module's Infrastructure project)
dotnet ef migrations add <MigrationName> \
  --project src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity \
  --startup-project src/Mirama.Api

# Apply migrations manually
dotnet ef database update \
  --project src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity \
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
  Mirama.Api/                  # Entry point — wires modules, auth, middleware
  Mirama.SharedKernel/         # Cross-cutting abstractions and base types
  Modules/
    Mirama.Modules.Identity/
      Mirama.Modules.Identity/               # Application + Infrastructure
      Mirama.Modules.Identity.Contracts/     # Public events/DTOs for other modules
    Mirama.Modules.Projects/
      Mirama.Modules.Projects.Application/
      Mirama.Modules.Projects.Domain/
      Mirama.Modules.Projects.Infrastructure/
      Mirama.Modules.Projects.Contracts/
```

Each module registers itself via `AddXxxModule(config)` called from `Program.cs`. Modules own their own `DbContext`, EF migrations (separate schema per module), and repository implementations.

### SharedKernel patterns

| Concept | Location |
|---|---|
| `AggregateRoot<TID>`, `Entity<TID>`, `ValueObject` | `Abstractions/Domain/Core/` |
| `ICommand<T>` / `IQuery<T>` | `Abstractions/Common/Interfaces/ICQRSRequests.cs` |
| `IRequestHandler<,>` / `INotificationHandler<>` | `Abstractions/Common/Interfaces/` |
| `IDispatcher` → `Dispatcher` | Custom mediator (replaced MediatR) |
| `IUnitOfWork` | Each module's `DbContext` implements this |
| `ApiControllerBase` | Exposes `Dispatcher`, maps `ErrorOr` → `ProblemDetails` |
| `TenantControllerBase` / `OrganizationControllerBase` | Scoped bases for tenant/org-bound controllers |
| `ITenantOwned` / `IOrganizationOwned` | Domain interfaces for multi-tenancy |

### Dispatcher decorator chain (applied in order)

1. `TransactionDecorator` — wraps commands in a DB transaction; applied per-module before SharedKernel decorators
2. `LoggingDecorator` — logs handler start/end
3. `PerformanceDecorator` — logs slow handlers
4. `ValidationDecorator` — runs FluentValidation before handler

Queries bypass the transaction decorator. Decorators are wired via **Scrutor** (`services.Decorate`).

### ErrorOr pattern

All handlers return `ErrorOr<T>`. `ApiControllerBase.Problem(errors)` maps error types to HTTP status codes. Use domain exception classes in `Abstractions/Domain/Exceptions/` to signal invariant violations.

### Multi-tenancy

Resources implement `ITenantOwned` and/or `IOrganizationOwned`. Auth policies `RequireTenantAndOrg` / `RequireTenantOnly` enforce presence of tenant/org claims. `IRequestContextProvider` gives handlers access to the current tenant/org context.

### Outbox / Inbox

`OutboxMessage` and `InboxMessage` models exist in SharedKernel — infrastructure for reliable async messaging between modules (not yet fully wired).

### Permissions

`Mirama.SharedKernel.Models.Permissions.Permissions` — static string constants for all resource actions (e.g. `Permissions.Project.Create`). Used in authorization checks.
