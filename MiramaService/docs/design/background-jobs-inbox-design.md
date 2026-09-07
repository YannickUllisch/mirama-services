# Inbox & Handler Execution

## Overview

This document picks up exactly where `background-jobs-outbox-design.md` leaves off: an `InboxMessage` row already exists in some module's own schema, put there by that outbox's fan-out step. It covers everything from that point onward - claiming it, running its handler, ordering, retrying, and dead-lettering. It does not cover how a domain event becomes an integration event or how fan-out decides who gets a row; see the outbox document for that.

Every module that hosts an integration event handler - whether that handler reacts to the module's own events or another module's - has exactly one inbox table and one background processor draining it, registered once at startup. A handler never knows, and never needs to know, whether the event it is reacting to originated in its own module or somewhere else; the row it was handed already carries everything it needs.

## Part 1 - What an InboxMessage is

One `InboxMessage` row is one unit of pending work for **one specific handler**. If three handlers across three modules are registered for the same integration event, fan-out produces three `InboxMessage` rows, in up to three different modules' schemas, each entirely independent of the other two. A row carries:

- The event's payload (JSON) and its assembly-qualified CLR type name, so any module can deserialize it without needing the publishing module's `*.Contracts` assembly.
- Which handler it is for (a full CLR type name), resolved from this module's own dependency injection container at drain time.
- The aggregate the event concerns, for ordering (Part 3).
- Its own lease fields and its own retry count - nothing here is shared with the outbox message it was fanned out from, or with any other handler's row for the same event.

A row is deleted the moment its handler succeeds, or once it is moved to `InboxDeadLetter` after exhausting its retries (Part 5). The live table therefore only ever holds work that might still succeed.

Alongside that, a row also carries the small envelope its originating `OutboxMessage` was stamped with - `OrganizationId`, `TenantId`, `TraceId`, `CorrelationId`, and an open `Headers` JSON column - copied verbatim at fan-out time (see the outbox document, Part 3). `TraceId`, `CorrelationId`, and `Headers` are purely informational: a handler is never required to read them, and they exist for operational queries and tracing - `TraceId` above all, for following one request across every module it touched. `OrganizationId` and `TenantId` are not purely informational, though - see Part 2 for why.

## Part 2 - Draining an inbox

Each module's inbox processor runs against that module's own `DbContext`, on its own timer, independently of every other module's inbox processor and of that module's own outbox processor. On each poll, it opens a brief transaction, selects unprocessed, currently-available, unleased rows with `FOR UPDATE SKIP LOCKED`, leases them, and commits - the identical mechanism the outbox side uses for claiming, applied to this module's own table instead.

For each claimed item, in its own scope and `DbContext` instance:

1. Its assembly-qualified type name is resolved directly via `Type.GetType` - no per-module resolver or Contracts-assembly binding needed, since every module's assembly is already loaded in this one process.
2. Its JSON payload is deserialized into that type.
3. The item's `OrganizationId`, `TenantId`, `TraceId`, `CorrelationId`, and `Headers` are pushed into that scope's `IAmbientMessageContext` before anything else touches the `DbContext` - see below for why this step exists.
4. The one handler the row names is resolved from this module's own dependency injection container (by matching its full type name against everything registered for `INotificationHandler<T>` for that event type) and invoked.
5. On success, the row is deleted. On failure, see Part 5.

A handler here is a plain `INotificationHandler<T>`, discovered by the same dependency-injection scan that would find it for a purely in-process `Publish` call. It is not aware it is being invoked from an inbox at all.

**Why step 3 exists.** There is no HTTP request behind any of this - it is a background processor - so `IRequestContextProvider`, which every module's `DbContext` reads for tenant/organization ownership enforcement on every `SaveChangesAsync` call, has no JWT claim to read `OrganizationId`/`TenantId` from. `IAmbientMessageContext` closes that gap: a small scoped service that `IRequestContextProvider` falls back to whenever there is no `HttpContext` at all, populated once per claimed item from the values above. Without it, a handler that creates or modifies a tenant- or organization-owned entity on the same `DbContext` would fail outright - `AuditableUnitOfWorkDbContext` requires an `OrganizationId` to persist an organization-owned entity, and none would be reachable from a background processor otherwise. With it, that entity is stamped with the same organization/tenant the original request ran under, exactly as if that request had made the write itself. A handler that wants to read `TraceId`, `CorrelationId`, or `Headers` directly - for logging, or a business decision - can simply inject `IAmbientMessageContext` like any other scoped service; no change to `INotificationHandler<T>` or to `IIntegrationEvent` was needed for this.

## Part 3 - Ordering

Most integration events carry no ordering requirement and should be designed as idempotent snapshots - current state, not a delta - so that delivery in any order converges to the same result, and full parallelism is available by default.

For events that do require strict ordering relative to each other for the same aggregate, an event type opts in via an empty marker interface (`IOrderedIntegrationEvent`). After claiming a batch, an inbox processor groups items whose resolved type carries that marker by aggregate identity and processes each group's items sequentially, in occurrence order, on one task; different aggregate groups, and every non-ordered item, run concurrently.

This guarantee holds within what one processor claims in one pass, against that module's own queue. It does not extend across two different running instances of the same module claiming different rows for the same aggregate at the same moment: claim-time row locking prevents two instances from claiming the *same* row twice, but nothing prevents one instance from claiming an aggregate's earlier item while another instance, in an independent poll, claims that aggregate's later item - each instance correctly orders what it claimed, but the two groups can still run concurrently against each other. Closing this fully requires making the claim query aggregate-aware for ordered types, or a lock held for the full processing duration of an aggregate's head-of-queue item, rather than only during the brief claim window. Neither exists today.

Because ordering is enforced independently per module, one module's ordering guarantee for a given aggregate has no relationship to any other module's - a slow or backed-up consumer never affects how promptly, or in what order, any other module processes the same underlying events.

## Part 4 - Locking and horizontal scaling

Every module's inbox processor is designed to run as more than one copy of the same process at once, polling only that module's own table, with no coordinator between instances or between modules.

The lease is what makes a crash safe: if an instance claims a batch and dies mid-handler, it never deletes the row. Once the lease expires, another instance's next claim sees it as eligible again.

The lease does not renew while a handler is running. If a handler legitimately runs longer than the lease duration, another instance can conclude the lease expired and reclaim the same item while the first instance is still mid-handler, and both could then invoke the same handler concurrently. The lease duration is set comfortably above the slowest handler's expected runtime as a mitigation, but the underlying requirement is unconditional: every integration event handler tolerates being invoked more than once for the same event. At-least-once delivery is the actual guarantee this system provides; exactly-once is not offered, and no outbox/inbox-style system provides it without an idempotency requirement on the consumer somewhere.

## Part 5 - Retry and dead-lettering, scoped to one handler

Handler failure is tracked entirely on the `InboxMessage` row itself. A failure increments that row's own retry count with exponential backoff; once the module's configured limit (`InboxOptions.MaxRetries`) is reached, the row moves to that module's `InboxDeadLetter` table - payload, handler name, retry count, and last error preserved for inspection or manual replay - and is removed from the live inbox.

This is scoped as narrowly as it can be. If a message has three interested handlers across three modules, a permanently broken one dead-letters entirely on its own, in its own module's `InboxDeadLetter`, while the other two continue to succeed or fail independently. Nothing about one handler's retry count, backoff schedule, or dead-letter threshold is visible to, or affected by, any other handler for the same event - including another handler for the same event living in the very same module. It is also independent of the outbox that fanned the message out in the first place: once an `InboxMessage` row exists, the publisher is done with it and has no further involvement in, or visibility into, whether it ever succeeds (see the outbox document, Part 8).

## Part 6 - Parallelization

Ordered groups (Part 3) each run on their own task, processing their items one at a time in order; every item within a group, and every non-ordered item, gets its own fresh scope and `DbContext` instance - a hard requirement, since a `DbContext` is not safe for concurrent use. Non-ordered items run through a single bounded-parallelism pool, sized per module (`InboxOptions.MaxDegreeOfParallelism`) and kept modest, since handlers perform real I/O - database writes and, for integration event handlers specifically, potentially outbound calls to other systems.

## Part 7 - What the inbox guarantees, and what remains a handler author's responsibility

The inbox guarantees: a fanned-out item will eventually be delivered to its named handler at least once, including across a crash or restart; its delivery is entirely independent of every other handler for the same event, in this module or another; no two running instances of this module ever claim the same item at the same instant; delivery order matches raise order within what one instance claims in one pass, for event types that opt into ordering; retries back off exponentially and eventually dead-letter rather than continuing forever; and an entity a handler creates or modifies inherits the original request's organization/tenant ownership automatically (Part 2), the same as it would inside that original HTTP request.

What remains a handler author's responsibility:

- **Idempotency.** At-least-once delivery is the actual contract; a handler must tolerate being invoked more than once for the same event.
- **Speed.** A slow handler occupies a limited concurrent slot under this module's non-ordered pool, or blocks every later item for its aggregate under ordered processing. Slow external work is better enqueued than performed inline inside a handler.
- **Not assuming order** unless an event type explicitly opts into it, and understanding the cross-instance limitation in Part 3 before relying on ordering for something correctness-critical.
- **Recognizing dead letters.** An item in `InboxDeadLetter` succeeded only at "stop retrying," not at "the business outcome happened." Nothing pages anyone about this; it requires a deliberate query or an alert that has not yet been built.

## Part 8 - Edge cases

| Situation | Behavior |
|---|---|
| An instance dies mid-batch, after claiming | Handled - lease expiry lets another instance reclaim the item. |
| Two instances racing for the same inbox item | Handled - row-level locking with skip-on-conflict. |
| One handler fails while other handlers for the same event, in this or another module, already succeeded | Handled - each handler has its own row, in its own module's schema; one failing has no effect on any other. |
| A handler retries repeatedly with no eventual success | Handled - exponential backoff, eventual dead-letter into this module's own `InboxDeadLetter`, scoped to that one handler only. |
| Ordering within one module's own claimed batch | Handled - grouped by aggregate identity for event types that opt in. |
| Ordering across concurrently-running instances of the same module, for the same aggregate | Open - only closed by an aggregate-aware claim guard or a lock held for the full processing duration, neither of which exists yet. |
| A handler running longer than the lease duration | Open - no lease renewal exists; mitigated only by setting the lease generously and requiring idempotent handlers. |
| Visibility into dead letters or a stalled inbox | Open - no alerting exists; requires a manual query or a health check/metric not yet built. |
| Unbounded inbox table growth | Mostly handled - a successfully handled item is deleted immediately. Open for `InboxDeadLetter` itself, which has no retention or purge job yet. |
| A handler creates or modifies a tenant- or organization-owned entity | Handled - `IAmbientMessageContext` supplies `OrganizationId`/`TenantId` from the claimed item before the handler runs, so ownership enforcement behaves as it would inside the original HTTP request (Part 2). |
