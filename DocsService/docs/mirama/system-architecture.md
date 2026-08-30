# System Architecture

This document covers the storage and synchronization architecture that supports the delegation-aware scope described in [Requirements](requirements.md). The reasoning behind targeting this scope in the first place lives in [Market Analysis](market-analysis.md).

---

## Dual Storage Approach

PostgreSQL remains the system of record for tenant and organization data, financial ledgers and transactional state. A second store, purpose built for relationships and retrieval, sits alongside it for the features that are graph problems rather than relational-report problems: blast-radius analysis, subcontractor payout chains and contract-clause-aware billing all involve tracing a change through several hops of dependency, which a flat relational model handles through increasingly expensive recursive queries.

Blob storage, backed by S3, stays exactly as it is today for asset originals, proxies and version history.

### What Stays As Is

PostgreSQL 18 continues as the backing store for every module, running on RDS in production and Docker locally, and S3 continues to hold assets under the deterministic path scheme `tenant/{tenantId}/org/{orgId}/project/{projectId}/task/{taskId}/asset/{assetId}/version/{versionId}` once the upload pipeline in [Requirements](requirements.md) section 1.2 exists. The modular monolith boundary, `Mirama.Modules.Identity`, `.PM`, `.Clients` and whichever modules the billing, payout and delegated-work requirements are organized into, along with the Contracts-project pattern for synchronous cross-module calls, all carry forward without change. See [Requirements](requirements.md) section 0 for exactly how far along each module is today; in short, Identity and PM are implemented, Clients has code but no applied migration, and everything else in this document is designed ahead of the corresponding backend work rather than describing something already running.

The Outbox and Inbox models already sitting in `Mirama.SharedKernel` are the single most valuable piece of existing infrastructure for this scope. They exist today but are not yet fully wired to a consumer. That is the exact mechanism the dual-storage pattern needs: an atomic local commit followed by an async, idempotent projection into the second store. It does not need to be designed, only finished and pointed at a new consumer.

### Which Graph and Vector Engine

The graph and vector store should default to Apache AGE for the graph and pgvector for embeddings, both as extensions on the existing PostgreSQL instance, rather than a separate service such as Cosmos DB. Cosmos DB is Azure native, and the rest of the stack, RDS, S3 and ECS, is AWS. Introducing a second cloud provider adds cross-cloud egress cost and a second identity and networking surface to manage, which is a real tax to take on before it is needed, particularly given the platform's own infrastructure notes already flag AWS cost as a live concern for a personal project.

AGE's openCypher support is sufficient for blast-radius queries three to five hops deep at the node counts a tenant would realistically have at this stage. If graph or vector load genuinely outgrows AGE, or a hard multi-region requirement appears later, that workload can move to Cosmos DB or a dedicated graph service without disturbing Postgres as the system of record. The Outbox pattern makes that swap low risk, since only the relay's consumer changes, not the domain event contracts or the Postgres side.

### Synchronization Through the Outbox

```
Handler mutates an Aggregate and raises a Domain Event
                |
                v
DbContext.SaveChangesAsync commits the business row and the OutboxMessage row
in one Postgres transaction
                |
                v
HTTP 200 returns immediately

-- asynchronously --

Outbox Relay polls OutboxMessages
                |
                v
Idempotent upsert into AGE and pgvector, nodes, edges and embeddings
                |
                v
ProcessedAtUtc is stamped, retried with backoff on failure,
moved to a dead letter queue after repeated failures
```

This is the same shape already documented in [Cross-Module Communication](modules/cross-module-communication.md) for in-process domain events. The only new work is a consumer that projects specific domain events, a subcontractor assigned, a milestone approved, a change order created, an invoice line generated, an asset version linked, into graph nodes and edges, and where relevant into a vector embedding.

### Graph Topology

Nodes: `Client`, `Project`, `Milestone`, `Task`, `Subcontractor`, `AssetVersion`, `ChangeOrder`, `ContractClause`, `Invoice`, `LedgerEntry`, `PayoutSplit`.

Key traversal edges:

* `(Subcontractor)-[ASSIGNED_TO]->(Task)`, who owns what, for scoped visibility.
* `(Task)-[BLOCKS]->(Task)`, the existing dependency model, now graph native instead of a recursive CTE.
* `(ChangeOrder)-[MODIFIES]->(Milestone)` and `(ChangeOrder)-[INCURS_FEE]->(LedgerEntry)`, the scope guardrail chain.
* `(LedgerEntry)-[SPLITS_TO]->(PayoutSplit)-[PAID_TO]->(Subcontractor)`, the payout chain, queryable end to end for audit-proof invoicing.
* `(AssetVersion)-[DERIVED_FROM]->(AssetVersion)`, version lineage as a real traversable edge instead of a foreign key chain. A simple parent-version column in Postgres is enough for a basic lineage view before the graph exists.
* `(ContractClause)-[GOVERNS]->(Project)` and `(ContractClause)-[CAPS]->(LedgerEntry)`, retainer caps and overage terms.

Blast-radius query example, the headline capability this layer exists for: given a proposed milestone date change, traverse `BLOCKS` edges outward to every dependent task, follow `ASSIGNED_TO` to affected subcontractors, and follow `INCURS_FEE` and `SPLITS_TO` to compute the fee delta. One bounded traversal, instead of a multi-join recursive query.

Vector store use: embeddings of client briefs, contract clauses and annotation feedback text, grounding the [AI Platform Intelligence](ai/index.md) roadmap, particularly the Conversational Copilot and Annotation Summarization features, in retrieved graph context rather than raw prompt stuffing.

### Multi-Tenancy

The platform has an existing, deliberate decision, covered in [Security Design](security-design.md), to enforce isolation at the application and ORM layer rather than through Postgres Row-Level Security, specifically to stay database-agnostic and keep isolation logic in reviewable code. That reasoning holds here too. The one addition needed is a fourth scope tier, `Subcontractor`, sitting alongside the existing `Tenant`, `Organization` and `Client` tiers, visible only to assigned tasks and its own payout records, structurally mirroring the already-designed `Client` portal scope pattern. On the graph side, every traversal opens with a mandatory tenant boundary node constraint, the same discipline applied at the Postgres layer today.

### Build Sequence

The dual storage layer is not needed for every requirement in this scope, and given how much of the system is still unbuilt, per requirements section 0, sequencing matters more than it would on a mature codebase. A reasonable order, driven by what each requirement depends on rather than by any particular market segment: finish the asset upload and storage pipeline (1.2 to 1.4) so the already-designed `Asset`, `AssetVersion` and `AssetFeedback` aggregates have something to operate on, wire the review and proofing endpoints on top of them (1.6), build time tracking (1.10) since payouts and billing both depend on it, then subcontractor access (1.15) and scope guardrails, all of which are shallow, well-bounded relational work that a normal Postgres foreign key model handles without difficulty. The graph and vector layer earns its place once blast-radius analysis and the split payout chain (1.16) are the features actually being built, not before. Standing up a graph database ahead of a graph-shaped feature would be engineering investment ahead of the need for it.
