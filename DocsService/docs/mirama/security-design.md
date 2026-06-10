# Security Design

This document covers the full security model of the Mirama platform: from authentication and token issuance, through the multi-tenant isolation boundary, down to the fine-grained permission system governing what users can do with the data they can access.

---

## 1. Authentication

### Overview

Authentication in Mirama is handled entirely by **NextAuth** on the Next.js frontend. NextAuth manages the full sign-in lifecycle - credential validation, session creation, token issuance and secure cookie management - via Next.js API routes. No separate identity server is involved.

Mirama supports multiple authentication paths that converge on a single user account. Signing in with Google, a work email or any other configured provider all resolve to the same identity with no duplicate accounts. Provider linking is handled at the account layer of MiramaService: when a new sign-in arrives, the system checks for an existing account with the same verified email address before creating a new record.

### NextAuth Token Flow

1. **Sign-in** - The user authenticates via NextAuth (credentials, OAuth provider, etc.). NextAuth issues a signed JWT and stores it in a secure, HTTP-only cookie on the Next.js domain. The token includes application-specific claims such as `tenantId`, `orgId` and roles, set during the NextAuth `jwt` and `session` callbacks.

2. **Frontend requests** - Client-side requests to the Next.js application read the session from the NextAuth cookie without any additional network call. Sensitive Next.js routes are protected by Next.js Middleware, which verifies the token at the edge before the request reaches any server component or API route handler.

3. **Backend proxying** - Requests matching the path prefix `api/v{versionNumber}` are forwarded by Next.js to the **MiramaService** backend. The NextAuth access token is extracted from the cookie and attached to the forwarded request (typically as a `Bearer` token in the `Authorization` header).

4. **Backend validation** - MiramaService receives the forwarded request, decrypts and validates the token using the shared signing key. On success, the token claims (`tenantId`, `orgId`, `userId`, roles, etc.) are mapped into the .NET `ClaimsPrincipal`, making them available to all downstream handlers and authorization middleware without additional database lookups.

---

## 2. Multi-Tenant Security Boundary

The outermost security boundary in Mirama is strict isolation between tenants. No tenant can ever access another tenant's data. This is enforced at the data layer, not solely at the API layer.

### 2.1 Architectural Philosophy

Mirama uses a **Single-Database, Shared-Schema** model of multi-tenancy. All tenant data lives in the same schema. Isolation is enforced at the **application layer** through custom ORM infrastructure extensions rather than relying on database-engine features.

This is a deliberate trade-off. A single schema means a single migration deploys across every tenant simultaneously - one connection pool, one monitoring surface, one backup strategy. Thousands of organizations can be onboarded with zero infrastructure overhead per signup.

The caveat is that this model puts the full weight of isolation correctness on the application code. That is exactly why the enforcement layer described in section 2.4 is non-negotiable.

### 2.2 Ownership Hierarchy

All data in Mirama flows through a strict top-down ownership tree:

```
Tenant  (billing entity & root owner)
  └── Organization  (the primary operational workspace)
        ├── Members  (users within this org)
        ├── Teams    (functional groups within the org)
        └── Projects
              ├── Tasks
              ├── Milestones
              └── Client Access Records  (external stakeholders)
```

The **Tenant** is the billing entity. Created automatically on signup, every piece of data in the system ultimately traces back to a single Tenant, which maintains a 1:1 relationship with its Root User.

The **Organization** is where day-to-day work happens. An agency might run everything inside a single organization; a larger enterprise might create separate organizations per business unit or client. Teams, members, and all project data live at this level.

**Resources** like Projects, Tasks, and Milestones are always children of a specific Organization. They cannot exist without one and they cannot be accessed outside of one.

**Client Access Records** sit at the project level and represent external stakeholders - customers or project clients - who have been granted scoped access to specific project deliverables. They are not organization members and carry no org-level visibility.

### 2.3 B2B and B2C Considerations

#### B2B is the Natural Fit

For business customers the hierarchy maps directly onto how they already operate. A design agency has one billing relationship with Mirama (the Tenant) but might serve a dozen separate client accounts, each with their own isolated workspace. Team members hop between client organizations without needing separate logins, and billing stays unified under the agency's account.

The model scales smoothly as a B2B customer grows. A startup might begin with a single organization. As they expand into regional teams or separate product lines, they spin up additional organizations under the same tenant without any restructuring. Access control within each organization remains completely independent.

Delegated access is another strength here. An external contractor can be added to one organization without being given any visibility into the others. When the contract ends, removing them from that organization is the only step required.

Where the model strains is at the enterprise tier. Large enterprise customers often have compliance teams that demand physical data separation, not logical isolation. The shared-schema model cannot offer that without a fundamental architectural change. There is also a cross-organization reporting gap: enterprises frequently want consolidated analytics across all their organizations, which means querying across the very isolation boundaries the system is specifically built to enforce.

#### B2C Works, With Product Design Effort

For individual consumer users, the hierarchy introduces concepts that don't map to their mental model. A freelancer who signs up gets a Tenant and an Organization whether they need it or not. The model is not wrong for B2C, but the product layer has to do real work to hide the scaffolding.

The payoff comes the moment that freelancer wants to collaborate. The organization and membership model is already in place underneath - there is no feature to retrofit, no schema migration to run. Solo use is simply a degenerate case of the same structure that handles a fifty-person agency.

#### What Strains Both

The most consistent pressure point across B2B and B2C is the noisy neighbor problem. Because all tenants share the same connection pool and compute, an unusually heavy workload from one customer can degrade response times for others. Application-layer rate limiting softens this but does not eliminate it.

The harder ceiling is compliance. This architecture is not suitable for industries requiring physical data residency or hardware-level separation. GovCloud workloads, financial services with data locality mandates, and healthcare all need a fundamentally different foundation. Mirama is built for design firms and creative studios. For these users, the primary concern is contractual integrity and asset safety, not government-grade separation.

### 2.4 How Isolation Is Enforced

#### The ScopedDb Pattern

The most dangerous assumption in any multi-tenant application is that developers will remember to filter by organization on every single database query. To remove that assumption entirely, the ORM is wrapped with a **ScopedDb** extension - a scoped proxy over the standard Prisma client that intercepts every operation before it reaches the database.

Reads automatically have `tenantId` or `organizationId` injected into the `WHERE` clause. Writes are automatically stamped with the correct owner IDs. If a query is attempted without a valid scope in context - for example during a misconfigured middleware chain - the client throws a hard exception rather than silently falling back to an unscoped query. There is no silent failure mode.

Not all data is scoped the same way. Three distinct access patterns exist within the extension:

| Scope | What It Covers | Who Can Access It |
|---|---|---|
| `Tenant` | Subscription data, billing, global settings | Root User of the tenant only |
| `Organization` | Projects, Tasks, Members, Teams | All members of that organization |
| `Client` | Shared deliverables, approved exports, project artifacts | Invited clients with a valid `CLIENT`-scoped access record |
| `Inclusive` | System defaults (e.g., built-in IAM roles) | Readable by all; modifiable by none |

#### The Request Lifecycle

Every incoming API request passes through a functional middleware stack before any handler executes. The philosophy is to fail fast and fail loudly rather than let ambiguous access pass through silently.

1. The JWT is decoded to retrieve the `userId` and the currently active `organizationId`. An expired or tampered token terminates the request immediately.

2. The middleware validates that any resource IDs present in the URL path - for example `/api/org/:orgId/project/:projectId` - match the IDs encoded in the user's session. A mismatch returns a `403 Forbidden` before the database is ever contacted. This is the primary defense against [Insecure Direct Object Reference (IDOR)](https://owasp.org/www-project-top-ten/) attacks.

3. A `ScopedDb` instance is created and attached to the request context as `ctx.db`. From this point forward, every database operation the handler performs is automatically scoped.

For CLIENT-scoped requests, step 2 additionally validates the invitation token against the `ClientAccessRecord` for the specific project. A client token that does not match the target project's record is rejected before any handler executes.

#### Why ORM-Layer Isolation Instead of Postgres Row-Level Security

Postgres Row-Level Security would enforce isolation at the database engine level, meaning even a raw SQL escape would still run through the policies. Application-layer isolation does not have that backstop - if someone bypasses the ORM entirely, there is nothing catching them below.

The security logic living in the TypeScript codebase is auditable, unit-testable, and visible during pull request reviews without needing to context-switch into SQL policy definitions that most engineers rarely touch. The approach is also database-agnostic, so migrating storage engines or layering in Redis does not require rewriting policies. Code review discipline and thorough test coverage are the equivalent safety net.

#### JWT-Based Organization Scoping

The user's currently active `organizationId` is embedded directly in the JWT payload. This means there is no extra database round-trip to verify "does this user belong to this org?" on every request.

The trade-off is stale sessions. If a user is removed from an organization, their existing JWT may continue to carry a now-invalid `organizationId` until the token expires. This is mitigated with short token lifetimes and explicit revocation events triggered by membership changes.

### 2.5 Why This Matters for Creative Teams

For a design agency, a data leak is not just a security incident on a compliance spreadsheet. It is a breach of trust that can end a client relationship overnight. Creative assets, unreleased brand identities, campaign imagery under NDA, and packaging mockups months ahead of a product launch are some of the most commercially sensitive material a business handles.

The isolation model protects these assets in three concrete ways.

**Contractual air-gapping.** A user from Agency X can never guess, enumerate, or accidentally stumble into a URL that resolves to a mockup belonging to Agency Y. There is no shared namespace, no global search that leaks across boundaries. The data simply does not exist from the perspective of anyone outside the owning organization.

**Leak-proof derivatives.** Creative workflows generate a lot of secondary material - thumbnails, low-resolution previews, transcoded video renditions, compressed board exports. Every one of those generated assets inherits the same strict ownership rules as the original upload. The multi-tenancy model extends into the storage layer so that a thumbnail cannot become a side-channel around the isolation that protects its parent file.

**Audit-ready access logs.** Because `organizationId` is baked into every database transaction through the ScopedDb extension, the system maintains a clean and immutable record of who accessed which asset and when. If a client ever asks for proof of data handling practices, the answer is already sitting in the transaction metadata.

---

## 3. Authorization Model (Policy-Based Access Control)

Inside the tenant boundary, Mirama uses a **Policy-Based Access Control (PBAC)** model. By decoupling permissions from hardcoded role names, the system avoids the classic "role explosion" problem and scales gracefully as the platform adds new feature areas.

### 3.1 The Three-Tier Abstraction

The system does not assign individual permissions directly to users. Instead, it builds up access through three levels of composition:

- **Policy Statement:** The atomic unit of permission. Defines an `Action` (e.g., `project:edit`), a `Resource` (e.g., `*` or a specific ID), and an `Effect` (`ALLOW` or `DENY`).
- **Policy:** A named grouping of statements representing a functional capability (e.g., "Project Management Policy").
- **Role:** A container for one or more policies. Assigning a Role to a user grants them the union of all underlying policy permissions.

This separation means that when a new feature is added to the platform, it requires only a new action string and a policy update - no new hardcoded roles and no schema changes.

### 3.2 Access Scoping

Permissions are partitioned by `AccessScope` to prevent cross-context pollution. An admin managing billing settings should not encounter project-level permission prompts, and a project editor should not need to care that billing exists. A client invited to review a deliverable should have no visibility into internal team workflows at all.

- **`ORGANIZATION` Scope:** Governs organization-level resources - projects, invitations, tasks, members, milestones.
- **`PROJECT` Scope:** Governs resources within a specific project container - tasks, milestones and expenses specific to a single project.
- **`CLIENT` Scope:** Governs what external project clients can access. CLIENT-scoped permissions are always bound to a specific project and can never escalate to organization-level visibility. A client invited to review a deliverable can only see what has been explicitly shared with them. They have no access to the project's internal tasks, member list, or expenses unless a policy explicitly grants it. The CLIENT scope is designed for outward-facing use - stakeholder reviews, client approvals, and shared exports - not for internal collaboration.

The current model's highest explicit scope is `ORGANIZATION`. Tenant-level access is handled implicitly via the Root User relationship established during signup. The architecture is designed so that a `TENANT` scope can be introduced in the future to support multi-admin tenant management or supervised "Support" tooling without restructuring the existing evaluation engine.

### 3.3 Permission Evaluation Flow

When an API endpoint requests an authorization check, the engine evaluates the user's rights through a layered hierarchy:

1. **Tenant Override:** If the user is the Root/Owner of the Tenant, access is granted immediately. Short-circuit, no further evaluation needed.
2. **Client Access:** If the request carries a CLIENT-scoped invitation token rather than a full organization membership JWT, the engine validates the `ClientAccessRecord` for the specific project and evaluates only the `CLIENT` scoped role policies. No organization role is evaluated. This path is entirely isolated from the standard member evaluation flow.
3. **Organization Level:** The engine fetches the user's `Member` record for the current organization and evaluates the policies attached to their `ORGANIZATION` scoped role.
4. **Project Level:** If the action relates to a specific project, the engine fetches the `ProjectMember` record for that project ID and evaluates the `PROJECT` scoped role.
5. **Union Result:** The final permission set is the union of the Org-level and Project-level grants.

> **Note on redundancy:** If an Organization Role grants `project:read` globally, the user automatically has access to all projects without a `ProjectMember` record. Conversely, if the Organization Role is restricted (e.g., a "Guest" role), the user only gains access to projects where an explicit `ProjectMember` record has been created for them. CLIENT-scoped users are always evaluated against their invitation record alone and are never subject to this union logic.

### 3.4 Performance: Redis-Flattened Permission Matrix

Evaluating a full role → policy → statement chain on every API request would be expensive. The system uses a flattening and caching strategy:

- **Flattening:** On login or organization switch, the entire role hierarchy is resolved and collapsed into a flat Permission Matrix - a `Set<string>` of action strings.
- **Redis Caching:** This matrix is stored in Redis, transforming what would be a multi-join relational query into an $O(1)$ key lookup on every subsequent request.
- **Automatic Invalidation:** The cache is invalidated and rebuilt only when the user's role or any underlying policies are updated. Quiet by default, reactive when it matters.

For CLIENT-scoped users, the matrix is derived from the ClientAccessRecord at invitation token validation time and is similarly cached. Because CLIENT permissions are intentionally narrow, the matrix is small and cheap to rebuild.

**Trade-offs:**

- Authorization checks add negligible latency to hot paths.
- A short window exists between a permission change and cache invalidation where the old matrix may still be in use. For most permission updates this is acceptable; for sensitive demotions (e.g., removing an Admin or revoking a client invitation), explicit invalidation should be triggered immediately.

### 3.5 Frontend Integration

The flattened Permission Matrix is passed to the Next.js frontend and stored in a dedicated React Context hook.

- **Conditional Rendering:** UI components read from this hook to show or hide elements (e.g., "Delete" buttons, admin panels, client-only review views) without a network round-trip.
- **Server-Side Rendering (SSR):** The same matrix is available during the SSR pass, ensuring restricted content is never included in the initial HTML payload sent to the client.

CLIENT-scoped sessions receive a restricted matrix that only exposes the shared deliverables view. Internal project UI - member lists, task boards, expense tracking - is never rendered into the SSR output for a CLIENT session.

---

## 4. Default System Roles

A set of predefined roles covers the common configuration cases out of the box, removing the need for every new organization to start from scratch. Custom roles can be composed for teams with more specific requirements.

| Role Name       | Scope          | Intended Use                                                                                         |
|-----------------|----------------|------------------------------------------------------------------------------------------------------|
| Owner           | `ORGANIZATION` | Full control over the tenant, billing, and all sub-organizations.                                    |
| Administrator   | `ORGANIZATION` | Management of members, teams, and projects across the organization.                                  |
| Standard Member | `ORGANIZATION` | Default employee access; can create projects and view public resources.                              |
| Project Lead    | `PROJECT`      | Full administrative control over a specific project and its milestones.                              |
| Editor          | `PROJECT`      | Ability to create and modify tasks and content within a specific project.                            |
| Viewer          | `PROJECT`      | Read-only access for stakeholders or external observers who are organization members.                 |
| Client          | `CLIENT`       | Read-only access to explicitly shared deliverables for external customers or project clients. No organization membership required. Invitation-based. |

---

## 5. Security Considerations & Known Trade-offs

### Action Naming Conventions

Adding entirely new feature areas to the platform - "Analytics," "Budgeting," "Approvals" - requires no schema changes. New action strings are registered and attached to policies. The authorization logic stays the same.

**Risk:** The system requires disciplined naming conventions for actions (e.g., `service:resource:action`). Without an enforced Action Registry, the database will accumulate inconsistent strings over time, making auditing difficult and permission grants ambiguous.

### Sticky Access After Demotion

When a user is demoted from Administrator to a restricted role, their implicit organization-wide access is immediately governed by the new role. However, any explicit `ProjectMember` records created prior to the demotion remain valid until manually removed.

This means a demoted admin may retain direct access to specific private projects via explicit membership records. The current approach deletes all `ProjectMember` records on demotion, since there is no reliable way to distinguish which project memberships were intentional vs. incidental to the admin role. This trades a potential access overhang for a potential access gap (projects the user should still see need to be manually re-added).

**Future improvement:** Allow the authority changing the role to make an explicit decision per `ProjectMember` record at demotion time.

### Client Invitation Revocation

CLIENT access records are tied to a signed invitation token with a configurable expiry. Revoking client access requires both deleting the `ClientAccessRecord` and explicitly invalidating the cached permission matrix for that token. If only the record is deleted without cache invalidation, the client may retain access until the token expires naturally.

Revocation events should always trigger immediate cache invalidation. This is especially important when a client relationship ends mid-project.

### ORM Bypass Risk

Application-layer isolation does not have a database-engine backstop. If someone bypasses the ORM entirely - via a raw SQL escape or a misconfigured admin tool - there is nothing catching them below. Code review discipline and test coverage are the primary mitigation. Any tooling that touches the database directly (migration scripts, admin panels, data export tools) must be audited to ensure it applies tenant scoping manually.
