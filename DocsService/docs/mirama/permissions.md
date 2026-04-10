# Permissions & Authorization System

The [Multi-Tenancy Architecture](multi-tenancy.md) document establishes the outer boundary of our security system which is strict isolation between organizations and tenants so that no tenant can ever see another tenant's data. This document covers the layer inside that boundary, i.e. what users within an organization are *allowed to do* with the data they can see.

Mirama uses a **Policy-Based Access Control (PBAC)** model. By decoupling permissions from hardcoded role names, the system avoids the classic "role explosion" problem and instead scales gracefully as the platform grows from a project management tool into a broader organizational platform.

---

## 1. Core Architectural Concepts

### The Three-Tier Abstraction

The system does not assign individual permissions directly to users. Instead, it builds up access through three levels of composition:

- **Policy Statement:** The atomic unit of permission. Defines an `Action` (e.g., `project:edit`), a `Resource` (e.g., `*` or a specific ID), and an `Effect` (`ALLOW` or `DENY`).
- **Policy:** A named grouping of statements representing a functional capability (e.g., "Project Management Policy").
- **Role:** A container for one or more policies. Assigning a Role to a user grants them the union of all underlying policy permissions.

This separation means that when a new feature is added to the platform, it requires only a new action string and a policy update, no new hardcoded roles and no schema changes.

### Access Scoping

Permissions are further partitioned by `AccessScope` to prevent cross-context pollution. An admin managing billing settings should not encounter project-level permission prompts and a project editor should not need to care that billing exists.

- **`ORGANIZATION` Scope:** Governs organization-level resources, projects, invitations, tasks, members, milestones.
- **`PROJECT` Scope:** Governs resources within a specific project container — tasks, milestones and expenses specific to only a single project.

The current model's highest explicit scope is `ORGANIZATION`. Tenant-level access is currently handled implicitly via the Root User relationship established during signup. This is intentional for the early stage, there is no need to manage tenant-level permissions when there is exactly one possible authority.

The architecture is designed so that a `TENANT` scope can be introduced in the future to support multi-admin tenant management or "Support" tooling that requires supervised access above the organization level without restructuring the existing evaluation engine.

---

## 2. Permission Hierarchy and Evaluation

When an API endpoint or UI component requests an authorization check, the engine evaluates the user's rights through a layered hierarchy rather than a single flat lookup.

### The Evaluation Flow

1. **Tenant Override:** If the user is the Root/Owner of the Tenant, access is granted immediately. Short-circuit, no further evaluation needed.
2. **Organization Level:** The engine fetches the user's `Member` record for the current organization and evaluates the policies attached to their `ORGANIZATION` scoped role.
3. **Project Level:** If the action relates to a specific project, the engine fetches the `ProjectMember` record for that project ID and evaluates the `PROJECT` scoped role.
4. **Union Result:** The final permission set is the union of the Org-level and Project-level grants.

> **Note on redundancy:** If an Organization Role grants `project:read` globally, the user automatically has access to all projects without a `ProjectMember` record. Conversely, if the Organization Role is restricted (e.g., a "Guest" role), the user only gains access to projects where an explicit `ProjectMember` record has been created for them. The system supports both broad grants and surgical exceptions using the same model.

---

## 3. Technical Implementation

### Backend Performance (Redis)

Evaluating a full role, policy, statement chain on every API request would be expensive. The system sidesteps this with a flattening and caching strategy:

- **Flattening:** On login or organization switch, the entire role hierarchy is resolved and collapsed into a flat Permission Matrix — a `Set<string>` of action strings.
- **Redis Caching:** This matrix is stored in Redis, transforming what would be a multi-join relational query into an $O(1)$ key lookup on every subsequent request.
- **Automatic Invalidation:** The cache is invalidated and rebuilt only when the user's role or any underlying policies are updated. Quiet by default, reactive when it matters.

**Pros:**

- Authorization checks add negligible latency to hot paths.
- Flattening is a clean separation between the "configuration" model (roles/policies) and the "evaluation" model (the matrix).

**Cons:**

- A short window exists between a permission change and cache invalidation where the old matrix may still be in use. For most permission updates this is acceptable; for sensitive demotions (e.g., removing an Admin), explicit invalidation should be triggered immediately.

### Frontend Integration (React Context)

The flattened Permission Matrix is passed to the Next.js frontend and stored in a dedicated React Context hook.

- **Conditional Rendering:** UI components read from this hook to show or hide elements (e.g., "Delete" buttons, admin panels) without a network round-trip.
- **Server-Side Rendering (SSR):** The same matrix is available during the SSR pass, ensuring that restricted content is never included in the initial HTML payload sent to the client.

---

## 4. Default System Roles

A set of predefined roles covers the common configuration cases out of the box, removing the need for every new organization to start from scratch. Custom roles can be composed for teams with more specific requirements.

| Role Name | Scope | Intended Use |
|---|---|---|
| Owner | `ORGANIZATION` | Full control over the tenant, billing, and all sub-organizations. |
| Administrator | `ORGANIZATION` | Management of members, teams, and projects across the organization. |
| Standard Member | `ORGANIZATION` | Default employee access; can create projects and view public resources. |
| Project Lead | `PROJECT` | Full administrative control over a specific project and its milestones. |
| Editor | `PROJECT` | Ability to create and modify tasks and content within a specific project. |
| Viewer | `PROJECT` | Read-only access for stakeholders or external observers. |

---

## 5. Observations and Future Scalability

### Action Naming Conventions

**Pros:** Adding entirely new feature areas to the platform, "Analytics," "Budgeting," "Approvals", requires no schema changes. New action strings are registered and attached to policies. The authorization logic stays the same.

**Cons:** The system requires disciplined naming conventions for actions (e.g., `service:resource:action`). Without an enforced Action Registry, the database will accumulate inconsistent strings over time, making auditing difficult and permission grants ambiguous.

### Security Note: Sticky Access After Demotion

When a user is demoted from Administrator to a restricted role, their **implicit** organization-wide access is immediately governed by the new role. However, any **explicit** `ProjectMember` records created prior to the demotion remain valid until manually removed.

This means a demoted admin may retain direct access to specific private projects via explicit membership records. We take a revoked-access approach to this issue, and delete delete all membership records from the user, since we cannot otherwise tell which Projects the previous Admin is supposed to be able to see. This brings up the issue of potentially having to readd a user to projects he should still be able to see.

In the future this could be solves by allowing the authority changing the admins role to select what exactly should happen to specific Membership records.
