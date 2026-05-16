# Cross-Module Communication

## Overview

MiramaService is a **Modular Monolith**: all business modules run in a single .NET host process (`Mirama.Api`), but each module owns its own database schema, its own `DbContext`, and its own domain model. Modules are not allowed to reach into each other's databases or call each other's internal services directly.

Cross-module communication happens through exactly two patterns, chosen for different situations:

1. **Synchronous communication via Contract projects** — the primary pattern. Used when a module needs data or a result from another module immediately.
2. **Asynchronous communication via Domain Events and the Outbox** — the secondary pattern. Used for side effects, reactions, and propagation of facts across module boundaries where immediacy is not required.

This document explains both patterns, the theory behind them, and why we chose them over alternatives.

---

## Key Concepts

| Term | Meaning |
|---|---|
| **Module** | A self-contained vertical slice (e.g. `Identity`, `Projects`). Owns its own `DbContext`, schema, and domain layer. |
| **Contract project** | A lightweight assembly (`*.Contracts`) that a module exposes publicly. Contains interfaces and DTOs other modules may depend on. No business logic. |
| **Domain Event** | A record that something happened in the domain (e.g. `ProjectCreated`). Raised by Aggregate Roots, published within and across modules. |
| **Aggregate Root** | The entry point to a domain cluster. The only entity that can raise Domain Events. |
| **Dispatcher** | The internal message bus. Routes commands/queries to a single handler; domain events to all subscribed handlers. |
| **Outbox** | A table in a module's own schema that records domain events atomically alongside the business write that produced them. |
| **Eventual Consistency** | A guarantee that, given no permanent failure, all modules will eventually reflect the same reality — even if there is a temporary lag. |
| **At-Least-Once Delivery** | Messages are guaranteed to be delivered but may be delivered more than once. Consumers must handle duplicates. |
| **Idempotency** | An operation that produces the same result regardless of how many times it is applied. |

---

## Pattern 1 — Synchronous Communication via Contract Projects

### Theory

Each module exposes a `*.Contracts` project — a thin, dependency-light assembly that defines the public interface of that module: service interfaces and the DTOs they return. The internal implementation lives in the module's Application or Infrastructure layer and is registered in the shared DI container at startup.

When Module A needs something from Module B, it takes a project reference on Module B's Contracts assembly, declares a dependency on the interface, and receives the implementation through DI at runtime. No knowledge of Module B's internals leaks into Module A.

```
[Mirama.Modules.Identity.Contracts]
  └── IIdentityService.cs          ← interface other modules consume
  └── UserDto.cs                   ← DTO in the public contract

[Mirama.Modules.Identity.Application]
  └── IdentityService.cs           ← implementation, invisible to callers
  └── registered in DI at startup

[Mirama.Modules.PM.Application]
  └── SomeHandler.cs               ← injects IIdentityService via constructor
```

### Why We Chose This as the Primary Pattern

For the large majority of cross-module interactions, the caller needs a result *now*: check whether an organization exists before creating a project, retrieve a user's display name, verify a permission. Introducing async machinery for these cases would add latency, complexity, and eventual-consistency problems in flows that are inherently synchronous from the HTTP request perspective.

The contract project pattern keeps this simple. It is still modular — Module A cannot touch Module B's database or internal types — but it avoids the overhead of event routing, outbox polling, and idempotency bookkeeping for queries and lookups that do not modify state.

### Pros

- **Simple to reason about.** A method call returns a value. No delayed delivery, no retry loops, no duplicate-handling logic.
- **Strong consistency.** The result is immediately accurate — no lag between when Module B mutates state and when Module A sees it.
- **Easy to test.** The interface can be mocked in unit tests. Integration tests call the real implementation.
- **Clear ownership.** The Contracts project is an explicit, versioned public API surface. What is not in Contracts is internal.

### Cons

- **Caller and callee share a process fate.** If the implementation throws or is slow, the calling request is affected. There is no circuit-breaker isolation between modules the way a network call would allow.
- **Temporal coupling.** The callee must be able to handle the call at the moment it is made. There is no buffering.
- **Discipline required.** Without enforcement, developers may be tempted to reference internal projects instead of Contracts projects, eroding module boundaries silently.

---

## Pattern 2 — Asynchronous Communication via Domain Events and the Outbox

### Theory

When a module performs a state change that other modules need to *react to*, it raises a **Domain Event** from an Aggregate Root. The event is a fact — "this happened" — not a command directed at any specific module. Any module that cares registers a handler for it.

The challenge is reliability: delivering that event must not be decoupled from the write that produced it. If the system crashes between saving business data and notifying other modules, the notification is lost. The **Transactional Outbox** pattern solves this by writing the event into the same database transaction as the business data. A background relay then reads undelivered events and dispatches them. The business write and the intent to notify are atomic.

### The Full Flow

#### Step 1 — Command executes inside a transaction

```
Request
  → ExceptionToErrorOrDecorator
  → LoggingDecorator
  → PerformanceDecorator
  → ValidationDecorator
  → TransactionDecorator       ← opens DB transaction
  → Handler                    ← mutates Aggregate Root, raises Domain Events
```

#### Step 2 — SaveChangesAsync captures and persists events

Before writing to the database, the module's `DbContext` override:

1. Collects all `IDomainEvent` instances held by tracked `AggregateRoot` entities.
2. Publishes in-process events via `IDispatcher.Publish()` — for within-module side effects that must be strongly consistent (e.g. updating a read model). These run inside the open transaction.
3. Serializes cross-module events to the `OutboxMessages` table as JSON rows.
4. Commits business data, in-process side effects, and outbox rows in a single `base.SaveChangesAsync()` call.

**Core guarantee: if the business write commits, the outbox row commits. They cannot be separated.**

#### Step 3 — The Outbox Relay delivers the event

A `BackgroundService` polls `OutboxMessages` for rows where `ProcessedAtUtc` is null. For each:

1. Deserializes the event.
2. Dispatches to all registered cross-module handlers.
3. On success, stamps `ProcessedAtUtc`.
4. On failure, records the error and retries on the next cycle.

Delivery is **at-least-once**: if the relay crashes after dispatching but before stamping, the message is re-delivered. Handlers must be idempotent.

#### Step 4 — Handlers deduplicate with an idempotency record

Each handler checks whether the event's ID exists in an `IdempotencyRecords` table before processing. If it does, the handler is a no-op. If it does not, the handler processes the event and records the ID in the same transaction as its state changes.

### Why We Use This for Side Effects and Reactions

Reactions to domain facts — provisioning resources, updating projections, sending notifications — do not need to happen within the calling request. The user who creates an organization does not need to wait for a default project to be provisioned; they need to know their organization was created. Decoupling the reaction from the originating command improves response time and makes the system more resilient: a temporary failure in the Projects module does not fail the Identity operation.

Domain events also serve as the natural seam for future service extraction. If `Identity` and `Projects` are ever split into separate services, the domain event contracts remain unchanged — only the relay changes from in-process dispatch to a broker publish.

### Pros

- **Resilient to partial failure.** The outbox guarantees delivery even if the receiving module or the relay is temporarily down. Retries are automatic.
- **Decoupled module lifecycles.** A failure or slowness in the reacting module does not affect the originating operation.
- **Natural microservice migration path.** Domain event contracts are infrastructure-independent. The relay can be swapped for a broker (RabbitMQ, Kafka) without changing domain or application code.
- **Loose coupling.** The emitting module has no knowledge of who reacts or how many modules subscribe.

### Cons

- **Eventual consistency.** There is a window between the original write and the reaction completing. Modules must be designed to tolerate this lag.
- **Idempotency is non-negotiable.** Every cross-module handler must be written to handle duplicate delivery. This adds design and testing overhead.
- **Harder to trace.** A synchronous call has a single stack trace. An async event chain spans multiple requests, poll cycles, and log entries. Correlation IDs are essential.
- **Polling overhead.** The outbox relay adds a continuous background load. This is acceptable at current scale but will need tuning as volume grows.

---

## Outbox Table Schema

Each module's schema contains an `OutboxMessages` table:

| Column | Type | Purpose |
|---|---|---|
| `Id` | `Guid` | Unique message identifier — used for idempotency checks |
| `Type` | `string` | .NET type name of the Domain Event |
| `Content` | `string` | JSON-serialized event payload |
| `OccurredAtUtc` | `DateTime` | When the event was raised |
| `ProcessedAtUtc` | `DateTime?` | Null until successfully delivered |
| `Error` | `string?` | Last delivery error, if any |

---

## Failure Scenarios

| Scenario | What happens |
|---|---|
| Handler throws an exception | `TransactionDecorator` rolls back. No business data written, no outbox row created. |
| Crash after `CommitTransaction`, before relay processes | On restart, relay finds the unprocessed outbox row and resumes. |
| Target module's handler throws | Relay records error in `Error` column and retries next cycle. |
| Message delivered twice | `IdempotencyRecord` check in the handler skips processing. |
| Synchronous call fails (Pattern 1) | Exception propagates up to the caller in the same request. Standard error handling applies. |

---

## Choosing the Right Pattern

| Situation | Pattern |
|---|---|
| Need a value or check result *right now* | Synchronous via Contracts |
| Query — read data from another module | Synchronous via Contracts |
| React to something that happened — no result needed | Async via Domain Event |
| Propagate a fact to multiple modules | Async via Domain Event |
| Side effect that can tolerate eventual consistency | Async via Domain Event |
| Side effect that must be strongly consistent | In-process event within same module |

---

## Design Rationale

**Why not direct service calls without contract projects?**  
Without a contracts boundary, developers reference internal types — application services, repositories, domain entities — from other modules. The boundary erodes. Internal changes in Module B break Module A silently. The contracts project creates a stable, explicit public API: if it is not in Contracts, it is internal and cannot be referenced.

**Why not make everything asynchronous?**  
Async-everywhere is seductive but introduces real costs: eventual consistency is hard to reason about, idempotency must be implemented everywhere, debugging requires distributed tracing, and simple reads become event-driven queries. For the majority of cross-module interactions — lookups, permission checks, foreign-key-equivalent validations — these costs buy nothing. Synchronous contracts are simpler and correct.

**Why not use an external message broker (e.g. RabbitMQ, Kafka) for the async path?**  
At the current stage, an external broker adds operational complexity — another service to run, monitor, and tune — without changing the reliability guarantee. The outbox pattern provides the same at-least-once delivery using the database each module already owns. The domain event contracts are broker-agnostic; when modules need to be extracted into separate services, the relay is the only component that changes.

**Why publish some events in-process during `SaveChangesAsync`?**  
Within-module side effects that must be strongly consistent — updating a read model, enforcing an invariant that spans two aggregates — need to run inside the same transaction as the state change. In-process dispatch handles these. Cross-module effects, where eventual consistency is acceptable and the reaction may fail independently, go through the outbox.
