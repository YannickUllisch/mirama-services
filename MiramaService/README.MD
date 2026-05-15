# MiramaService

Backend for the Mirama platform. ASP.NET Core 10 modular monolith using clean architecture and vertical slices.

## Overview

Single deployable service that hosts independent feature modules. Each module owns its domain, application logic, infrastructure, and database schema. Modules communicate either via synchronous calls to "Contract DBO"s exposed my a module and in-process domain events.

## Modules

| Module | Responsibility |
|---|---|
| **Identity** | Users, tenants, organizations, authentication |
| **Projects** | Project management domain |

## Tech Stack

- **Runtime:** .NET 10 / ASP.NET Core
- **Database:** PostgreSQL via Entity Framework Core (per-module schemas)
- **Auth:** JWT bearer tokens with tenant/org claims
- **Validation:** FluentValidation
- **Mediator:** Custom `IDispatcher` (replaces MediatR)
- **Error handling:** `ErrorOr` -> `ProblemDetails`

## Getting Started

```bash
# Build
dotnet build MiramaService.slnx

# Run (migrations + seeding run automatically in dev)
dotnet run --project src/Mirama.Api/Mirama.Api.csproj

# Docker
docker build -t mirama-service .
```

## Architecture

Clean architecture + vertical slices inside each module. Modules register themselves via `AddXxxModule()` in `Program.cs`.

```
src/
  Mirama.Api/           # Entry point - wires modules, auth, middleware
  Mirama.SharedKernel/  # Shared abstractions, base types, dispatcher
  Modules/
    Mirama.Modules.Identity/
    Mirama.Modules.Projects/
```

Dispatcher decorator chain (commands): `Transaction -> Logging -> Performance -> Validation -> Handler`
