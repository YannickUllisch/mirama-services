# Mirama System Core Requirements

This document defines the functional and non-functional requirements for the Mirama Platform. It serves as the architectural "North Star" ensuring that as the platform evolves, the core mission remains the priority, delivering a visual-first, high-performance production environment for creative teams.

---

## 1. Functional Requirements

### 1.1 Visual-First Project, Task & Asset Lifecycle

* **FR-1.11 Recursive Asset Trees:** Support N-level deep task nesting.
  This allows creative teams to structure work exactly like their design folder hierarchies. A branding project can contain logo work, which can contain variations, which can contain social exports, without forcing users into rigid depth limits.

* **FR-1.12 Complex Task Modeling:** Support Epics, Features, Stories, Tasks, Issues.
  Mirama must accommodate both structured planning and creative workflows. Some teams work with epics and stories, others work with design deliverables. The system needs to support both without friction.

* **FR-1.13 Asset-Centric Tasks:** Tasks act as containers for Figures.
  Instead of assets being secondary attachments, the visual output becomes the core object. Users interact with mockups, videos and images first, with metadata and discussion surrounding them.

* **FR-1.14 Version Tracking:** Upload iterations to same task.
  Designers frequently iterate. This ensures revisions remain grouped together, allowing stakeholders to follow the evolution of a concept from first draft to final production version.

* **FR-1.15 Progress Aggregation:** Parent progress auto-calculated.
  Project leads should instantly understand progress without manually updating rollups. Nested tasks automatically determine the completion of higher-level containers.

* **FR-1.16 Collaboration & Favorites:** Comments and activity feed.
  Communication stays directly connected to the work itself. Stakeholders comment on visuals, reply in threads and favorite important deliverables for quick access.

---

### 1.2 Large Asset Management & Storage

* **FR-1.21 Large Asset Upload Support:** Support uploads up to 1GB (fow now).
  Creative workflows often include large PSD files, 4K video exports, After Effects renders or RAW photography. The system must comfortably handle these without forcing users to rely on external storage tools.

* **FR-1.22 Direct-to-Storage Uploads:** Upload directly to S3.
  Large file transfers should never pass through the application server. Instead, the client uploads directly to storage using secure signed URLs, ensuring stability and scalability.

* **FR-1.23 Chunked Multipart Upload:** Files split into chunks.
  Uploading in smaller chunks avoids timeouts and improves reliability, especially on slower or unstable internet connections.

* **FR-1.24 Resume Interrupted Uploads:** Support resumable uploads.
  If a connection drops mid-upload, users should be able to continue from where they left off rather than restarting a 900MB upload.

* **FR-1.25 Original Format Preservation:** Store original file.
  Mirama should act as a centralized asset repository, meaning the exact original file must always remain available for download.

* **FR-1.26 Upload Progress Feedback:** Real-time progress UI.
  Users need clear feedback when uploading large files, including percentage progress and retry indicators.

---

### 1.3 Asset Versioning & Fast Retrieval

* **FR-1.31 Fast Asset History Loading:** Optimized version retrieval.
  Even assets with dozens of revisions should load instantly so users can navigate between versions without delays.

* **FR-1.32 Active Version Prioritization:** Fetch active version first.
  Boards and dashboards should render quickly by loading only the currently active version before loading the full history.

* **FR-1.33 Lazy Loading History:** Load versions incrementally.
  Historical versions load on demand to prevent performance degradation.

* **FR-1.34 Version Metadata Indexing:** Indexed metadata.
  Version information such as uploader, status and timestamps should be searchable and filterable.

* **FR-1.35 Evolution Toggle:** Switch versions instantly.
  Designers and stakeholders can quickly compare revisions and approvals.

---

### 1.4 Background Asset Processing

* **FR-1.41 Background Compression:** Generate compressed previews.
  Large assets automatically generate smaller preview versions for fast rendering.

* **FR-1.42 Multi-Resolution Assets:** Create thumbnails.
  Different UI surfaces require different resolutions, thumbnails, previews and high-resolution views.

* **FR-1.43 Preview-First Rendering:** Load compressed assets first.
  Users should see visual feedback immediately while larger originals load in the background on request. This is important to not overwhelm the browser
  but still allow stakeholders to see the intended version when in approval flow.

* **FR-1.44 Non-Blocking Processing:** Compression async.
  Upload completion should not wait for processing tasks.

---

### 1.5 Multi-Tenant Identity & Workspace Hierarchy

* **FR-1.51 Tenant Isolation:** Strictly filter all data by active organization and tenant.  
  The system must strictly filter all data, mockups and user metadata by the user's currently active `OrganizationId` and assumed `TenantId`. This ensures that no data from another organization is ever accidentally visible. Every request, query and asset lookup is always scoped to the active tenant context.

* **FR-1.52 Context Switching:** Switch organization and tenant context without reauthentication.  
  Users with access to multiple organizations must be able to switch their active organization and tenant context without reauthentication. Agencies often work across multiple clients. Switching context should feel instant and should not interrupt the workflow or require logging out.

* **FR-1.53 Team-Based Workspaces:** Partition projects into teams with granular access control.  
  Organizations must be able to partition projects into teams with granular access control. This allows departments such as Illustration, Marketing, and Finance to work in isolation while still belonging to the same organization.

* **FR-1.54 Delegated Invitation & Access:** Invite members and limit access by project or team.  
  Admins can invite internal members or external collaborators using secure tokenized invitations. Once accepted, access can be limited to specific projects or teams so for example freelancers only see the work relevant to them.

---

### 1.6 Review & Approval Workflow

* **FR-1.61 Visual Review Mode:** Dedicated stakeholder view for mockup inspection and approvals.  
  The system needs to provide a dedicated stakeholder view for inspecting high-resolution mockups and leaving approval states. This gives clients and managers a simplified interface focused on reviewing visuals rather than navigating project structure.

* **FR-1.62 Asset Direct-Link:** Secure, time-limited URLs for asset viewing.  
  The system must generate secure, time-limited URLs for viewing high-resolution assets. These links allow external reviewers to access visuals without exposing the entire workspace or storage bucket.

* **FR-1.63 Annotated Notifications:** Notifications linked to assets or comment threads.  
  System-managed notifications are linked directly to visual resources or comment threads. Notifications should bring users back to the exact asset version or discussion, reducing friction in feedback loops.

---

### 1.7 Platform Intelligence & Integration

* **FR-1.71 Favourite Entities & Routes:** Favorite projects, tasks, assets or views.  
  Users must be able to favorite projects, tasks, asset versions or specific views. This allows frequently accessed deliverables to appear instantly in the sidebar or on the application dashboard for faster navigation.

* **FR-1.72 Google Ecosystem Integration:** Google authentication and calendar sync.  
  The system shall support authentication via Google IdP and synchronization with Google Calendar. Project milestones and deadlines can automatically appear in calendars, aligning creative production with planning tools teams already use.

* **FR-1.73 Stateless Authentication with JWTs:** The system shall use stateless JSON Web Tokens (JWTs) for user authentication and session management.
This allows any service or API to validate a token without querying a central session store, improving performance, horizontal scalability, and reliability across distributed services.

---

### 1.8 Creative Interface & Adaptive Visualization

* **FR-1.81 Multi-Surface Task Navigation:** Synchronized Kanban, lists, tables and Gantt views.  
  The system shall provide synchronized views such as Kanban boards, hierarchical lists, tables and Gantt charts. Different roles prefer different visualizations. Designers may prefer boards, while managers rely on timelines and structured views.

* **FR-1.82 Contextual Mockup Visualization:** Visualize assets across different scopes.  
  Users shall visualize design assets across different scopes. A single task view focuses on immediate work, while container views aggregate all visuals across campaigns or epics.

* **FR-1.83 Evolution & Approval Dashboard:** Approval matrix for status and versions.  
  The UI shall provide an approval matrix for quickly identifying approval status and active versions. This helps teams understand what is production-ready and what still requires revision.

* **FR-1.84 Visual Asset Boards:** Board view with assets as primary cards.  
  A dedicated board view visualizes assets as primary cards. This allows creative teams to scan visual progress without reading heavy text-based task cards.

* **FR-1.85 Seamless View State Management:** Persist user view preferences.  
  The system shall persist user view preferences. Each user returns to their preferred visualization automatically, improving efficiency across sessions.

---

### 1.9 Milestone & Project Timeline Tracking

* **FR-1.91 Milestone Tracking:** Define milestones at project level.  
  Users shall be able to define milestones at the project level. Milestones represent key delivery checkpoints such as “Client Review”, “Final Export” or “Campaign Launch”.

* **FR-1.92 Milestone Progress Aggregation:** Milestone progress reflects associated tasks/assets.  
  Milestones automatically reflect progress based on associated tasks and assets. This provides a high-level overview of delivery readiness without manual tracking.

* **FR-1.93 Timeline Visualization:** Visualize milestones in Gantt and roadmap views.  
  Milestones shall be visualized within Gantt and roadmap views. This helps teams understand dependencies and plan delivery schedules visually.

* **FR-1.94 Milestone Notifications:** Notify users when milestones approach or change state.  
  Users shall receive notifications when milestones approach or change state. This ensures deadlines remain visible and reduces missed delivery dates.

---

## 2. Non-Functional Requirements

### 2.1 Performance & Edge Optimization

* **NFR-2.11 API Response Latency:** 95th percentile API responses must complete under **800ms**, with critical navigation endpoints targeting **<250ms**.

* **NFR-2.12 Asset Preview Load Time:** Compressed preview assets should render within **<500ms** from CDN edge locations.

* **NFR-2.13 Active Board Rendering:** Visual boards should display primary content within **<1.2 seconds** even for projects containing 100+ assets.

* **NFR-2.14 Time-To-Interactive:** Initial application load should reach interactive state within **<2.5 seconds** on standard broadband.

* **NFR-2.15 Token Validation Efficiency:** Stateless JWT validation must complete within <5ms per request, avoiding additional round-trips or database calls, even under high concurrency.

---

### 2.2 Large Asset Upload Reliability

* **NFR-2.21 Upload Stability:** The system must support sustained uploads of **1GB files** without server memory consumption exceeding **50MB per request**.

* **NFR-2.22 Multipart Chunk Size:** Uploads should use chunk sizes between **5MB–25MB** for optimal throughput.

* **NFR-2.23 Upload Timeout Avoidance:** No upload operation should rely on API routes exceeding **30 seconds** execution time.

* **NFR-2.24 Resume Reliability:** Upload recovery must succeed for **>95%** of interrupted uploads.

* **NFR-2.25 Upload Feedback:** Users should see progress and error recovery options in real time.

---

### 2.3 Asset Processing Performance

* **NFR-2.31 Preview Generation:** Images, videos and other asset previews should be generated efficiently (<8–25s for large assets).

* **NFR-2.32 Dashboard Responsiveness:** Evolution and approval dashboards, multi-view boards, and milestone views must remain responsive during active asset processing.

* **NFR-2.33 Background Queue Throughput:** Processing system should support **minimum 20 concurrent asset jobs**.

* **NFR-2.34 Compression Ratio Target:** Preview assets should reduce file size by **70–95%** compared to originals.

---

### 2.4 Security & Isolation

* **NFR-2.41 Signed URL Expiration:** Asset URLs expire within 5 minutes.

* **NFR-2.42 Tenant & Organization Isolation:** All asset, project, and task access must validate tenant and organization membership.

* **NFR-2.43 Encryption & TLS:** Assets encrypted at rest (AES-256) and all transfers over TLS 1.2+.

* **NFR-2.44 Access Enforcement (RBAC):** Delegated permissions, role-based access and external review links must respect scoped access at all times.

* **NFR-2.45 Stateless JWT & OIDC:** Authentication tokens must be stateless JWTs compliant with OpenID Connect (OIDC) standards, including claims for tenantId, orgId and roles. Services can validate tokens independently without additional database lookups, reducing attack surface and improving response speed.

---

### 2.5 Availability & Reliability

* **NFR-2.51 System Availability:** Platform target uptime 99.5% (Phase 1)*, 99.9% (Phase 2).

* **NFR-2.52 Background Job Retry:** Failed processing jobs retried up to **3 times**. After which they should be handled in a dead-letter-queue (DLQ).

* **NFR-2.53 Eventual Consistency Window:** Data convergence within **<5 seconds** for most operations.

* **NFR-2.54 CDN Cache Hit Ratio:** Preview assets and dashboards must make use of CDN caching with a hit ratio **>70%**, ensuring fast load even for large projects.

---

### 2.6 Scalability

* **NFR-2.61 Horizontal Elasticity:** Automatically scale horizontally during demand spikes.  
  In production the cloud infrastructure must be able to automatically scale horizontally during traffic or processing spikes (uploads, previews, dashboards) and scale down when demand is low. This ensures resources are only used when needed, keeping costs under control without impacting performance.

* **NFR-2.62 Stateless Services:** Core services remain stateless for flexible scaling.  
  Core services (API, asset processing, BFF) should remain stateless, allowing any instance to handle requests independently and be safely added or removed from the cluster.

* **NFR-2.63 Queue-Based Load Management:** Use asynchronous queues for burst handling.  
  Asset uploads, compression and preview generation should use asynchronous queues (SNS+SQS) to handle bursts in demand without overwhelming servers.

* **NFR-2.64 CDN & Edge Scaling:** Leverage CDNs for global low-latency access.  
  Asset previews, dashboards and frequently accessed resources must make use of CDNs with regional edge nodes to maintain low latency worldwide, even as the user base grows.

---

### 2.7 Maintainability

* **NFR-2.71 Layered Architecture (Next.js Backend):** Enforce clear backend code separation.  
  Backend code must follow clear layered separation (controllers, services, repositories, utilities) to improve readability, testability and feature evolution.

* **NFR-2.72 Clean Architecture & Vertical Slices (.NET Backend):** Use Clean Architecture for microservices.  
  Phase 2 microservices and backend components must use Clean Architecture principles with vertical slice patterns, ensuring independent services can evolve without breaking unrelated functionality.

* **NFR-2.73 Consistent Coding Standards:** Adhere to coding conventions and linting.  
  All codebases (frontend, backend, scripts) must adhere to defined coding conventions, naming rules and linting policies for readability and maintainability.

* **NFR-2.74 Modular & Componentized Frontend:** Build reusable and decoupled UI components.  
  UI components and pages must be reusable, composable, and decoupled, allowing teams to extend views, boards, and dashboards efficiently.

* **NFR-2.75 Automated Testing & Coverage:** Ensure high test coverage with automation.  
  Unit, integration and end-to-end tests must cover all critical workflows, with automated pipelines ensuring high test coverage before deployment.

* **NFR-2.76 Documentation & Onboarding:** Maintain up-to-date documentation and guides.  
  Maintain up-to-date technical documentation, architecture diagrams and setup guides to ensure new developers can onboard quickly.

* **NFR-2.77 Configurable Environment Management:** Replicate environments easily.  
  Local, staging, and production environments must be easy to replicate with configuration files, scripts, and containerization to reduce environment-specific bugs.

* **NFR-2.78 Structured Logging & Tracing:** Implement structured logging and distributed tracing.  
  All services must implement structured logging to capture meaningful events, errors, and context for each request.  
    * Phase 1 (Next.js): Use structured logging (e.g., Pino) to record request/response metadata, errors and key application events. Tracing should also be implemented to propagate and match `traceId`s across flows such as SNS, SQS file handling, so that errors can be directly related back to a specific request.
    * Phase 2 (.NET services): Extend logging with distributed tracing to allow correlation of requests across services, supporting debugging, performance analysis, and visualization in monitoring tools.

---

### 2.8 Storage Organization

Assets must follow deterministic hierarchical paths:

```bash
tenant/{tenantId}/org/{orgId}/project/{projectId}/task/{taskId}/asset/{assetId}/version/{versionId}
```

This structure ensures strict tenant isolation, predictable retrieval patterns and scalability as the asset library grows.
