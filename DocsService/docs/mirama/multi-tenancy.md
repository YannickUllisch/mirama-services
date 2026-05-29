# Multi-Tenancy Architecture

As outlined in the [System Context](system-context.md), Mirama is a visual-first production environment for creative teams managing complex projects, assets and workflows. For a platform like that, the most critical engineering challenge isn't just storing data, it is guaranteeing isolation. We built our request pipeline around a Logical Multi-Tenancy model to answer two foundational questions. 1) Whose data is this? and 2) Who can see it? By baking these checks into the infrastructure, we've made unauthorized access a structural impossibility rather than a pinky-promise in a policy doc.

---

## Architectural Philosophy

Mirama uses a **Single-Database, Shared-Schema** model of multi-tenancy. Rather than spinning up a separate database per customer (I don't have the money for that, and this is usually more required in very high security environments like Gov or Finance), all tenant data lives in the same schema. Isolation is enforced at the **application layer** through a custom ORM infrastructure extensions rather than relying on database-engine features.

This is a deliberate trade-off. A single schema means a single migration deploys across every tenant simultaneously. One connection pool, one monitoring surface, one backup strategy. Thousands of organizations can be onboarded with zero infrastructure overhead per signup. For a platform at Mirama's stage, this is the correct call.

The caveat is that this model puts the full weight of isolation correctness on the application code. That is exactly why the enforcement layer described later in this document is non-negotiable.

---

## The Ownership Hierarchy

All data in Mirama flows through a strict top-down ownership tree:

```bash
Tenant  (billing entity & root owner)
  └── Organization  (the primary operational workspace)
        ├── Members  (users within this org)
        ├── Teams    (functional groups within the org)
        └── Projects
              ├── Tasks
              └── Milestones
```

The **Tenant** is the billing entity. Created automatically on signup, every piece of data in the system ultimately traces back to a single Tenant, which maintains a 1:1 relationship with its Root User. The **Organization** is where day-to-day work happens. An agency might run everything inside a single organization; a larger enterprise might create separate organizations per business unit or client. Teams, members, and all project data live at this level. **Resources** like Projects, Tasks, and Milestones are always children of a specific Organization. They cannot exist without one and they cannot be accessed outside of one.

---

## This Hierarchy in a B2B and B2C Context

The Tenant to Organization hierarchy is simple by design, but the implications of that simplicity change significantly depending on who is actually using the platform. What feels natural to a fifty-person agency feels like unnecessary overhead to a solo freelancer and what scales beautifully for a startup becomes a constraint for a Fortune 500 procurement team. This section walks through both sides honestly.

### B2B is the Natural Fit

For business customers the hierarchy maps directly onto how they already operate. A design agency has one billing relationship with Mirama (the Tenant) but might serve a dozen separate client accounts, each with their own isolated workspace. Their team members hop between client organizations without needing separate logins, and billing stays unified under the agency's account. Nobody needs to explain how context switching works because the structure already mirrors how they think about their engagements.

The model also scales smoothly as a B2B customer grows. A startup might begin with a single organization. As they expand into regional teams or separate product lines, they spin up additional organizations under the same tenant without any restructuring. Access control within each organization remains completely independent, so a developer in the "Platform" org cannot accidentally wander into the "Marketing" org's expense reports. There is no migration step, no plan upgrade required, just a new workspace under the same roof.

Delegated access is another strength here. An external contractor can be added to one organization without being given any visibility into the others under the same tenant. When the contract ends, removing them from that organization is the only step required and it cleans up every associated permission automatically.

Where the model starts to strain on the B2B side is at the enterprise tier. Large enterprise customers often have compliance teams that demand physical data separation, not logical isolation. A security auditor may not care how good the ORM extension is, they want contractual guarantees backed by separate hardware and separate network boundaries. The shared-schema model cannot offer that without fundamentally changing what it is. There is also the cross-organization reporting gap. Enterprises frequently want consolidated analytics across all their organizations within a tenant, which means querying across the very isolation boundaries the system is specifically built to enforce. That is solvable with a dedicated reporting pipeline, but it is additional work that the current architecture does not hand you for free.

### B2C Works, With Product Design Effort

For individual consumer users, the hierarchy introduces concepts that don't map to their mental model. A freelancer who signs up gets a Tenant and an Organization whether they need it or not. They are never going to "switch organizations" because they only have one, and exposing that language in the UI would just create confusion. The model is not wrong for B2C, but the product layer has to do real work to hide the scaffolding.

The payoff comes the moment that freelancer wants to collaborate. Say they want to share a project with a client or bring in another designer for review. The organization and membership model is already in place underneath. There is no feature to retrofit, no schema migration to run, no "upgrade to team plan" gate. Solo use is simply a degenerate case of the same structure that handles a fifty-person agency, which means the upgrade path from individual to team is seamless.

The scaling economics are also genuinely good for a B2C growth pattern. A large number of small, mostly-inactive tenants is exactly the scenario where a shared-schema model performs best. There are no thousands of empty schemas sitting on disk. It is one schema with more rows, and relational databases handle that very comfortably. The marginal cost of a new consumer signup is effectively zero in infrastructure terms.

The real B2C limitation is feature expectations. Consumer users tend to expect things like social login flows, free tier usage caps and instant onboarding with no setup. None of those are blocked by the tenancy model, but they aren't provided by it either. The hierarchy is invisible infrastructure for B2C and the product experience has to be built separately on top of it.

### What Strains Both

The most consistent pressure point across B2B and B2C is the noisy neighbor problem. Because all tenants share the same connection pool and compute, an unusually heavy workload from one customer can degrade response times for others. Application-layer rate limiting softens this but does not eliminate it. It is an inherent characteristic of logical multi-tenancy and the honest answer is that you manage it rather than solve it.

The harder ceiling is compliance. This architecture is not suitable for industries requiring physical data residency or hardware-level separation. GovCloud workloads, financial services with data locality mandates and healthcare all need a fundamentally different foundation. If we ever move into those segments, we’d likely evolve toward a Pooled and Silo hybrid. In that world, high-compliance organizations get their own dedicated infrastructure, while standard tenants stay on the shared pool. It’s an evolution of our current stack rather than a total rewrite, but it represents a significant shift in operational overhead.

Fortunately, that’s not our target. Mirama is built for design firms and creative studios. For these users, the primary concern isn't a complex government audit, it's for the most part contractual integrity and asset safety.

---

## How Isolation Is Enforced

Everything described above only matters if the isolation is actually airtight. Saying "data belongs to an organization" is just a label until the system physically prevents one organization's code path from touching another organization's rows. This section covers how that is achieved across the ORM, the request pipeline, and the authentication layer.

### The ScopedDb Pattern

The most dangerous assumption in any multi-tenant application is that developers will remember to filter by organization on every single database query. They will not. Not consistently, not under deadline pressure, not in a late-night debugging session. So we removed the assumption entirely.

We customized the ORM across our API server with a **ScopedDb** extension, a scoped proxy over the standard Prisma client that intercepts every operation before it reaches the database. Reads automatically have `tenantId` or `organizationId` injected into the where clause. Writes are automatically stamped with the correct owner IDs. If a query is attempted without a valid scope in context, for example during a misconfigured middleware chain, the client throws a hard exception rather than silently falling back to an unscoped query. There is no silent failure mode. A developer literally cannot write application code that leaks data across organizations because the wrong data is structurally unreachable from `ctx.db`.

Not all data is scoped the same way. Three distinct access patterns exist within the extension.

| Scope | What It Covers | Who Can Access It |
|---|---|---|
| `Tenant` | Subscription data, billing, global settings | Root User of the tenant only |
| `Organization` | Projects, Tasks, Members, Teams | All members of that organization |
| `Inclusive` | System defaults (e.g., built-in IAM roles) | Readable by all; modifiable by none |

### The Request Lifecycle

Every incoming API request passes through a functional middleware stack before any handler executes. The philosophy is to fail fast and fail loudly rather than let ambiguous access pass through silently.

First, the JWT is decoded to retrieve the `userId` and the currently active `organizationId`. An expired or tampered token terminates the request immediately.

Second, the middleware validates that any resource IDs present in the URL path, for example `/api/org/:orgId/project/:projectId`, match the IDs encoded in the user's session. A mismatch returns a `403 Forbidden` before the database is ever contacted. This is the primary defense against [Insecure Direct Object Reference (IDOR)](https://owasp.org/www-project-top-ten/) attacks, where an attacker manually edits a URL parameter hoping to reach a resource in a different organization.

Third, a `ScopedDb` instance is created and attached to the request context as `ctx.db`. From this point forward, every database operation the handler performs is automatically scoped. The developer writes standard Prisma code and does not need to think about organization filtering because it has already happened.

### Why ORM-Layer Isolation Instead of Postgres Row-Level Security

An alternative approach would be to enforce isolation at the database engine level using Postgres Row-Level Security. We chose ORM-layer enforcement instead, and the reasoning is worth being explicit about.

The security logic lives in the TypeScript codebase. It is auditable, unit-testable, and visible during pull request reviews without needing to context-switch into SQL policy definitions that most engineers rarely touch. Adding a new data access pattern means the isolation rules are checked as part of the normal development workflow. The approach is also database-agnostic, so migrating storage engines or layering in Redis does not require rewriting policies.

The honest trade-off is that Postgres RLS enforces isolation at the engine level, meaning even a raw SQL escape would still run through the policies. Application-layer isolation does not have that backstop. If someone bypasses the ORM entirely, there is nothing catching them below. Code review discipline and thorough test coverage are the equivalent safety net, which means this approach requires consistent effort to maintain. We consider that trade-off acceptable given the gains in developer velocity, portability, and testability.

### JWT-Based Organization Scoping

The user's currently active `organizationId` is embedded directly in the JWT payload. This means there is no extra database round-trip to verify "does this user belong to this org?" on every request. The signed token is a stateless, cryptographically verifiable proof of membership that works equally well on the API server, at a CDN edge, or inside SSR functions.

The trade-off is stale sessions. If a user is removed from an organization, their existing JWT may continue to carry a now-invalid `organizationId` until the token expires. We mitigate this with short token lifetimes and explicit revocation events triggered by membership changes, but the window exists and is worth being aware of.

---

## Why This Matters for Creative Teams

Everything above is infrastructure. This section is about why that infrastructure exists in the first place.

For a design agency, a data leak is not just a security incident on a compliance spreadsheet. It is a breach of trust that can end a client relationship overnight. Creative assets, unreleased brand identities, campaign imagery under NDA, packaging mockups months ahead of a product launch are some of the most commercially sensitive material a business handles. They are also exactly the kind of material that Mirama is built to manage.

The isolation model protects these assets in three concrete ways.

First, contractual air-gapping. A user from Agency X can never guess, enumerate, or accidentally stumble into a URL that resolves to a mockup belonging to Agency Y. The system treats every organization as its own island. There is no shared namespace, no global search that leaks across boundaries, no "related content" sidebar pulling from the wrong pool. The data simply does not exist from the perspective of anyone outside the owning organization.

Second, leak-proof derivatives. Creative workflows generate a lot of secondary material. Thumbnails, low-resolution previews, transcoded video renditions, compressed board exports. Every one of those generated assets inherits the same strict ownership rules as the original upload. The multi-tenancy model extends into the storage layer so that a thumbnail cannot become a side-channel around the isolation that protects its parent file.

Third, audit-ready access logs. Because `organizationId` is baked into every database transaction through the ScopedDb extension, we maintain a clean and immutable record of who accessed which asset and when. If a client ever asks for proof of data handling practices, the answer is already sitting in the transaction metadata. There is no retroactive log assembly required.

We are not building for intelligence agencies. We are building so that a lead designer can hand off a staging link to a stakeholder and sleep soundly knowing their client's next product launch will not end up on a public mood board because of a missing WHERE clause somewhere in the codebase.

---

## IAM Integration

Tenancy boundaries define *which data* a tenant can access. They say nothing about *what a specific user* inside e.g. an organization is allowed to do with that data. That responsibility belongs to the fine-grained authorization layer.

Mirama uses a **Policy-Based Access Control (PBAC)** system for this, described in the companion document: [Permissions & Authorization System](permissions.md).

The two systems are complementary by design. The tenancy layer is the outer wall that separates organizations from each other and the PBAC layer is the set of locks inside that determines what each person within an organization can do.
