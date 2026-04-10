# Multi-Tenancy Architecture

For a SaaS platform like Mirama, the most critical engineering challenge isn't just storing data, it is guaranteeing isolation. We built our request pipeline around a Logical Multi-Tenancy model to answer two foundational questions. 1) Whose data is this? and 2) Who can see it? By baking these checks into the infrastructure, we've made unauthorized access a structural impossibility rather than a pinky-promise in a policy doc.

---

## Architectural Philosophy

Mirama uses a **Single-Database, Shared-Schema** model of multi-tenancy. Rather than spinning up a separate database per customer (I don't have the money for that, and this is usually more required in very high security environments like Gov or Finance), all tenant data lives in the same schema. Isolation is enforced at the **application layer** through a custom ORM infrastructure extensions rather than relying on database-engine features.

This is a deliberate trade-off. The model prioritizes:

- **Developer velocity:** A single migration file deploys across all tenants simultaneously. No fleet-wide rollouts.
- **Cost efficiency:** Thousands of tenants can be supported on a single RDS instance with zero infrastructure overhead per new signup.
- **Operational simplicity:** One connection pool, one monitoring surface, one backup strategy.

To keep things airtight, we customized the ORM across our API server to automatically inject TenantId and OrganizationId into every query and resource creation. We did this because relying on a developer to manually add a .where('organizationId', ...) to every single database call is a recipe for a very bad day. By moving that logic into the ORM itself, we’ve made data isolation the default behavior, effectively "human-proofing" the codebase against accidental leaks.

### How It Works

When a request enters the pipeline, the middleware instantiates a `ScopedDb` client bound to the current session context. This client acts as a scoped proxy over the standard Prisma client and intercepts every operation before it reaches the database:

- **Reads:** Automatically injects `tenantId` or `organizationId` into every `where` clause.
- **Writes:** Automatically stamps new records with the correct owner IDs.
- **Safety Net:** If a query is attempted without a valid scope in context for example, during a misconfigured middleware chain, the client throws a hard exception rather than silently falling back to an unscoped query. There is no silent failure mode.

The result is that a developer literally cannot write application code that leaks data across organizations. The wrong data is structurally unreachable from `ctx.db`.

### Isolation Scopes

Not all data is scoped the same way. Three distinct access patterns are defined:

| Scope | What It Covers | Who Can Access It |
|---|---|---|
| `Tenant` | Subscription data, billing, global settings | Root User of the tenant only |
| `Organization` | Projects, Tasks, Members, Teams | All members of that organization |
| `Inclusive` | System defaults (e.g., built-in IAM roles) | Readable by all; modifiable by none |

**Pros:**

- Zero per-tenant infrastructure cost makes early-stage growth extremely economical.
- Migrations are a single, atomic operation rather than a fleet-wide rollout.
- Monitoring and observability tooling only needs to cover one database.

**Cons:**

- Isolation correctness depends entirely on application-layer discipline; a raw SQL escape or misconfigured middleware is a security event, not just a bug.
- This model is unsuitable for customers requiring physical data residency (see [Section 5](#5-scalability--limitations)).

---

## 2. The Ownership Hierarchy

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

- **Tenant:** The billing entity. Created automatically on signup. Every piece of data in the system ultimately traces back to a single Tenant, which maintains a 1:1 relationship with its Root User.
- **Organization:** The day-to-day workspace. An agency might run within a single organization; a larger enterprise might create separate organizations per business unit or client. Teams, members and all project data live here.
- **Resources:** Projects, Tasks, and Milestones are always children of a specific Organization. They cannot exist without one and they cannot be accessed outside of one.

---

## 3. Request Lifecycle & Security Pipeline

Every incoming API request passes through a functional middleware stack before any handler is invoked. The philosophy is to **fail fast and fail loudly** rather than allowing ambiguous access through.

1. **Identity Verification:** The JWT is decoded to retrieve the `userId` and the currently active `organizationId`. An expired or tampered token terminates the request at this step.

2. **IDOR Protection:** The middleware validates that any resource IDs in the URL path (e.g., `/api/org/:orgId/project/:projectId`) match the IDs encoded in the user's session. A mismatch returns a `403 Forbidden` before the database is ever contacted. This is a primary defense against [Insecure Direct Object Reference (IDOR)](https://owasp.org/www-project-top-ten/) attacks.

3. **Context Injection:** A `ScopedDb` instance is created and attached to the request context as `ctx.db`.

4. **Handler Execution:** Developers write standard Prisma queries against `ctx.db`. Organization-level scoping is invisible and automatic.

---

## 4. Design Decisions & Trade-offs

### Application-Layer Isolation vs. Postgres Row-Level Security (RLS)

Isolation is enforced in the ORM layer rather than using Postgres RLS.

**Pros:**

- **Portability:** Security logic is database-agnostic. Migrating storage engines or introducing a caching layer like Redis does not require rewriting SQL policies.
- **Visibility:** Isolation logic lives in the TypeScript codebase — it is explicitly auditable, unit-testable, and reviewable during pull requests. No context-switching between application code and database policy definitions.
- **Testability:** ORM-layer rules can be fully exercised in unit tests. RLS policies require a running database to validate.

**Cons:**

- **Trust boundary:** Postgres RLS enforces isolation at the engine level, meaning a raw SQL escape would still hit RLS policies. Application-layer isolation does not have this backstop; discipline in code review is the equivalent safety measure.
- **Test surface:** Every new data access pattern must be verified against the isolation rules, adding a consistent (and necessary) testing overhead.

### JWT-Based Organization Scoping

The user's currently active `organizationId` is embedded directly in the JWT payload.

**Pros:**

- **Zero extra round-trips:** There is no need to query "does this user belong to this org?" on every request. The signed token is a stateless, cryptographically verifiable proof of membership.
- **Edge compatibility:** Stateless JWTs work equally well on the API server, at a CDN edge, or inside SSR functions — no session store required at the boundary.

**Cons:**

- **Stale sessions:** If a user is removed from an organization, their existing JWT may carry a now-invalid `organizationId` until the token expires. This is mitigated through short token lifetimes and explicit revocation events triggered by membership changes.

---

## 5. Scalability & Limitations

### Strengths

The shared-schema model scales horizontally on a single database instance. New customer signups incur zero infrastructure overhead and are live the moment their account is created.

### The Noisy Neighbor Problem

Since all tenants share the same connection pool and compute resources, a single tenant with an exceptionally heavy workload can degrade response times for others. This is mitigated through application-layer rate limiting, but it remains an inherent characteristic of logical multi-tenancy.

### Compliance Ceilings

This model is intentionally designed for B2B SaaS teams. It is **not** suited for industries requiring physical data residency or hardware-level separation, GovCloud, financial services with strict locality requirements, or healthcare under strict HIPAA mandates. If Mirama ever targets those segments, the architecture would evolve towards a **Pooled + Silo** hybrid model, where high-compliance tenants receive dedicated isolated infrastructure while standard tenants remain on shared infrastructure.

---

## 6. IAM Integration

Tenancy boundaries define *which data* a tenant can access. They say nothing about *what a specific user* inside e.g. an organization is allowed to do with that data. That responsibility belongs to the fine-grained authorization layer.

Mirama uses a **Policy-Based Access Control (PBAC)** system for this, described in the companion document: [Permissions & Authorization System](permissions.md).

The two systems are complementary by design. The tenancy layer is the outer wall that separates organizations from each other and the PBAC layer is the set of locks inside that determines what each person within an organization can do.
