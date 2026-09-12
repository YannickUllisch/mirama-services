# Outbox Flow - Sequence Diagram

This traces one domain event from the moment an aggregate raises it through to fan-out: the point where it becomes one row per interested handler in some module's Inbox. It corresponds to `background-jobs-outbox-design.md`. What happens to an Inbox row afterward - claiming it, running its handler, retrying, ordering, dead-lettering - is a separate diagram: `background-jobs-inbox-flow.md`.

Two independent gates decide what a domain event produces:

- Whether a **domain event handler** is registered for it - if so, it runs synchronously, inside the same transaction, before it commits.
- Whether an **integration event mapper** is registered for it - not whether an integration event *type* happens to exist in code, but whether a mapper is actually wired up to produce one from this specific domain event. No mapper means nothing is ever written to the outbox.

Fan-out below inserts one row into the Inbox per interested handler - it makes no difference whether a handler lives in the same module that raised the original event or a different one; see the design doc, Part 4.

A processed message is marked, not deleted - a separate worker archives it to a history table later, on its own much slower schedule; see the design doc, Part 9. That worker isn't shown below since it runs independently of this flow.

```mermaid
sequenceDiagram
    autonumber
    participant Agg as Aggregate
    participant DC as DbContext
    participant DH as DomainEventHandler
    participant Map as Mapper
    participant Outbox as Outbox / DLQ
    participant Proc as OutboxProcessor (fan-out)
    participant Inbox as Inbox

    Agg->>Agg: raise domain event
    DC->>DC: SaveChangesAsync() collects it

    alt domain event handler registered
        DC->>DH: HandleAsync(event)
        DH-->>DC: writes on the same DbContext
    else no handler registered
        Note over DC: dropped - not an error
    end

    alt integration event mapper registered
        DC->>Map: Map(event)
        Map-->>DC: zero, one, or many integration events
        DC->>Outbox: add one OutboxMessage row per event
    else no mapper registered
        Note over DC: nothing produced
    end

    DC->>Outbox: base.SaveChangesAsync() - one commit for all of it

    Note over Proc,Outbox: separately, on a timer
    Proc->>Outbox: claim batch (FOR UPDATE SKIP LOCKED)
    Outbox-->>Proc: claimed rows, leased

    loop for each message, one local transaction
        Proc->>Proc: resolve type, find every registered handler
        alt fan-out succeeds
            Proc->>Inbox: insert one row per handler
            Proc->>Outbox: mark message processed
        else fails
            Proc->>Outbox: retry with backoff
            Note over Proc: past MaxRetries: move to DLQ
        end
    end
```
