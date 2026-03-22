# Mirama System Core Requirements 

This document defines the functional and non-functional requirements for the Mirama Platform. It serves as the architectural "North Star," ensuring that as the platform evolves, the core mission remains the priority, delivering a visual-first, high-performance production environment for creative teams. Our guiding principle is practicality over complexity, ensuring that every technical decision, whether in an initial unified build or a future distributed network, directly supports the creative lifecycle.

---

The Mirama Platform is engineered to centralize the creative process, transforming project management into a visual production hub. The following core functionalities define the system’s capabilities:

## 1. Functional Requirements 

### 1.1 Visual-First Project, Task & Asset Lifecycle
Unlike traditional agile tools where images are secondary attachments, Mirama treats Mockups, Figures and Illustrations as primary data entities.

* **FR-1.1: Recursive Asset Trees:** The system should support N-level deep task nesting to mirror complex design folder structures (e.g., Project > Branding > Logo > Social Mockups).
* **FR-1.2: Complex Task Modeling:** In extension to the above, the system should support diverse task types, including Epics, Features, Stories, Tasks and Issues with recursive sub-tasking.
* **FR-1.3: Asset-Centric Tasks:** Every task entity must act as a container for "Figures" (Images/Mockups), treating them as primary data rather than generic attachments.
* **FR-1.4: Version Tracking:** The system shall allow users to upload new iterations of a mockup to the same task, maintaining a chronological "Evolution History" that is easily toggleable.
* **FR-1.5: Progress Aggregation:** The system shall automatically calculate the completion percentage of a parent project based on the status of all recursive child tasks.
* **FR-1.6: Collaboration & Favorites:** Integrated threaded commenting between team members, clients and managers. Which includes reply support and an "Activity Feed" for transparency. Users can Favourite specific Projects, Tasks/Asset Versions or Routes for rapid navigation

### 1.2 Multi-Tenant Identity & Workspace Hierarchy
The system manages complex relational boundaries between Tenants, Organizations, Teams and Clients.
* **FR-2.1: Tenant Isolation:** The system must strictly filter all data, mockups and user metadata by the users currently active 'OrganizationId' and the currently assumed 'TenantId'.
* **FR-2.2: Context Switching:** Users with access to multiple organizations must be able to switch their active organization and tenant context without a full reauthentication.
* **FR-2.3: Team-Based Workspaces:** Organizations must be able to partition projects into "Teams" with granular access control (e.g., the "Illustration Team" cannot access "Finance Team" projects).
* **FR-2.4: Delegated Invitation & Access:** Admins shall be able to invite internal members or external freelancers to an Organization via secure, tokenized email invitations. Upon acceptance, Admins can delegate access to specific Projects or Teams without granting full Organization-level visibility.

### 1.3 Review & Approval Workflow
* **FR-3.1: Visual Review Mode:** The system shall provide a dedicated view for "Stakeholders" to inspect high-res mockups and leave status updates (Approved/Revision Needed).
* **FR-3.2: Asset Direct-Link:** The system must generate secure, time-limited URLs for viewing high-res assets stored in Cloud storage to prevent unauthorized external access.
* **FR-3.3: Annotated Notifications:** System-managed notifications (Read/Unread) linked directly to specific visual resources or comment threads.

### 1.4 Platform Intelligence, Discovery & Integration
This section defines how Mirama ensures data is discoverable, collaborative and extensible within a professional tech stack.

* **FR:4.1 Fast, Indexed Search:** The system shall provide high-performance, server-side based "type-ahead" discovery across Projects, Tasks and Assets Versions.
* **FR:4.2 Hierarchical Filtering & Shortcuts:** Users shall have the ability to filter views by complex criteria (e.g., "High-Priority Mockups assigned to Designer X") and "Favourite" specific entities or routes for rapid sidebar access.
* **FR:4.4 Google Ecosystem Integration:** The system shall support authentication via Google IdP and provide synchronization with Google Calendar for automated project timeline and milestone tracking.

* **FR:4.5 Self-Service Admin:** Administrators shall be able to manage the organization lifecycle, including team-role assignments and secure, token-based user invitations.

### 1.5 Creative Interface & Adaptive Visualization
This section defines how Mirama translates complex hierarchical data into intuitive, visual-first work environments for designers, managers, and stakeholders.

* **FR: 5.1 Multi-Surface Task Navigation:** The system shall provide multiple synchronized views for the same task data, including Drag-and-Drop Kanban Boards for flow, Hierarchical Lists and Tables for deep organization and Gantt Charts for timeline-based roadmap visualization.
* **FR: 5.2 Contextual Mockup Visualization:** Users shall be able to visualize design assets across different scopes:
    - Single Task View: Focused view of a task and its direct sub-tasks.
    - Container/Epic View: An aggregated view of all visual assets nested under a high-level container (e.g., viewing all mockups for an entire Marketing Campaign story).
* **FR: 5.3 Evolution & Approval Dashboard:** The UI shall provide a high-level "Approval Matrix" to instantly identify which tasks have "Approved" figures, which are in "Revision" and which version of a specific asset is currently marked as the Active/Production iteration.
* **FR: 5.4 Visual Asset Boards:** A dedicated board view shall exist to visualize "Figures" and "Mockups" as the primary cards (rather than text-heavy cards), allowing creative teams to scan project progress visually.
* **FR: 5.5 Seamless View State Management:** The system shall persist user view preferences (e.g., "Always open Project X in Gantt view") to ensure a personalized and efficient workflow across different roles.

### 1.6 Asset & Workflow Orchestration
* **FR: 6.1 Milestone & Roadmap Management:** Users shall be able to set critical roadmap dates and visualize progress toward major delivery goals through a high-level milestone tracking system.
* **FR:6.2 Secure Asset Handling:** The system shall handle secure storage and retrieval of creative figures and documents via AWS S3, ensuring access controls strictly respect the Tenant/Organization boundary.
* **FR:6.3 (Phase 2) Event-Driven Consistency:** The system shall utilize asynchronous messaging (SNS/SQS) to propagate state changes across service boundaries—for example, a "Task Approved" event in the Project Service triggering a "Billing Update" in the Expense Service.
* **FR:6.4 (Phase 2) BFF Data Aggregation:** The Backend-for-Frontend (BFF) shall orchestrate complex data retrieval from downstream services (Account, Project, Asset) to provide unified payloads, such as combining a recursive task tree with its associated financial budget.

---

## 2. Non-Functional Requirements (Quality Attributes)

These attributes define how the system should perform and evolve. We prioritize these to ensure the system is maintainable by a single developer while remaining "production-ready."


### 2.1 Performance & Edge Optimization
Mirama is built for an "instant" feel, essential for creative professionals who cannot wait for heavy assets to load.
* **NFR-1.1: Response Latency:** All API routes must aim for a response time of < 1 second. High-traffic routes (Task updates, Navigation) should target < 300ms.
* **NFR-1.2: Asset Delivery Optimization:** To prevent bottlenecking the API, high-res mockups and figures shall not flow through the application server. Instead, the system generates Signed S3 URLs allowing the client to fetch assets directly from the edge.
* **NFR-1.3: Optimistic UI & Local State:** The frontend shall implement Optimistic Updates (via TanStack Query). The UI reflects task changes or asset renames immediately, with a background synchronization and a robust "Failure Fallback" to revert state if the server rejects the change.
* **NFR-1.4: Rendering Strategy:** Make use of Next.js SSR and PPR (Partial Prerendering) to reduce Time-to-First-Byte.

### 2.2 Security: Stateless & Multi-Layered
Security is baked into the network architecture to protect sensitive client IP and design mockups.
* **NFR-2.1: Stateless JWTs:** Authentication utilizes stateless JSON Web Tokens. In Phase 2, OpenIddict will manage these tokens, including custom claims for tenantId and orgId to ensure services don't need to "call home" for every request.
* **NFR-2.2: Asset Encryption:** All creative assets (Mockups/Figures) are encrypted at rest within AWS S3 and served exclusively over TLS 1.2+.
* **NFR-2.3: Tenant Isolation (Middleware):**
    - Phase 1: Enforced via Prisma middleware/global filters in Next.js.
    - Phase 2: Enforced via C# Action Filters and EF Core Global Query Filters to prevent cross-tenant data leaks.

### 2.3 Reliability & Maintainability
To ensure the "Playground" doesn't become a "Graveyard," we enforce high engineering standards across both stacks.
* **NFR-3.1: Standards & Patterns:**
    - Phase 1 (Next.js): Layered architecture in API with small controllers for high testability of components to ensure speed of delivery and correctness. Furthermore, frontend should split components in a component based way across different portals e.g. clients/internal organization members. 
    - Phase 2 (C#): Strict Clean Architecture and Vertical Slices. This isolation ensures that changing the "Expense Service" logic cannot break the "Asset Service."
* **NFR-3.2: Automated CI/CD:**
    - Phase 1: Vercel-based deployments through GitHub actions CI/CD pipeline with basic integration tests.
    - Phase 2: Full GitHub Actions pipelines building Docker containers for environment parity across local, staging and production.
* **NFR-3.3: Testing Strategy:**
    - Unit Testing: Focus on complex logic like "Progress Aggregation" (FR-1.5).
    - Integration Testing: Ensuring the BFF correctly orchestrates data from multiple services (FR-6.4).

### 2.4 Availability & The CAP Theorem (Phase 2)
In distributed systems, the CAP Theorem states that a system can only provide two of three guarantees: Consistency, Availability, and Partition Tolerance. Mirama is designed as an $AP$ System, prioritizing responsiveness over immediate consistency.
* **NFR-4.1 Prioritizing Creative Flow:** It is functionally superior for a designer to move a task or upload a figure—even if auxiliary data takes a few seconds to sync, than to have the interface freeze due to a background service delay.
* **NFR-4.2 Embracing Eventual Consistency:** Instead of forcing every component to agree before confirming an action, the system commits changes locally and broadcasts updates in the background. While data might diverge for a few seconds, it will always converge to a single "Truth" once processing completes.
