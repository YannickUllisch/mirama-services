# Security Design

This document covers the full security model of the Mirama platform: from authentication and token issuance, through the multi-tenant isolation boundary, down to the fine-grained permission system governing what users can do with the data they can access. Infrastructure-level security is covered in [IaC Strategy](iac-strategy.md).

---

## 1. Authentication

### Overview

Authentication in Mirama is handled entirely by **NextAuth** on the Next.js frontend. NextAuth manages the full sign-in lifecycle — credential validation, session creation, token issuance and secure cookie management — via Next.js API routes. No separate identity server is involved.

Mirama supports multiple authentication paths that converge on a single user account. Signing in with Google, a work email or any other configured provider all resolve to the same identity with no duplicate accounts. Provider linking is handled at the account layer of MiramaService: when a new sign-in arrives, the system checks for an existing account with the same verified email address before creating a new record.

### NextAuth Token Flow

1. **Sign-in** — The user authenticates via NextAuth (credentials, OAuth provider, etc.). NextAuth issues a signed JWT and stores it in a secure, HTTP-only cookie on the Next.js domain. The token includes application-specific claims such as `tenantId`, `orgId` and roles, set during the NextAuth `jwt` and `session` callbacks.

2. **Frontend requests** — Client-side requests to the Next.js application read the session from the NextAuth cookie without any additional network call. Sensitive Next.js routes are protected by Next.js Middleware, which verifies the token at the edge before the request reaches any server component or API route handler.

3. **Backend proxying** — Requests matching the path prefix `api/v{versionNumber}` are forwarded by Next.js to the **MiramaService** backend. The NextAuth access token is extracted from the cookie and attached to the forwarded request (typically as a `Bearer` token in the `Authorization` header).

4. **Backend validation** — MiramaService receives the forwarded request, decrypts and validates the token using the shared signing key. On success, the token claims (`tenantId`, `orgId`, `userId`, roles, etc.) are mapped into the .NET `ClaimsPrincipal`, making them available to all downstream handlers and authorization middleware without additional database lookups.

---

## 2. Multi-Tenant Security Boundary

The outermost security boundary in Mirama is strict isolation between tenants. No tenant can ever access another tenant's data; this is enforced at the data layer, not solely at the API layer.

The multi-tenancy model is documented in full in [Multi-Tenancy](multi-tenancy.md). The key security properties are:

- Every resource is scoped to a `tenantId` at the database level.
- All queries are filtered by tenant context derived from the authenticated JWT, not from user-supplied parameters.
- The Root/Owner of a tenant has implicit full access within that tenant. No permission evaluation is required for this role - it short-circuits all checks.

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

Permissions are partitioned by `AccessScope` to prevent cross-context pollution. An admin managing billing settings should not encounter project-level permission prompts, and a project editor should not need to care that billing exists.

- **`ORGANIZATION` Scope:** Governs organization-level resources - projects, invitations, tasks, members, milestones.
- **`PROJECT` Scope:** Governs resources within a specific project container - tasks, milestones and expenses specific to a single project.

The current model's highest explicit scope is `ORGANIZATION`. Tenant-level access is handled implicitly via the Root User relationship established during signup. This is intentional for the early stage - there is no need to manage tenant-level permissions when there is exactly one possible authority.

The architecture is designed so that a `TENANT` scope can be introduced in the future to support multi-admin tenant management or supervised "Support" tooling without restructuring the existing evaluation engine.

### 3.3 Permission Evaluation Flow

When an API endpoint requests an authorization check, the engine evaluates the user's rights through a layered hierarchy:

1. **Tenant Override:** If the user is the Root/Owner of the Tenant, access is granted immediately. Short-circuit, no further evaluation needed.
2. **Organization Level:** The engine fetches the user's `Member` record for the current organization and evaluates the policies attached to their `ORGANIZATION` scoped role.
3. **Project Level:** If the action relates to a specific project, the engine fetches the `ProjectMember` record for that project ID and evaluates the `PROJECT` scoped role.
4. **Union Result:** The final permission set is the union of the Org-level and Project-level grants.

> **Note on redundancy:** If an Organization Role grants `project:read` globally, the user automatically has access to all projects without a `ProjectMember` record. Conversely, if the Organization Role is restricted (e.g., a "Guest" role), the user only gains access to projects where an explicit `ProjectMember` record has been created for them. The system supports both broad grants and surgical exceptions using the same model.

### 3.4 Performance: Redis-Flattened Permission Matrix

Evaluating a full role → policy → statement chain on every API request would be expensive. The system uses a flattening and caching strategy:

- **Flattening:** On login or organization switch, the entire role hierarchy is resolved and collapsed into a flat Permission Matrix - a `Set<string>` of action strings.
- **Redis Caching:** This matrix is stored in Redis, transforming what would be a multi-join relational query into an $O(1)$ key lookup on every subsequent request.
- **Automatic Invalidation:** The cache is invalidated and rebuilt only when the user's role or any underlying policies are updated. Quiet by default, reactive when it matters.

**Trade-offs:**

- Authorization checks add negligible latency to hot paths.
- A short window exists between a permission change and cache invalidation where the old matrix may still be in use. For most permission updates this is acceptable; for sensitive demotions (e.g., removing an Admin), explicit invalidation should be triggered immediately.

### 3.5 Frontend Integration

The flattened Permission Matrix is passed to the Next.js frontend and stored in a dedicated React Context hook.

- **Conditional Rendering:** UI components read from this hook to show or hide elements (e.g., "Delete" buttons, admin panels) without a network round-trip.
- **Server-Side Rendering (SSR):** The same matrix is available during the SSR pass, ensuring restricted content is never included in the initial HTML payload sent to the client.

---

## 4. Default System Roles

A set of predefined roles covers the common configuration cases out of the box, removing the need for every new organization to start from scratch. Custom roles can be composed for teams with more specific requirements.

| Role Name       | Scope          | Intended Use                                                                 |
|-----------------|----------------|------------------------------------------------------------------------------|
| Owner           | `ORGANIZATION` | Full control over the tenant, billing, and all sub-organizations.            |
| Administrator   | `ORGANIZATION` | Management of members, teams, and projects across the organization.          |
| Standard Member | `ORGANIZATION` | Default employee access; can create projects and view public resources.      |
| Project Lead    | `PROJECT`      | Full administrative control over a specific project and its milestones.      |
| Editor          | `PROJECT`      | Ability to create and modify tasks and content within a specific project.    |
| Viewer          | `PROJECT`      | Read-only access for stakeholders or external observers.                     |

---

## 5. Security Considerations & Known Trade-offs

### Action Naming Conventions

Adding entirely new feature areas to the platform - "Analytics," "Budgeting," "Approvals" - requires no schema changes. New action strings are registered and attached to policies. The authorization logic stays the same.

**Risk:** The system requires disciplined naming conventions for actions (e.g., `service:resource:action`). Without an enforced Action Registry, the database will accumulate inconsistent strings over time, making auditing difficult and permission grants ambiguous.

### Sticky Access After Demotion

When a user is demoted from Administrator to a restricted role, their implicit organization-wide access is immediately governed by the new role. However, any explicit `ProjectMember` records created prior to the demotion remain valid until manually removed.

This means a demoted admin may retain direct access to specific private projects via explicit membership records. The current approach deletes all `ProjectMember` records on demotion, since there is no reliable way to distinguish which project memberships were intentional vs. incidental to the admin role. This trades a potential access overhang for a potential access gap (projects the user should still see need to be manually re-added).

**Future improvement:** Allow the authority changing the role to make an explicit decision per `ProjectMember` record at demotion time.

### Infrastructure Security

Infrastructure-level controls - IAM least privilege, VPC subnet isolation, Security Groups, NACLs and secrets management via AWS Secrets Manager - are documented in [IaC Strategy](iac-strategy.md).
