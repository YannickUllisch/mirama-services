# Mirama System Core Requirements

This document defines the functional and non-functional requirements for the Mirama Platform. It serves as the architectural "North Star" ensuring that as the platform evolves, the core mission remains the priority: delivering a unified Creative Operating System that handles the specific complexity of creative work while maintaining the data integrity needed for client accountability, resource management and - eventually - financial reporting.

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

* **FR-1.17 Task Dependencies:** Define dependencies between tasks with automatic downstream adjustment.
  If a "Creative Brief" task is delayed, all dependent downstream tasks (Copywriting, Design, Development) must shift their scheduled dates automatically. This is essential for campaign planning and prevents silent deadline misalignment.

* **FR-1.18 Custom Task Statuses:** Organizations can define and configure their own workflow statuses.
  Generic "In Progress" is insufficient for creative pipelines. Teams need statuses such as "Internal Review," "Awaiting Client Assets," "Client Revisions," and "Final Approval" to accurately reflect real production stages and reduce miscommunication.

---

### 1.2 Large Asset Management & Storage

* **FR-1.21 Large Asset Upload Support:** Support uploads up to 1GB (for now).
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
  Users should see visual feedback immediately while larger originals load in the background on request. This is important to not overwhelm the browser but still allow stakeholders to see the intended version when in approval flow.

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

### 1.6 Review, Approval & Native Proofing

* **FR-1.61 Visual Review Mode:** Dedicated stakeholder view for mockup inspection and approvals.  
  The system needs to provide a dedicated stakeholder view for inspecting high-resolution mockups and leaving approval states. This gives clients and managers a simplified interface focused on reviewing visuals rather than navigating project structure.

* **FR-1.62 Asset Direct-Link:** Secure, time-limited URLs for asset viewing.  
  The system must generate secure, time-limited URLs for viewing high-resolution assets. These links allow external reviewers to access visuals without exposing the entire workspace or storage bucket.

* **FR-1.63 Annotated Notifications:** Notifications linked to assets or comment threads.  
  System-managed notifications are linked directly to visual resources or comment threads. Notifications should bring users back to the exact asset version or discussion, reducing friction in feedback loops.

* **FR-1.64 Native Click-on-Spot Annotation:** Pin comments to specific coordinates on images, PDFs and video.  
  Reviewers must be able to click any point on a visual asset and leave a contextual comment at that exact location. This eliminates the need for external proofing tools (Frame.io, Markup.io) and keeps feedback directly inside the task. Annotations should be visible as pins on the asset surface, threaded, and resolvable.

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

* **FR-1.86 Calendar View:** Date-based task and milestone visualization.  
  A calendar view surfaces tasks, deadlines and milestones by date. This is essential for content-heavy teams (social media, editorial) who plan work in weekly or monthly cycles rather than by sprint.

---

### 1.9 Milestone & Project Timeline Tracking

* **FR-1.91 Milestone Tracking:** Define milestones at project level.  
  Users shall be able to define milestones at the project level. Milestones represent key delivery checkpoints such as "Client Review", "Final Export" or "Campaign Launch".

* **FR-1.92 Milestone Progress Aggregation:** Milestone progress reflects associated tasks/assets.  
  Milestones automatically reflect progress based on associated tasks and assets. This provides a high-level overview of delivery readiness without manual tracking.

* **FR-1.93 Timeline Visualization:** Visualize milestones in Gantt and roadmap views.  
  Milestones shall be visualized within Gantt and roadmap views. This helps teams understand dependencies and plan delivery schedules visually.

* **FR-1.94 Milestone Notifications:** Notify users when milestones approach or change state.  
  Users shall receive notifications when milestones approach or change state. This ensures deadlines remain visible and reduces missed delivery dates.

---

### 1.10 Time Tracking & Resource Management

This section covers the financial and operational foundation that separates a professional creative platform from a generic task tracker. Without time tracking and workload visibility, the platform cannot support billing, profitability analysis or capacity planning - making a future ERP layer impossible.

* **FR-1.101 Native Task Timer:** A built-in start/stop timer on every task.  
  Every task must have a native time tracking control. Designers, developers and copywriters log time directly on the work object without switching to a separate time tracker. Logged time entries capture duration, user and optionally a note. This data is the source of truth for future billing and profitability reports.

* **FR-1.102 Billable vs. Non-Billable Classification:** Time entries are classified as billable or non-billable.  
  Each time entry must be explicitly classified. Billable time flows into client invoicing; non-billable time represents internal overhead, rework or unbilled scope. Without this distinction, a project can appear profitable on paper while the actual margin is negative.

* **FR-1.103 Budget Tracking & Burn Alerts:** Projects have a defined budget (hours or currency) with threshold alerts.  
  Project managers must be able to set a budget against a project. The system shall calculate burn rate from logged time entries and notify relevant members when the project reaches 50%, 75% and 100% of the allocated budget. This prevents silent over-servicing - a common cause of margin loss in agencies.

* **FR-1.104 Capacity & Workload View:** A manager-level view showing team member utilization across all active projects.  
  The system must provide a cross-project workload dashboard showing each team member's scheduled or logged hours in a given period. Overbooked members are surfaced visually (red), available capacity is visible (green). This allows project managers to redistribute work before deadlines are missed rather than after.

---

### 1.11 Client & Intake Management (CRM Foundation)

This section establishes the relational client model and intake pipeline that connects the CRM layer to project execution. The `Client` entity is a first-class object in the data model - linked to intake briefs, projects, time records and (in future) invoices. If the client is a tag or a folder name rather than a relational object, cross-client reporting and billing break.

* **FR-1.111 Custom Intake & Request Forms:** Configurable external forms for client brief submission.  
  Organizations must be able to create branded, configurable intake forms that external clients fill out to submit a brief or request. A submitted form automatically creates a Lead record (CRM) or a Project (PM) depending on configuration. This gives freelancers and agencies a professional intake pipeline without a separate CRM tool.

* **FR-1.112 Client Portal & Guest Access:** A simplified, scoped view for external clients.  
  External clients must be able to access a dedicated portal that shows project progress, active assets for review, and approved deliverables - without seeing internal budget data, team discussions, or organizational structure. The portal requires no account creation if accessed via a secure invitation link.

* **FR-1.113 Public/Private Privacy Toggles on Tasks and Comments:** Granular control over what clients see in the portal.  
  Project managers must be able to mark individual tasks or comment threads as internal-only. Internal content is hidden from the client portal regardless of project-level sharing settings. This allows teams to have honest internal discussions about revisions or issues without exposing them to the client.

* **FR-1.114 Lead-to-Project Conversion:** A brief submitted via intake form links to a structured project.  
  When a Lead is converted to a Project, the client relationship record, brief data and any attachments carry over automatically. The `Client` object remains the parent entity linking the intake brief, the project, logged time and future invoices - maintaining a continuous audit trail of the client engagement.

---

### 1.12 Automation, Templates & Search

* **FR-1.121 Project Templates:** One-click project creation from a saved template.  
  Organizations must be able to save any project structure as a reusable template - including pre-set tasks, subtasks, roles, custom statuses and estimated durations. Common service packages (e.g., "Brand Identity," "Campaign Package," "Website Build") become one-click setups rather than manual recreation. This is the primary scaling mechanism for small agencies delivering repeatable services.

* **FR-1.122 Automated Workflow Triggers:** Event-driven rule engine for status changes and notifications.  
  The system shall support configurable automations of the form: "When [condition], then [action]." Examples: when a task status changes to "Approved," automatically send a notification to the client and move the task to "Final Delivery." When a milestone deadline is 48 hours away with no activity, notify the project lead. Automations reduce manual overhead and enforce consistent workflows across teams.

* **FR-1.123 Global Search:** A unified search engine across tasks, files and conversations.  
  Users must be able to search across task names, file names, comment text and client records in a single query from anywhere in the application. Search results are scoped to the user's active organization and respect all permission boundaries. This is critical for large projects where content accumulates quickly and navigation becomes insufficient.

---

### 1.14 AI Platform Intelligence

This section defines requirements for the AI capabilities embedded across the Mirama engagement lifecycle. Each capability targets a specific workflow pain point rather than adding a generic "AI assistant" layer. The goal is augmentation of existing workflows, not replacement of user judgment.

* **FR-1.141 AI Brief Intelligence:** When a client submits an intake form, the system shall use an LLM to parse the brief text and extract key structured data - deliverable types, desired timelines, budget signals, and scope indicators. Extracted data shall pre-populate project creation fields and suggest the most relevant project template from the organization's library. This removes manual interpretation overhead from the intake-to-project conversion step and connects directly to FR-1.114 (Lead-to-Project Conversion).

* **FR-1.142 Vision-Based Asset Tagging:** On upload, image and PDF assets shall be processed by a multimodal AI model to generate descriptive tags covering visual content, dominant colors, style characteristics and detected objects or subjects. Tags are indexed alongside file metadata and surfaced within global search (FR-1.123), enabling visual assets to be found by content rather than file name alone.

* **FR-1.143 Annotation Feedback Summarization:** After a client or reviewer leaves multiple point annotations across one or more asset versions (FR-1.64), the system shall offer an AI-generated summary consolidating all comments into a structured list of actionable revision notes grouped by theme. The summary is attached to the task and visible to the production team, reducing the time spent interpreting scattered client feedback.

* **FR-1.144 Predictive Risk Detection:** The system shall continuously analyze active projects for signals of budget overrun and deadline risk - including burn rate velocity, unresolved task dependencies, blocked tasks, and low logged-hour ratios relative to estimated work. When a risk threshold is crossed, a prioritized alert is surfaced to the project lead with the specific contributing factors. This extends the budget burn alerts in FR-1.103 with predictive rather than threshold-only logic.

* **FR-1.145 Conversational Project Copilot:** Users shall be able to query the platform in natural language from a persistent command interface. Queries may retrieve information ("show me all tasks awaiting client approval on Project X"), trigger actions ("create a task for logo revisions under the Brand Identity milestone"), or request summaries ("what is the current budget burn across all active projects?"). Responses are scoped to the user's organization and respect all PBAC permission boundaries.

* **FR-1.146 Smart Automation Suggestions:** The system shall observe recurring workflow patterns across teams - such as consistently moving tasks to a specific status after a client comment, or always creating the same task type at project kickoff - and surface suggested automation rules (FR-1.122) based on observed behavior. Suggestions are presented as a proposed rule with a one-click acceptance flow, reducing the expertise required to configure workflow automation.

---

### 1.13 Platform Extensibility

* **FR-1.131 Open API:** A versioned, publicly documented REST API.  
  All core entities (clients, projects, tasks, time entries, assets) must be accessible via a documented REST API. This allows users to integrate Mirama with tools not natively supported - custom scripts, internal dashboards, or industry-specific software - without relying on Mirama to build every integration.

* **FR-1.132 Webhooks:** Outbound event notifications for key platform events.  
  The system must support configurable webhooks that fire on defined events (task status change, new comment, asset uploaded, milestone reached). This enables real-time integration with Slack, email services, Adobe Creative Cloud and custom workflow tooling without polling the API.

* **FR-1.133 Audit Logs:** An immutable record of all significant platform actions.  
  The system must record who performed what action, on what resource, and when - for all destructive or permission-sensitive operations (deadline changes, file deletions, role modifications, billing record edits). Audit logs are read-only, scoped to the organization and accessible to Administrators. This is a non-negotiable requirement for large firm accountability and compliance.

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

* **NFR-2.44 Access Enforcement (PBAC):** Delegated permissions, policy-based access and external review links must respect scoped access at all times.

* **NFR-2.45 Stateless JWT & OIDC:** Authentication tokens must be stateless JWTs compliant with OpenID Connect (OIDC) standards, including claims for tenantId, orgId and roles. Services can validate tokens independently without additional database lookups, reducing attack surface and improving response speed.

---

### 2.5 Availability & Reliability

* **NFR-2.51 System Availability:** Platform target uptime 99.5% (initial), 99.9% (production scale).

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

* **NFR-2.71 Layered Architecture (Next.js Frontend):** Enforce clear frontend code separation.  
  Frontend code must follow clear layered separation (components, hooks, services, utilities) to improve readability, testability and feature evolution.

* **NFR-2.72 Clean Architecture & Vertical Slices (MiramaService Backend):** Use Clean Architecture for the modular monolith.  
  The MiramaService backend must use Clean Architecture principles with vertical slice patterns per feature, ensuring modules can evolve independently without breaking unrelated functionality.

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
    * **Next.js Frontend:** Use structured logging (e.g., Pino) to record request/response metadata, errors and key application events. Tracing should propagate `traceId`s across async flows (SNS, SQS file handling) so that errors can be directly related back to a specific request.  
    * **MiramaService Backend:** Extend logging with structured tracing (e.g., Serilog + OpenTelemetry) to allow correlation of requests across module boundaries, supporting debugging, performance analysis, and visualization in monitoring tools.

---

### 2.8 Storage Organization

Assets must follow deterministic hierarchical paths:

```bash
tenant/{tenantId}/org/{orgId}/project/{projectId}/task/{taskId}/asset/{assetId}/version/{versionId}
```

This structure ensures strict tenant isolation, predictable retrieval patterns and scalability as the asset library grows.
