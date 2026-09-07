# Inbox Flow - Sequence Diagram

This picks up exactly where `background-jobs-outbox-flow.md` ends: a row already exists in some module's own Inbox. It corresponds to `background-jobs-inbox-design.md`. It says nothing about how that row got there - that's the outbox's job, covered separately.

```mermaid
sequenceDiagram
    autonumber
    participant Inbox as Inbox / DLQ
    participant Proc as InboxProcessor
    participant H as IntegrationEventHandler

    Note over Inbox: a row already exists here

    Proc->>Inbox: claim batch (FOR UPDATE SKIP LOCKED)
    Inbox-->>Proc: claimed rows, leased

    loop for each claimed row
        Proc->>Proc: resolve type, deserialize, resolve handler
        Proc->>H: HandleAsync(event)
        alt succeeds
            H-->>Proc: success
            Proc->>Inbox: delete row
        else fails
            H-->>Proc: exception
            Proc->>Inbox: retry with backoff
            alt retries exhausted
                Proc->>Inbox: move to DLQ, delete row
            end
        end
    end
```

Two things worth keeping in view when reading this against the outbox diagram: nothing here knows or cares whether this module raised the underlying domain event itself or received it from another module's fan-out - Part 1 of the design doc explains why that distinction is deliberately erased by the time execution reaches this point. And a second module's inbox processing the same underlying event is a completely separate run of this same diagram, against that module's own Inbox, with its own retry count and its own dead letters - never shown here because it has no interaction with this one at all.
