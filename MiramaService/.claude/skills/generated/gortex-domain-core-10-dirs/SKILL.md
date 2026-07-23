---
name: gortex-domain-core-10-dirs
description: "Work in the Domain/Core +10 dirs area — 87 symbols across 22 files (77% cohesion)"
---

# Domain/Core +10 dirs

87 symbols | 22 files | 77% cohesion

## When to Use

Use this skill when working on files in:
- `src/Mirama.SharedKernel/Abstractions/Common/Interfaces/IDispatcher.cs`
- `src/Mirama.SharedKernel/Abstractions/Common/Interfaces/INotificationHandler.cs`
- `src/Mirama.SharedKernel/Abstractions/Common/Interfaces/IRequest.cs`
- `src/Mirama.SharedKernel/Abstractions/Domain/Core/AggregateRoot.cs`
- `src/Mirama.SharedKernel/Abstractions/Domain/Core/Entity.cs`
- `src/Mirama.SharedKernel/Abstractions/Domain/Core/IAuditable.cs`
- `src/Mirama.SharedKernel/Abstractions/Domain/Core/OrganizationAggregateRoot.cs`
- `src/Mirama.SharedKernel/Abstractions/Domain/Core/OrganizationEntity.cs`
- `src/Mirama.SharedKernel/Abstractions/Domain/Events/IDomainEvent.cs`
- `src/Mirama.SharedKernel/Abstractions/Domain/Events/IDomainEventEntity.cs`
- `src/Mirama.SharedKernel/Infrastructure/Messaging/Outbox/OutboxMessage.cs`
- `src/Mirama.SharedKernel/Models/Dispatcher.cs`
- `src/Modules/Mirama.Modules.Clients/Mirama.Modules.Clients.Contracts/Events/ClientArchivedEvent.cs`
- `src/Modules/Mirama.Modules.Clients/Mirama.Modules.Clients.Contracts/Events/ClientCreatedEvent.cs`
- `src/Modules/Mirama.Modules.Clients/Mirama.Modules.Clients/Domain/Events/ClientArchived.cs`
- `src/Modules/Mirama.Modules.Clients/Mirama.Modules.Clients/Domain/Events/ClientCreated.cs`
- `src/Modules/Mirama.Modules.Clients/Mirama.Modules.Clients/Domain/Events/ClientPortalInvitationSent.cs`
- `src/Modules/Mirama.Modules.Clients/Mirama.Modules.Clients/Domain/Events/ContactAdded.cs`
- `src/Modules/Mirama.Modules.Clients/Mirama.Modules.Clients/Infrastructure/Persistence/ClientsDbContext.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Infrastructure/Persistence/IdentityDbContext.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM.Contracts/Events/ProjectCreatedEvent.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Infrastructure/Persistence/PMDbContext.cs`

## Key Files

| File | Symbols |
|------|---------|
| `src/Mirama.SharedKernel/Abstractions/Common/Interfaces/IDispatcher.cs` | notification, Publish, TNotification, cancellationToken |
| `src/Mirama.SharedKernel/Abstractions/Common/Interfaces/INotificationHandler.cs` | notification, HandleAsync, cancellationToken |
| `src/Mirama.SharedKernel/Abstractions/Common/Interfaces/IRequest.cs` | IRequest, TResponse |
| `src/Mirama.SharedKernel/Abstractions/Domain/Core/AggregateRoot.cs` | _domainEvents, TID, AggregateRoot, GetDomainEvents, ClearDomainEvents |
| `src/Mirama.SharedKernel/Abstractions/Domain/Core/Entity.cs` | LastModifiedBy, LastModified, Created, CreatedBy, TID, ... |
| `src/Mirama.SharedKernel/Abstractions/Domain/Core/IAuditable.cs` | created, lastModified, lastModifiedBy, LastModified, LastModifiedBy, ... |
| `src/Mirama.SharedKernel/Abstractions/Domain/Core/OrganizationAggregateRoot.cs` | ClearDomainEvents, AddDomainEvent, _domainEvents, GetDomainEvents, @event, ... |
| `src/Mirama.SharedKernel/Abstractions/Domain/Core/OrganizationEntity.cs` | TID, OrganizationId, organizationId, SetOrganizationId, OrganizationEntity |
| `src/Mirama.SharedKernel/Abstractions/Domain/Events/IDomainEvent.cs` | IDomainEvent, OccurredAt |
| `src/Mirama.SharedKernel/Abstractions/Domain/Events/IDomainEventEntity.cs` | IDomainEventEntity, GetDomainEvents, ClearDomainEvents |
| `src/Mirama.SharedKernel/Infrastructure/Messaging/Outbox/OutboxMessage.cs` | Type, OutboxMessage, Id, ProcessedAtUtc, Content, ... |
| `src/Mirama.SharedKernel/Models/Dispatcher.cs` | cancellationToken, Send, Publish, TNotification, cancellationToken, ... |
| `src/Modules/Mirama.Modules.Clients/Mirama.Modules.Clients.Contracts/Events/ClientArchivedEvent.cs` | ClientArchivedEvent |
| `src/Modules/Mirama.Modules.Clients/Mirama.Modules.Clients.Contracts/Events/ClientCreatedEvent.cs` | ClientCreatedEvent |
| `src/Modules/Mirama.Modules.Clients/Mirama.Modules.Clients/Domain/Events/ClientArchived.cs` | ClientArchived |
| `src/Modules/Mirama.Modules.Clients/Mirama.Modules.Clients/Domain/Events/ClientCreated.cs` | ClientCreated |
| `src/Modules/Mirama.Modules.Clients/Mirama.Modules.Clients/Domain/Events/ClientPortalInvitationSent.cs` | ClientPortalInvitationSent |
| `src/Modules/Mirama.Modules.Clients/Mirama.Modules.Clients/Domain/Events/ContactAdded.cs` | ContactAdded |
| `src/Modules/Mirama.Modules.Clients/Mirama.Modules.Clients/Infrastructure/Persistence/ClientsDbContext.cs` | SaveChangesAsync, cancellationToken |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Infrastructure/Persistence/IdentityDbContext.cs` | SaveChangesAsync, cancellationToken |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM.Contracts/Events/ProjectCreatedEvent.cs` | ProjectCreatedEvent |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Infrastructure/Persistence/PMDbContext.cs` | cancellationToken, SaveChangesAsync |

## Connected Communities

- **Domain/Core +4 dirs** (3 cross-edges)

## How to Explore

```
get_communities with id: "community-146"
smart_context with task: "understand Domain/Core +10 dirs", format: "gcx"
```

_`format: "gcx"` returns the [GCX1 compact wire format](../../docs/wire-format.md) — round-trippable, ~27% fewer tokens than JSON. Drop it for JSON output; agents using `@gortex/wire` or the Go `github.com/gortexhq/gcx-go` package decode either._
