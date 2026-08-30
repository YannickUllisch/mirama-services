# Mirama System Core Requirements

This document defines the functional and non-functional requirements for the Mirama Platform. It is the architectural "North Star" for a unified Delivery Operating System built for freelancers and micro-agencies who delegate client work, covering the engagement from the lead through any subcontractors brought in to help deliver it, while maintaining the data integrity needed for client accountability, subcontractor accountability, resource management and financial reporting. Every requirement below describes the same one system, nothing here is a separate later product bolted on afterward.

The MVP is the solo freelancer and the delegating lead or boutique studio of up to twenty people, the two tiers described in [Project Description](index.md) and validated in [Market Analysis](market-analysis.md). Requirements are written against that scope first. Sections that touch organization-wide analytics, department-level access control or capacity planning across dozens of people belong to the same modules and the same data model as everything else here, not a separate track, but they are not what gets built or tested against first. Section 0 states plainly what already exists today; the build order in [System Architecture](system-architecture.md) states what comes next.

---

## 0. Implementation Status

The requirements below describe the full target system. As of this writing, most of it is specified but not built. This section states plainly what exists in the MiramaService backend today, based on a direct read of the codebase rather than a summary of it. The Next.js frontend was not part of this review and is not covered here.

| Area | Status | Detail |
|---|---|---|
| Identity and multi-tenancy | Implemented | Organizations, invitations, members, teams, tags, role and policy based access control, and tenant subscription settings are live, with applied database migrations. This is the most complete part of the system. |
| Project and task execution | Implemented | Full CRUD on projects, project members, teams, milestones, custom statuses and priorities, and a complete project template system covering cycle, milestone and task templates. |
| Client and intake (CRM) | Started, not functional | The Client aggregate, contacts, and a token-based client portal invitation flow exist in code, but the module has no applied database migration yet. There is no intake form, no lead-to-project conversion, and no privacy toggle on tasks or comments. |
| Assets, versioning and proofing | Domain modeled only | The Asset aggregate is fully designed at the domain layer, including version numbering, an approval workflow (draft, in review, changes requested, approved, rejected, archived), threaded feedback with spatial, temporal and page-based annotation coordinates, and connections to other entities. None of it is exposed through an API endpoint, connected to storage, or backed by a database migration yet. |
| Time tracking, client billing and payouts | Not started | No timer, no billable classification, no client-facing invoicing, no Stripe integration. The "Billing" code that exists today lives in the Identity module and covers Mirama's own subscription plans and usage for tenants paying for the platform. It is unrelated to this requirement area and the naming collision is worth keeping in mind. |
| Delegated work: subcontractor access, scope guardrails, blast radius, split payouts | Not started | Specified in 1.15 and 1.16. |
| Analytics, audit logs, webhooks, open API | Not started | |
| AI platform intelligence | Not started, planned post-core | See [AI Platform Intelligence](ai/index.md). |

Because so little beyond the project and task execution surface exists yet, the sections below are written with enough detail to build from directly rather than as a high-level feature list.

---

## 1. Functional Requirements

### 1.1 Visual-First Project, Task & Asset Lifecycle

**Status:** the task and project half of this section is implemented. The asset half is designed at the domain layer, as the `Asset` aggregate under the PM module, but has no API surface yet.

* **FR-1.11 Recursive Asset Trees:** Support N-level deep task nesting.
  This allows creative teams to structure work exactly like their design folder hierarchies. A branding project can contain logo work, which can contain variations, which can contain social exports, without forcing users into rigid depth limits. Implemented as part of the Task aggregate.

* **FR-1.12 Complex Task Modeling:** Support Epics, Features, Stories, Tasks, Issues.
  Mirama must accommodate both structured planning and creative workflows. Some teams work with epics and stories, others work with design deliverables. The system needs to support both without friction. Task type and workflow status are configurable per organization through the existing Workflow feature set (statuses, priorities, task statuses, task priorities), which is implemented.

* **FR-1.13 Asset-Centric Tasks:** Tasks act as containers for assets.
  Instead of assets being secondary attachments, the deliverable becomes the core object a task is built around. Users interact with mockups, videos, documents and code artifacts first, with metadata and discussion surrounding them. The `Asset` aggregate already models this correctly: an asset optionally links to a `ProjectId`, carries a `Type` (Image, Video, Audio, Document, ThreeDModel, Font, Animation, SourceFile, Archive, Other, so the deliverable does not have to be visual), and moves through an approval `Status` of Draft, InReview, ChangesRequested, Approved, Rejected or Archived. What remains to be built is the API surface: create an asset, attach it to a task, list assets on a task, and submit it for review, request changes, approve or reject it through the endpoints the domain methods already support (`SubmitForReview`, `Approve`, `RequestChanges`, `Reject`, `Archive`).

* **FR-1.14 Version Tracking:** Upload iterations to the same asset.
  Designers and consultants alike frequently iterate. This ensures revisions remain grouped together, allowing stakeholders to follow the evolution of a deliverable from first draft to final production version. The domain model already tracks this through `AssetVersion`, which auto-increments a `VersionNumber`, records `UploadedByMemberId`, `UploadedAt` and free-text `Notes`, and the asset's `CurrentVersionId` pointer, changed through `AddVersion` and `SetCurrentVersion`. What remains to be built is the storage integration described in 1.2 through 1.4: `AssetVersion` today has a `StorageKey`, `FileName`, `FileSizeBytes` and `MimeType` field ready to receive a real upload, but nothing populates them yet.

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

**Status:** not started. Nothing in this section exists in the backend yet. The `AssetVersion.StorageKey` field is ready to hold a reference into whichever storage backend is chosen, but no upload endpoint, signed URL issuer or storage client has been built.

* **FR-1.21 Large Asset Upload Support:** Support uploads up to 1GB (for now).
  Creative and technical workflows often include large PSD files, 4K video exports, After Effects renders, RAW photography or large source archives. The system must comfortably handle these without forcing users to rely on external storage tools. The cap is a starting point, not a hard architectural ceiling, and should be a configurable value rather than hardcoded.

* **FR-1.22 Direct-to-Storage Uploads:** Upload directly to S3.
  Large file transfers should never pass through the application server. Instead, the client requests a pre-signed URL from the API, then uploads directly to S3 using that URL, and finally confirms completion back to the API, which records the resulting `StorageKey`, `FileName`, `FileSizeBytes` and `MimeType` onto a new `AssetVersion` via the existing `AddVersion` domain method. This keeps the API stateless and avoids buffering large files in application memory.

* **FR-1.23 Chunked Multipart Upload:** Files split into chunks.
  Uploading in smaller chunks avoids timeouts and improves reliability, especially on slower or unstable internet connections. S3 multipart upload is the natural backing mechanism, coordinated through the pre-signed URL flow in FR-1.22.

* **FR-1.24 Resume Interrupted Uploads:** Support resumable uploads.
  If a connection drops mid-upload, users should be able to continue from where they left off rather than restarting a 900MB upload. This depends on tracking in-progress multipart upload state (upload id, completed part numbers) somewhere queryable by the client on reconnect, most naturally a short-lived record keyed by a pending `AssetVersion` before it is finalized.

* **FR-1.25 Original Format Preservation:** Store original file.
  Mirama should act as a centralized asset repository, meaning the exact original file must always remain available for download. Background processing in 1.4 generates derived previews and proxies, but the original referenced by `AssetVersion.StorageKey` is never mutated or deleted while the version exists.

* **FR-1.26 Upload Progress Feedback:** Real-time progress UI.
  Users need clear feedback when uploading large files, including percentage progress and retry indicators. This is a frontend concern layered on top of the multipart upload state from FR-1.23 and FR-1.24; no backend work beyond exposing upload progress state is required.

---

### 1.3 Asset Versioning & Fast Retrieval

**Status:** the domain model exists (`AssetVersion`, `Asset.CurrentVersionId`, `Asset.SetCurrentVersion`), but there is no read API yet, so none of the retrieval behavior below is built.

* **FR-1.31 Fast Asset History Loading:** Optimized version retrieval.
  Even assets with dozens of revisions should load instantly so users can navigate between versions without delays. `GetAssetById` should default to returning the current version's metadata plus a lightweight list of the remaining versions (id, version number, label, uploader, timestamp), not every version's full detail, in the same paginated pattern already used across the PM module's list endpoints.

* **FR-1.32 Active Version Prioritization:** Fetch active version first.
  Boards and dashboards should render quickly by loading only the currently active version, resolved through `Asset.CurrentVersionId`, before loading the full history. This is the version used everywhere the asset is referenced outside its own detail view, for example on a task card or a kanban board.

* **FR-1.33 Lazy Loading History:** Load versions incrementally.
  Historical versions load on demand to prevent performance degradation. A dedicated `GetAssetVersions` endpoint, paginated the same way as other list endpoints in this codebase, serves this rather than embedding the full version list in the asset response.

* **FR-1.34 Version Metadata Indexing:** Indexed metadata.
  Version information such as uploader, status and timestamps should be searchable and filterable. This needs a database index on `AssetVersion.UploadedAt` and `UploadedByMemberId` at minimum once the module has migrations.

* **FR-1.35 Evolution Toggle:** Switch versions instantly.
  Designers and stakeholders can quickly compare revisions and approvals by calling `SetCurrentVersion` on the aggregate, which already validates that the target version belongs to the asset before switching.

---

### 1.4 Background Asset Processing

**Status:** not started. This depends on FR-1.22's upload flow existing first, since there is nothing to process yet.

* **FR-1.41 Background Compression:** Generate compressed previews.
  Large assets automatically generate smaller preview versions for fast rendering. On upload confirmation, the API publishes an event (following the existing domain event and outbox pattern already used elsewhere in the codebase) that a background worker consumes to generate a preview and write its storage key back onto the `AssetVersion`, which needs a new field to hold it.

* **FR-1.42 Multi-Resolution Assets:** Create thumbnails.
  Different UI surfaces require different resolutions: a thumbnail for a task card, a mid-resolution preview for a board, and the full asset for the detail view. Video assets need an additional frame-accurate thumbnail extracted at a fixed offset.

* **FR-1.43 Preview-First Rendering:** Load compressed assets first.
  Users should see visual feedback immediately while larger originals load in the background on request. This matters most in the approval flow (1.6), where a reviewer needs to see the intended version quickly without waiting on a multi-hundred-megabyte original.

* **FR-1.44 Non-Blocking Processing:** Compression async.
  Upload completion should not wait for processing tasks. The version is visible and usable, marked as processing, the moment the original finishes uploading; previews attach to it asynchronously without blocking the asset's approval workflow.

---

### 1.5 Multi-Tenant Identity & Workspace Hierarchy

**Status:** implemented. This is the most mature part of the system: Organizations, Teams, Tags, Members, Invitations and Roles and Policies (PBAC) all exist with applied migrations in the Identity module.

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

**Status:** domain modeled, not built. The `AssetFeedback` entity already supports exactly the click-to-spot model described in FR-1.64, including a threaded reply list and an open, resolved or won't-fix status, but there is no endpoint to create feedback, no notification wiring, and no dedicated review surface.

* **FR-1.61 Visual Review Mode:** Dedicated stakeholder view for mockup inspection and approvals.  
  The system needs to provide a dedicated stakeholder view for inspecting high-resolution mockups and leaving approval states. This gives clients and managers a simplified interface focused on reviewing visuals rather than navigating project structure. Approval itself is a status transition already modeled on the aggregate (`SubmitForReview`, `Approve`, `RequestChanges`, `Reject`), so this requirement is largely a dedicated frontend surface over an API that mostly needs to exist first.

* **FR-1.62 Asset Direct-Link:** Secure, time-limited URLs for asset viewing.  
  The system must generate secure, time-limited URLs for viewing high-resolution assets. These links allow external reviewers to access visuals without exposing the entire workspace or storage bucket. Time-limited access, per NFR-2.41, should default to expiring within five minutes and be reissued transparently by the client rather than requiring the user to notice and refresh a stale link.

* **FR-1.63 Annotated Notifications:** Notifications linked to assets or comment threads.  
  System-managed notifications are linked directly to a specific asset version or feedback thread, not just the parent task. Notifications should bring users back to the exact asset version or discussion, reducing friction in feedback loops. This has no domain model yet; a `Notification` entity referencing an asset, version and feedback id needs to be designed.

* **FR-1.64 Native Click-on-Spot Annotation:** Pin comments to specific coordinates on images, PDFs and video.  
  Reviewers must be able to click any point on a visual asset and leave a contextual comment at that exact location. This eliminates the need for external proofing tools such as Frame.io or Approval Studio and keeps feedback directly inside the task. The coordinate model already exists as `FeedbackAnnotation`: X, Y, Width and Height as a 0 to 1 percentage of the asset's dimensions for images, StartSeconds and EndSeconds for video and audio, and PageNumber for documents, with only the fields relevant to the asset type populated. Annotations are visible as pins on the asset surface, threaded through `AssetFeedback.Replies`, and resolvable through the existing `Resolve` and `MarkWontFix` domain methods. What is missing is the API layer: create feedback with an optional annotation, list feedback per version, add and remove replies, and resolve or mark won't fix.

* **FR-1.65 Non-Visual Gated Delivery:** Extend delivery gating to non-visual deliverables.  
  Not every engagement produces an image or a video. A fractional executive's board deck, a technical consultant's audit report or a private repository link deserves the same protection as a brand asset: locked until the client has approved and paid, released as a deliberate action rather than a default. The existing `AssetType` enum already includes Document, SourceFile and Archive alongside Image, Video, Audio, ThreeDModel, Font, Animation and Other, so the gating logic should key off approval status and payment state rather than asset type, and apply uniformly across all of them.

---

### 1.7 Platform Intelligence & Integration

**Status:** stateless JWT authentication is implemented as part of the Identity module's Auth feature set. Favourites and Google Calendar sync are not started.

* **FR-1.71 Favourite Entities & Routes:** Favorite projects, tasks, assets or views.  
  Users must be able to favorite projects, tasks, asset versions or specific views. This allows frequently accessed deliverables to appear instantly in the sidebar or on the application dashboard for faster navigation.

* **FR-1.72 Google Ecosystem Integration:** Google authentication and calendar sync.  
  The system shall support authentication via Google IdP and synchronization with Google Calendar. Project milestones and deadlines can automatically appear in calendars, aligning creative production with planning tools teams already use.

* **FR-1.73 Stateless Authentication with JWTs:** The system shall use stateless JSON Web Tokens (JWTs) for user authentication and session management.
  This allows any service or API to validate a token without querying a central session store, improving performance, horizontal scalability, and reliability across distributed services.

---

### 1.8 Creative Interface & Adaptive Visualization

**Status:** unknown from this pass. This section is primarily a frontend concern, and the Next.js frontend was not reviewed here; the backend's Kanban board aggregate exists to support FR-1.81, which is a reasonable signal but not confirmation the UI is built.

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

**Status:** implemented at the data layer. Milestone create, update, delete and list all exist under the Projects feature set; whether timeline and calendar visualizations exist depends on the unreviewed frontend.

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

**Status:** not started. Nothing described below exists yet, including the underlying `TimeEntry` concept.

This section covers the financial and operational foundation that separates a professional delivery platform from a generic task tracker. Without time tracking and workload visibility, the platform cannot support billing, profitability analysis, capacity planning, or the payout distribution in 1.16, which depends on knowing exactly whose logged time and which deliverables a given invoice line covers.

* **FR-1.101 Native Task Timer:** A built-in start/stop timer on every task.  
  Every task must have a native time tracking control. Leads, subcontractors and consultants log time directly on the work object without switching to a separate time tracker. A logged time entry needs, at minimum, a start and end timestamp or duration, the member who logged it, the task it is against, and an optional note. Because a task can carry contributions from both the lead and one or more subcontractors (1.15), a time entry must record which member logged it distinctly from who is assigned to the task, so a later payout split can attribute hours correctly. This data is the source of truth for billing, profitability reporting and the split payout engine in 1.16.

* **FR-1.102 Billable vs. Non-Billable Classification:** Time entries are classified as billable or non-billable.  
  Each time entry must be explicitly classified, either at creation or by editing it afterward. Billable time flows into client invoicing; non-billable time represents internal overhead, rework or unbilled scope. Without this distinction, a project can appear profitable on paper while the actual margin is negative, and a subcontractor payout calculated against unfiltered logged time would overpay for work the client never agreed to fund.

* **FR-1.103 Budget Tracking & Burn Alerts:** Projects have a defined budget (hours or currency) with threshold alerts.  
  Project managers must be able to set a budget against a project, expressed in hours, currency, or both. The system calculates burn rate from logged time entries and notifies relevant members when the project reaches 50%, 75% and 100% of the allocated budget. This prevents silent over-servicing, a common cause of margin loss for both solo operators and agencies delegating to subcontractors, where a single lead may not notice a subcontractor quietly running over on a fixed-fee task.

* **FR-1.104 Capacity & Workload View:** A manager-level view showing team member utilization across all active projects.  
  The system must provide a cross-project workload dashboard showing each team member's scheduled or logged hours in a given period, including subcontractors invited under FR-1.151, scoped to only the projects that subcontractor has access to when the viewer is not the lead. Overbooked members are surfaced visually as over capacity, available capacity as under capacity, so leads can redistribute work or bring in additional help before deadlines are missed rather than after.

---

### 1.11 Client & Intake Management (CRM Foundation)

**Status:** started, not functional. The `Client` aggregate exists with `Contacts`, a `ClientPortalInvitation` flow that issues a token and validates it is not expired or already used, and a resulting `ClientPortalUser`. None of it has an applied database migration yet, so it cannot run against a real database as it stands, and there is no intake form and no lead-to-project conversion.

This section establishes the relational client model and intake pipeline that connects the CRM layer to project execution. The `Client` entity is a first-class object in the data model, linked to intake briefs, projects, time records and invoices. If the client is a tag or a folder name rather than a relational object, cross-client reporting and billing break.

* **FR-1.111 Custom Intake & Request Forms:** Configurable external forms for client brief submission.  
  Organizations must be able to create branded, configurable intake forms that external clients fill out to submit a brief or request. A submitted form automatically creates a Lead record or a Project depending on configuration. This gives freelancers and agencies a professional intake pipeline without a separate CRM tool. There is no `Lead` concept in the domain model yet; it needs to be introduced, most naturally as a pre-`Client` state or a status on `Client` itself, since `Client.Status` already models Prospect, Active and Archived and Prospect is functionally a lead.

* **FR-1.112 Client Portal & Guest Access:** A simplified, scoped view for external clients.  
  External clients must be able to access a dedicated portal that shows project progress, active assets for review, and approved deliverables, without seeing internal budget data, team discussions, subcontractor identities or rates, or organizational structure. The portal requires no account creation if accessed via a secure invitation link. The invitation and acceptance flow already exists on the `Client` aggregate (`InviteContact`, `AcceptInvitation`, `RevokePortalAccess`); what remains is the portal's own scoped read surface and, per FR-1.151, keeping subcontractor-related data out of it by construction rather than by filtering it after the fact.

* **FR-1.113 Public/Private Privacy Toggles on Tasks and Comments:** Granular control over what clients see in the portal.  
  Project managers must be able to mark individual tasks or comment threads as internal-only. Internal content is hidden from the client portal regardless of project-level sharing settings. This allows teams to have honest internal discussions, including discussions with or about a subcontractor, about revisions or issues without exposing them to the client. Neither the flag nor the enforcement exists yet.

* **FR-1.114 Lead-to-Project Conversion:** A brief submitted via intake form links to a structured project.  
  When a Lead is converted to a Project, the client relationship record, brief data and any attachments carry over automatically. The `Client` object remains the parent entity linking the intake brief, the project, logged time and invoices, maintaining a continuous audit trail of the client engagement. This depends on FR-1.111's Lead concept existing first.

---

### 1.12 Automation, Templates & Search

**Status:** project templates are implemented in full, including cycle, milestone and task templates. Automated workflow triggers and global search are not started.

* **FR-1.121 Project Templates:** One-click project creation from a saved template.  
  Organizations must be able to save any project structure as a reusable template - including pre-set tasks, subtasks, roles, custom statuses and estimated durations. Common service packages (e.g., "Brand Identity," "Campaign Package," "Website Build") become one-click setups rather than manual recreation. This is the primary scaling mechanism for small agencies delivering repeatable services.

* **FR-1.122 Automated Workflow Triggers:** Event-driven rule engine for status changes and notifications.  
  The system shall support configurable automations of the form: "When [condition], then [action]." Examples: when a task status changes to "Approved," automatically send a notification to the client and move the task to "Final Delivery." When a milestone deadline is 48 hours away with no activity, notify the project lead. Automations reduce manual overhead and enforce consistent workflows across teams.

* **FR-1.123 Global Search:** A unified search engine across tasks, files and conversations.  
  Users must be able to search across task names, file names, comment text and client records in a single query from anywhere in the application. Search results are scoped to the user's active organization and respect all permission boundaries. This is critical for large projects where content accumulates quickly and navigation becomes insufficient.

---

### 1.14 AI Platform Intelligence

**Status:** not started, and deliberately sequenced last. See [AI Platform Intelligence](ai/index.md) for the full design.

This section defines requirements for the AI capabilities embedded across the Mirama engagement lifecycle. Each capability targets a specific workflow pain point rather than adding a generic "AI assistant" layer. The goal is augmentation of existing workflows, not replacement of user judgment.

* **FR-1.141 AI Brief Intelligence:** When a client submits an intake form, the system shall use an LLM to parse the brief text and extract key structured data - deliverable types, desired timelines, budget signals, and scope indicators. Extracted data shall pre-populate project creation fields and suggest the most relevant project template from the organization's library. This removes manual interpretation overhead from the intake-to-project conversion step and connects directly to FR-1.114 (Lead-to-Project Conversion).

* **FR-1.142 Vision-Based Asset Tagging:** On upload, image and PDF assets shall be processed by a multimodal AI model to generate descriptive tags covering visual content, dominant colors, style characteristics and detected objects or subjects. Tags are indexed alongside file metadata and surfaced within global search (FR-1.123), enabling visual assets to be found by content rather than file name alone.

* **FR-1.143 Annotation Feedback Summarization:** After a client or reviewer leaves multiple point annotations across one or more asset versions (FR-1.64), the system shall offer an AI-generated summary consolidating all comments into a structured list of actionable revision notes grouped by theme. The summary is attached to the task and visible to the production team, reducing the time spent interpreting scattered client feedback.

* **FR-1.144 Predictive Risk Detection:** The system shall continuously analyze active projects for signals of budget overrun and deadline risk - including burn rate velocity, unresolved task dependencies, blocked tasks, and low logged-hour ratios relative to estimated work. When a risk threshold is crossed, a prioritized alert is surfaced to the project lead with the specific contributing factors. This extends the budget burn alerts in FR-1.103 with predictive rather than threshold-only logic.

* **FR-1.145 Conversational Project Copilot:** Users shall be able to query the platform in natural language from a persistent command interface. Queries may retrieve information ("show me all tasks awaiting client approval on Project X"), trigger actions ("create a task for logo revisions under the Brand Identity milestone"), or request summaries ("what is the current budget burn across all active projects?"). Responses are scoped to the user's organization and respect all PBAC permission boundaries.

* **FR-1.146 Smart Automation Suggestions:** The system shall observe recurring workflow patterns across teams - such as consistently moving tasks to a specific status after a client comment, or always creating the same task type at project kickoff - and surface suggested automation rules (FR-1.122) based on observed behavior. Suggestions are presented as a proposed rule with a one-click acceptance flow, reducing the expertise required to configure workflow automation.

---

### 1.13 Platform Extensibility

**Status:** not started as a deliberate capability. A REST API exists by construction since every module exposes its features that way, but it is not versioned as a public contract, not documented for external consumption, and neither webhooks nor audit logs exist yet.

* **FR-1.131 Open API:** A versioned, publicly documented REST API.  
  All core entities (clients, projects, tasks, time entries, assets) must be accessible via a documented REST API. This allows users to integrate Mirama with tools not natively supported - custom scripts, internal dashboards, or industry-specific software - without relying on Mirama to build every integration.

* **FR-1.132 Webhooks:** Outbound event notifications for key platform events.  
  The system must support configurable webhooks that fire on defined events (task status change, new comment, asset uploaded, milestone reached). This enables real-time integration with Slack, email services, Adobe Creative Cloud and custom workflow tooling without polling the API.

* **FR-1.133 Audit Logs:** An immutable record of all significant platform actions.  
  The system must record who performed what action, on what resource, and when - for all destructive or permission-sensitive operations (deadline changes, file deletions, role modifications, billing record edits). Audit logs are read-only, scoped to the organization and accessible to Administrators. This is a non-negotiable requirement for large firm accountability and compliance.

---

### 1.15 Delegated Work: Subcontractor Access & Scope Protection

**Status:** not started. Nothing in this section has a domain model yet. This is the section that most defines Mirama as a Cross-disciplinary Delegation OS rather than another project tracker, and it should be read as the highest priority area to design and build next, ahead of AI and ahead of most of the analytics work in 1.13.

This section establishes a second external-facing scope, sitting alongside the CLIENT scope defined in 1.11, for the people doing delegated work rather than the people paying for it. Without this distinction, a lead who wants to bring in a subcontractor has to either hand them a full team seat, exposing client budgets and other people's rates, or manage the whole arrangement outside the platform over email and spreadsheets.

* **FR-1.151 Subcontractor Identity & Scoped Access:** A distinct access tier for delegated collaborators.  
  Subcontractors are invited per project, the same way a client is invited through the existing `ClientPortalInvitation` pattern in 1.11, and only see the tasks and assets assigned to them plus their own payout status. Client budgets, other subcontractors' rates and the wider organization structure stay hidden. Concretely, this needs a `Subcontractor` entity distinct from both `Member` (Identity module) and `Client.Contact` (Clients module), an invitation and acceptance flow mirroring `ClientPortalInvitation`, and a new authorization scope enforced at the same layer as the existing tenant, organization and client scopes, never falling back to a broader query if a lookup would otherwise resolve outside the subcontractor's assignments. Subcontractors are frequently shared across several leads and platforms, so a lightweight, scoped invitation model keeps onboarding fast without requiring a full member seat.

* **FR-1.152 Scope Guardrails & Change Orders:** Enforceable deliverable and revision boundaries.  
  Deliverable boundaries and revision limits are defined per milestone, most naturally as a revision count cap and a free-text scope description attached to the `Milestone` entity. When a client leaves feedback, through `AssetFeedback` in 1.6, that would push a deliverable past its defined revision limit, or that a reviewer flags as describing new work rather than a revision, the system prompts the lead to raise a `ChangeOrder`: a priced, described addition to scope that requires the client's explicit sign-off, most simply an e-signature or a typed confirmation tied to their portal identity, before the lead resumes work covered by it. Unpaid revision cycles are one of the largest sources of lost margin for solo operators and micro-agencies alike, and tying scope protection directly to the proofing and milestone surface makes the boundary enforceable rather than aspirational.

* **FR-1.153 Blast Radius Analysis:** Show the downstream effect of a change before it is committed.  
  Given a proposed change to a milestone date, an asset or a scope item, the system computes and displays every downstream task, subcontractor and fee affected, by walking the existing task dependency graph (FR-1.17) outward from the changed item, then resolving which subcontractors are assigned to each affected task and what fee, if any, is tied to it through the change order or time tracking data. A single "small" client request can ripple through several dependent subcontractor tasks and shift a delivery date the lead has already committed to. Seeing the full downstream effect before agreeing to a change turns a guess into a decision. At the scale of a single organization's task graph, this is answerable with recursive queries over the existing relational model; it does not require a graph database to ship a first version, see [System Architecture](system-architecture.md).

---

### 1.16 Payout Distribution & Audit Trace

**Status:** not started. Depends on FR-1.101 through FR-1.104 (time tracking) and FR-1.151 (subcontractor identity) existing first.

* **FR-1.161 Split Payout Engine:** Automatic payout splitting on milestone approval.  
  On milestone approval and payment, the system automatically splits the payout between the lead and the assigned subcontractor or subcontractors, according to a pre-agreed rate or percentage set when the subcontractor was assigned to the task or project. This is built on Stripe Connect: the platform account receives the client's payment, and Stripe Connect transfers move the subcontractor's share to their connected account, with the split percentage or fixed amount, the source milestone or invoice, and the resulting transfer id all recorded. Today this step is a manual bank transfer or a second invoice done from memory once the client has paid. Automating it removes both the administrative burden and the risk of a subcontractor being paid late or incorrectly. Note that this is entirely distinct from the tenant subscription billing that already exists in the Identity module; that code answers "what does this organization pay Mirama," this requirement answers "what does the client pay the organization, and how is it split."

* **FR-1.162 Audit-Proof Invoice Trace:** Every invoice line traceable to its source.  
  Every invoice line item links back to the time entry, approved asset version or change order that produced it, as a stored reference, not a reconstructable-if-you-dig-through-logs relationship. When a client or their finance team questions a bill, the answer needs to be a link, not a memory. This extends the audit log requirement in 1.13 to the billing surface specifically, and depends on an `Invoice` and `InvoiceLine` entity that does not exist yet.

* **FR-1.163 Contract Clause Terms:** Structured, queryable retainer and pass-through terms.  
  Retainer caps, overage rates and subcontractor pass-through rates are represented as structured terms attached to a client or project, most naturally a `ContractTerm` entity with a type (retainer cap, overage rate, subcontractor pass-through rate), a value, and an effective date range, rather than living only in a signed PDF outside the system. Fractional executives and consultants running a retainer-plus-overage model need the system to know when overage starts, what rate applies, and whether a different rate applies to a subcontractor looped in to help deliver it, so that FR-1.161's split can be calculated correctly without manual intervention.

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

* **NFR-2.46 Subcontractor Scope Enforcement:** The subcontractor access tier (FR-1.151) must be enforced at the same layer and with the same rigor as the CLIENT scope. A subcontractor request that resolves outside its assigned tasks or its own payout record must fail closed, never fall back to a broader query.

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
