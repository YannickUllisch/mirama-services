# Outbox & Cross-Module Event Architecture

## Overview

The system is a modular monolith: independently-schemaed modules (PM, Clients, Identity, Workspace) sharing one process and one dependency injection container, deployed as multiple running copies of that same process behind a load balancer. Modules communicate in two ways: synchronous in-process service calls for reads, and events for "something happened, react if relevant" - the subject of this document.

Two kinds of events exist, deliberately kept distinct:

- **Domain events** are internal facts. An aggregate raises one to describe something that happened to it. They never leave the module that raised them, are never durable, and are dispatched synchronously, inline, inside the same transaction as the change that raised them.
- **Integration events** are the durable, public form of a fact - the thing other handlers, in the same module or any other, can react to reliably, even across a crash or a restart. They are never raised directly. They are produced by translating a domain event through a mapper.

This document covers everything up to and including **fan-out**: how a domain event becomes a durable outbox row, and how that row turns into one pending item per interested handler. What happens to that item afterward - claiming it, running the handler, retrying, dead-lettering - is a separate concern with its own document: see `background-jobs-inbox-design.md`. The two are split deliberately, because they are two independent failure domains (a message failing to fan out has nothing to do with a handler failing to run) and two independent processes (a module fans out its own outbox on one schedule and drains its own inbox on another).

## Part 1 - What may run synchronously, and what must not

A domain event handler runs inline, inside `SaveChangesAsync`, before the surrounding transaction commits. That makes it the correct place for one thing only: further writes to the same `DbContext`, so that an aggregate's change can cascade into another local write and land in the same atomic commit. It is the wrong place for anything that reaches outside that transaction - an HTTP call to another system, a message broker publish, an email, a call to a payment processor, or any operation whose failure mode includes "the network was down" or "the other side timed out."

Three concrete problems follow from putting networked or otherwise externally-effecting work in a domain event handler:

1. **Coupled failure.** The handler runs inside `SaveChangesAsync`; if it throws, the entire transaction rolls back - an unrelated business write now fails because a third-party API had a bad moment.
2. **Post-hoc inconsistency.** The handler runs before `base.SaveChangesAsync()`. If the external call succeeds and the subsequent commit then fails for an unrelated reason (a concurrency conflict, a constraint violation), an external system has now been told something that, locally, never happened - and an HTTP call already sent cannot be rolled back.
3. **No retry.** A transient failure in a synchronous handler simply fails the request. There is no automatic redelivery.

Whether the consumer of the fact lives in the same module or a different one is irrelevant to this decision. A same-module handler that calls an external payment gateway has exactly the same failure modes as a handler in another module doing the same thing. The rule is: **any side effect with external reach is expressed as an integration event and delivered through the outbox, regardless of which module consumes it.** Same-transaction-only work is the only thing that belongs in a synchronous domain event handler. See Part 3 for what this means for a same-module integration event handler specifically.

## Part 2 - The mapping layer

An aggregate raises only domain events - plain facts, with no knowledge of any public contract. Turning a domain event into a public integration event is a separate, explicit step performed by a mapper:

```
IIntegrationEventMapper<TDomainEvent>.Map(TDomainEvent domainEvent) : IEnumerable<IIntegrationEvent>
```

One domain event maps to zero, one, or several integration events. Zero is the common case for most domain events - most facts are purely internal and nothing durable needs to be produced. One is the common case for a fact another module (or a durable same-module handler) needs to react to. More than one covers a fact that has more than one independent public consequence.

A mapper is a pure, synchronous, in-memory function - no database access, no network calls, nothing awaited. It runs inline, inside `SaveChangesAsync`, so it has to be cheap and safe to run on every save without exception. Whatever data a mapper needs is already present on the domain event; if it is not, it is added there when the event is raised, not fetched during mapping. This is also what makes a mapper trustworthy to run unconditionally: a mapper that throws indicates a defect (a null reference, a missing field), not a transient condition, and aborting the transaction in that case is correct - mapping is supposed to be deterministic.

Multiple mapper classes may target the same domain event type; their combined output is used. A mapper class is registered exactly like a notification handler - a class implementing `IIntegrationEventMapper<T>` inside a module's own assembly is discovered by that module's dependency injection scan with no additional wiring.

An integration event produced by a mapper is data only: an immutable record carrying whatever public consumers need, an `EventId` (a correlation/idempotency identity a handler can use to recognize a repeat delivery), an `AggregateId` (the aggregate it concerns), and an `OccurredAt` timestamp. It carries no behavior. It is defined in the producing module's `*.Contracts` project specifically when another module needs to reference its shape to write a handler for it; a purely same-module integration event (produced only to get durable, retried delivery for one local handler) needs no such placement.

Raising an integration event directly from an aggregate - bypassing the mapper - is rejected at runtime rather than silently accepted: an object implementing `IIntegrationEvent` raised via `AddDomainEvent` would otherwise be dispatched as if it were an ordinary domain event and never reach the outbox, a silent and easy-to-miss mistake. The persistence layer checks for this on every raised event and fails loudly if it happens.

## Part 3 - From a raised fact to a fanned-out inbox row

**An aggregate raises a fact.** Inside an aggregate method, `AddDomainEvent(event)` appends to a private, in-memory list on the aggregate. Nothing has happened outside the aggregate yet; this is bookkeeping until `SaveChangesAsync` runs.

**`SaveChangesAsync` reads that list once.** The base persistence class every module's `DbContext` inherits does, in order:

1. Audit-stamps tracked entities and assigns tenant/organization ownership.
2. Collects every raised event off every tracked aggregate, clearing each aggregate's list as it does.
3. For each event: if it implements `IIntegrationEvent`, the save fails immediately (Part 2). Otherwise it is dispatched synchronously to every `INotificationHandler<T>` registered for its concrete type, right here, before `base.SaveChangesAsync()`. A handler that adds or modifies entities on the same `DbContext` has those changes captured and committed in the same transaction. An event raised with no registered handler is simply observed and dropped - not an error.
4. Every collected domain event is then offered to whatever `IIntegrationEventMapper<T>` is registered for its concrete type. The combined output - zero, one, or many `IIntegrationEvent` instances - becomes one `OutboxMessage` row per integration event, added to the same `DbContext`.
5. `base.SaveChangesAsync()` runs. The business write, the outboxed rows, and anything a synchronous domain event handler added are committed together, or none of them are.

This is the entire durability guarantee for getting a fact recorded: either the business change and the durable record that a public fact occurred both land, or neither does.

**A background process fans each message out.** Each module with outboxed events runs its own outbox processor against its own `DbContext`. On a timer - a short interval right after finding work, a longer one when idle - it opens a brief transaction and selects unprocessed, currently-available, unleased rows with `FOR UPDATE SKIP LOCKED`, immediately leases them (a `LockedUntilUtc`/`LockedBy` pair) and commits. Claiming is a separate, fast transaction from actual fan-out.

For each claimed message, in its own scope and `DbContext` instance, inside one local transaction:

1. The message's stored type name is resolved back to a CLR type via a per-module lookup built once at startup from that module's own `*.Contracts` assembly.
2. Every `INotificationHandler<T>` registered anywhere in the process for that concrete type is resolved - this reaches across every module because every module is composed into one shared dependency injection container.
3. For each resolved handler, its owning module's schema is looked up (every module registers this once at startup - see `IModuleSchemaRegistry`), and one `InboxMessage` row is inserted directly into that module's own Inbox table: the event's payload, its assembly-qualified type name, which handler it is for, and its `AggregateId`.
4. The outbox message row is deleted.

**A handler in the same module as the aggregate goes through exactly the same fan-out as a handler in any other module.** Nothing about this process treats "same module" as a shortcut: if `Mirama.Modules.Clients` both raises the domain event (via its mapper) and hosts a handler for the resulting integration event, that handler's owning schema - resolved the same way as any other - happens to be `clients`, so the `InboxMessage` row lands in the Clients module's own inbox table, right next to any other module's. See Part 4 for why this uniformity is intentional rather than an oversight.

Steps 1–4 either all happen or none of them do: fan-out touches nothing but the local database, with no external I/O anywhere in it, so the whole thing runs inside one transaction per message. There is no partial state a retry could ever observe - either the Inbox rows exist and the outbox row is gone, or nothing changed and the next poll tries again from scratch. If resolving the type fails, or the database rejects a write, the message's own retry count increments with backoff, and after enough failures it is moved to that module's `OutboxDeadLetter` table and removed from the live outbox (Part 5).

From here, what happens to a fanned-out `InboxMessage` - claiming it, invoking its handler, retrying, ordering, dead-lettering - is entirely `background-jobs-inbox-design.md`'s subject, not this document's. The outbox's job ends the moment the row exists somewhere and its own row is gone.

**Every message also carries a small envelope alongside its payload**, stamped once by the same base persistence class that builds the `OutboxMessage` rows, and copied verbatim onto every `InboxMessage` a message fans out into (and onto `OutboxDeadLetter`/`InboxDeadLetter` if either side ever dead-letters it): `OrganizationId` and `TenantId` (the caller's, at the moment of the original save), `TraceId` (from the ambient `Activity`, the same source already used for audit logging elsewhere in the codebase), a `CorrelationId` (currently mirroring `TraceId`, reserved for a distinct concept later), and an open `Headers` JSON column for anything else that doesn't yet warrant its own field. None of this is required reading to follow the flow above - it exists for operational queries (`OrganizationId` above all) and future tracing needs, not for fan-out logic itself, which never inspects any of it.

## Part 4 - Same-module fan-out: why not special-case it

It would be possible to have the outbox processor invoke a same-module handler directly, in-process, instead of writing an `InboxMessage` and letting that module's own inbox processor pick it up later. It is deliberately not built that way:

- **One mechanism, no branch.** A handler is a handler regardless of which module owns it. Special-casing same-module handlers would mean two different code paths with two different failure/retry behaviors for what is, from the aggregate's perspective, the identical situation - "some handler somewhere needs this integration event."
- **The reason the event went through a mapper at all still applies.** An integration event exists specifically because Part 1's rule - anything with external reach must not run inside the original transaction - applies regardless of module boundary. If a same-module handler could run synchronously and safely, it would have stayed a domain event handler; the fact that it is an *integration* event handler at all means it needs the same durability, retry, and isolation from the original request that any other integration event handler gets.
- **Independent retry ownership.** Routing every handler through an inbox, uniformly, is what makes Part 5's per-handler retry/dead-letter isolation possible at all (see `background-jobs-inbox-design.md`, Part 5). If same-module handlers bypassed the inbox, they would fall back to some other retry story, and the system would have two different reliability guarantees to reason about instead of one.

The cost is one extra hop - a same-module handler waits for its own module's inbox processor to poll, rather than running the instant fan-out claims the message. At the poll intervals this system already runs at (low hundreds of milliseconds when busy), that cost is small next to the consistency of having exactly one delivery mechanism for every integration event handler, everywhere.

## Part 5 - Fan-out retry and dead-lettering

Fan-out failure is tracked on `OutboxMessage` itself, and it is a narrow failure surface: an event type that cannot be resolved (typically a rename made without a transition plan - see Part 7), a handler whose module never registered its schema, or a database error while writing an Inbox row. None of these have anything to do with whether a handler's business logic works; fan-out never runs a handler, so it can never fail because one did.

A message that keeps failing to fan out increments its own retry count with exponential backoff and, past the configured limit (`OutboxOptions.MaxRetries`), moves to that module's `OutboxDeadLetter` table and is removed from the live outbox. This is a separate failure domain from anything a handler does - see `background-jobs-inbox-design.md` Part 5 for how a handler's own failures are tracked, which is on the `InboxMessage` side and never touches this table.

## Part 6 - Locking, horizontal scaling, and parallelization

The outbox is designed to run as more than one copy of the same process at once, all polling the same table, with no coordinator between them. Claiming uses row-level locking with skip-on-conflict: if one instance's claim transaction has a row locked, a concurrent claim query from another instance simply skips it and selects a different eligible row instead of waiting. More running instances means more disjoint batches claimed per unit time - throughput scales without any added coordination code.

The lease is what makes a crash safe: if an instance claims a batch and dies before finishing fan-out, it never deletes the row. Once the lease's expiry passes, another instance's next claim sees the row as eligible again. Fan-out has no realistic double-invocation risk in practice, unlike handler execution - it does no external work and completes near-instantly relative to any realistic lease duration, so the window in which a lease could expire mid-fan-out and cause two instances to fan out the same message concurrently is vanishingly small (and even if it happened, the worst outcome is a duplicate `InboxMessage` row, which the inbox side's own idempotency requirement on handlers already has to tolerate).

Fan-out has no ordering concern of its own - it only copies rows into each consumer's inbox and never executes anything, so a claimed batch runs with full bounded parallelism (`OutboxOptions.MaxDegreeOfParallelism`) across every claimed message, with no sequential groups to account for. Ordering, where it matters, is enforced entirely on the inbox side - see `background-jobs-inbox-design.md` Part 3.

## Part 7 - Evolving an event's shape

A stored outbox message records its type by a short name and its payload as JSON. Renaming or restructuring an event type breaks resolution for any row already sitting in the outbox under the old shape; the type lookup fails loudly rather than silently corrupting data, which turns the mistake into a retrying, eventually dead-lettering message (Part 5) rather than a corrupted delivery.

Once fanned out, each `InboxMessage` carries the type's full assembly-qualified name rather than the short name, resolved directly via `Type.GetType` on the inbox side - this is what lets a consuming module deserialize an event it did not define without needing any copy of, or binding to, the publishing module's `*.Contracts` assembly.

An additive, backward-compatible change - a new optional field with a sensible default - is safe without coordination; already-serialized rows without the new field deserialize with it left at its default. A breaking change is not made in place: a new type is introduced under a new name, both old and new are handled on the consuming side during a transition window, the producing aggregate's mapper (not the aggregate itself) is switched to emit the new type, and once the old outbox has fully drained everywhere, the old handlers and type are removed.

The mapping layer makes this narrower than it would otherwise be: since the aggregate raises an internal domain event and never references the public contract directly, the version being emitted for any given domain event is entirely a property of its mapper. Adding a field the aggregate does not currently expose, changing which type a domain event produces, or having one domain event start producing an additional integration event, are all changes confined to a mapper - the aggregate and the domain event it raises are unaffected.

## Part 8 - What fan-out guarantees, and what remains a caller's responsibility

The outbox guarantees: an integration event, once produced by a mapper during a successful save, will eventually be fanned out to every handler registered for its type at least once, including across a crash or restart; no two running instances ever claim the same outbox message at the same instant; and fan-out retries back off exponentially and eventually dead-letter rather than continuing forever.

What remains a caller's responsibility:

- **Choosing whether a domain event needs a mapper at all**, and whether that mapper should produce zero, one, or several integration events.
- **Registering for the inbox pipeline.** Any module that hosts an integration event handler - whether it consumes its own events or another module's - must call the inbox registration during startup so its schema is known to fan-out, even if that module never publishes anything of its own. See `background-jobs-inbox-design.md`.
- **Schema evolution discipline** (Part 7): additive changes to an existing type are safe; anything else needs a new type and a transition window.
- **Recognizing dead letters.** A message in `OutboxDeadLetter` succeeded only at "stop retrying to fan out," not at "every handler ran." Nothing pages anyone about this; it requires a deliberate query or an alert that has not yet been built.

Everything about what happens to a fanned-out event afterward - idempotency, ordering, speed, dead-letter recognition on the handler side - is `background-jobs-inbox-design.md`'s responsibility list, not this one.

## Part 9 - Edge cases

| Situation | Behavior |
|---|---|
| Crash between commit and fan-out | Handled - the outbox row commits atomically with the business change; any instance's next poll picks it up. |
| An instance dies mid-batch, after claiming | Handled - lease expiry lets another instance reclaim the row. |
| Two instances racing for the same outbox row | Handled - row-level locking with skip-on-conflict. |
| Fan-out partially succeeds (some Inbox rows written, then a failure) | Handled - fan-out for one message is one transaction; a failure rolls all of it back, leaving nothing partial to reconcile. |
| Fan-out retries repeatedly with no eventual success | Handled - exponential backoff, eventual dead-letter into the publishing module's `OutboxDeadLetter`. |
| A mapper throws | Handled by failing fast - the transaction aborts; a throwing mapper indicates a defect, not a transient condition. |
| A domain event with no registered mapper | Handled, silently - produces no integration events. |
| An integration event raised directly from an aggregate, bypassing a mapper | Handled - rejected at runtime with an explicit error. |
| An event raised with zero registered handlers | Handled, silently - fan-out finds nothing to insert and deletes the outbox row immediately. |
| Unbounded outbox table growth | Mostly handled - a successfully fanned-out message is deleted immediately. Open for `OutboxDeadLetter` itself, which has no retention or purge job yet. |
