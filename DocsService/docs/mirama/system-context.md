# Mirama Platform – System Context

This document provides a high-level overview of the Mirama Platform, outlining its purpose, stakeholders, key assumptions, core use cases and architectural context. It serves as a foundation for understanding how Mirama fits into the broader business and technical landscape.

---

## Purpose

Mirama is a Creative Operating System - a unified platform that handles the specific operational complexity of creative work while maintaining the data integrity needed for financial reporting and client accountability. It combines a CRM foundation (client relationships, intake and briefs), a full project management execution layer (task hierarchies, asset proofing, approvals), and resource visibility (time tracking, workload, budget burn) into a single coherent workspace.

The platform is designed to serve creative freelancers, small agencies and medium sized creative firms - each with distinct needs that a generic task tracker cannot address. A lean ERP layer is the long-term direction, built on the same relational data model from day one rather than retrofitted later.

---

## Stakeholders

- **Freelancers:** Solo creative operators who need professional client intake, time tracking and a client-facing portal to operate at agency standards without agency overhead.
- **Small Agencies:** Teams of 2–50 people who need project templates, cross-project workload visibility and collaborative client portals to scale their delivery without tool sprawl.
- **Large Creative Firms:** Organizations that require capacity planning across departments, audit logs for accountability, fine-grained access control and budget burn reporting at scale.
- **End Clients / External Collaborators:** Freelancers, contractors and reviewers who interact with the platform through the client portal or secure asset review links for feedback and approvals.
- **Developers:** Internal engineering teams responsible for building, maintaining and evolving the platform.
- **Business Owners / Project Sponsors:** Product managers and leadership overseeing platform direction and ROI.
- **Support & Operations:** Teams responsible for platform uptime, support and customer success.

---

## Key Assumptions

- **Relational Client Model:** The `Client` entity is a first-class object in the database, linked to intake briefs, projects, time records and (in future) invoices. Siloed data structures will break reporting - this is a foundational constraint.
- **Cloud-Native:** The platform makes use of cloud infrastructure (AWS) for scalability, reliability and global reach, while adhering to the AWS Well-Architected framework.
- **Security:** Strict tenant and organization isolation is enforced at all layers, with PBAC authorization for fine-grained access control within each organization.
- **Open by Default:** An open API and webhook system is part of the platform contract - users will integrate with Slack, Adobe Creative Cloud, Google Drive and other tools even if Mirama covers the core.
- **Iterative Delivery:** Core PM and CRM capabilities are the immediate priority. Resource management and the financial/ERP layer follow once the execution foundation is solid.
- **User Base:** The platform must serve freelancers through to large firms, requiring flexible access models (guest portals, fine-grained team permissions) alongside enterprise-grade controls.

---

## Core Use Cases & Features

### Use Case Diagrams

Below are three primary use case diagrams representing the most important workflows in Mirama. Each diagram is followed by a detailed description of the actors, interactions and system boundaries.

#### 1. Project & Asset Lifecycle Management

![Project and Asset Management Use Case](../images/use_case1.png)

**Description:**  
This diagram illustrates how users create projects, manage recursive task and asset hierarchies, upload and version assets, and collaborate through inline annotation and approvals. It highlights the core workflow for creative production teams - from brief creation through to final delivery.

#### 2. Multi-Tenant Access & Collaboration

![Multi-tenant and Collaboration Use Case](../images/use_case2.png)

**Description:**  
This diagram focuses on user authentication, context switching between organizations/tenants, team-based access control, and delegated invitations. It demonstrates how Mirama supports agencies and enterprises working across multiple clients and teams, including the client portal experience for external stakeholders.

#### 3. Review, Approval & Notification Workflow

![Review, Approval and Notification Workflow Use Case](../images/use_case3.png)

**Description:**  
This diagram shows the review and approval process, including native proofing with click-on-spot annotation, version comparison, secure asset sharing, and milestone tracking. It covers the feedback loop between internal teams and external collaborators.

---

### Must-Have Core Features

**Execution Engine:**
- **Recursive Task Hierarchies:** Workspace → Client → Project → Task → Sub-task; N-level deep with progress aggregation.
- **Flexible Views:** Kanban boards (high-velocity design work), Gantt/timeline (campaign planning and dependencies), Calendar (content-heavy teams).
- **Task Dependencies:** Dependency tracking with automatic downstream shift when upstream tasks are delayed.
- **Custom Statuses:** Beyond "In Progress" - support for "Internal Review," "Client Review," "Awaiting Assets," and user-defined stages.

**Asset Collaboration & Proofing:**
- **Large Asset Upload:** Support for files up to 1GB via direct-to-S3 multipart uploads with resume support.
- **Native Proofing & Annotation:** Click-on-spot comments on images, PDFs and video - eliminating the need for Frame.io or Markup.io.
- **Version Control:** Stacked version history (v1, v2, v3) with instant switching; the team always works from the active version.
- **Contextual Task-Level Discussion:** Comments live inside the task, not in a general channel.
- **Rich Media Embedding:** Briefs support embedded Figma files, Loom videos and mood boards.

**Resource & Financial Foundation:**
- **Native Time Tracking:** A built-in timer on tasks; this data feeds future profitability reports.
- **Billable vs. Non-Billable Toggle:** Separates client revenue from internal overhead.
- **Budget Burn Alerts:** Notifications at 50%, 75% and 100% of allocated hours or budget.
- **Capacity / Workload View:** Manager-level view showing who is overbooked or available across all projects.

**Client & Intake Management (CRM Foundation):**
- **Custom Intake Forms:** External forms for clients to submit briefs that automatically create a Lead or Project.
- **Client Portal / Guest Access:** A simplified view where clients see progress and approve work without seeing internal budget data or team discussions.
- **Public/Private Privacy Toggles:** Control which tasks and comments are visible in the client portal.
- **Lead-to-Project Conversion:** A brief submitted via intake form flows directly into a structured project, preserving the client relationship link.

**Automation & Scale:**
- **Project Templates:** One-click setups for standard service packages (e.g., "Brand Identity") with pre-set tasks, roles and durations.
- **Automated Triggers:** Rule-based actions - e.g., "When status changes to Approved, send email and move task to Final Delivery."
- **Global Search:** Searches across task names, file names and comment text in a single query.

**Technical Foundation:**
- **Relational Client Object:** Client links to Briefs (CRM), Projects (PM) and Time Records (ERP) from day one.
- **Open API & Webhooks:** Integration surface for Slack, Adobe Creative Cloud, Google Drive and custom workflows.
- **Audit Logs:** Who changed a deadline, deleted a file, or modified permissions - essential for large firm accountability.

### Nice-to-Have Features

- **Advanced Analytics:** Project health dashboards, utilization reports, client profitability summaries.
- **AI-Driven Asset Tagging:** Automated metadata extraction and smart search across large asset libraries.
- **ERP Extension (Future):** Invoice generation, contractor payments and profitability reporting built on the time and client data already captured.

---

## System Context Diagram (C4 Model – System Context)

![C4 System Context Diagram](../images/c4-context-diagram.png)

---

## External Systems & Integrations

- **AWS Cloud:** S3 (asset storage), RDS (database), ElastiCache (caching), ECS (compute), SNS/SQS (queues), CloudFront (CDN).
- **Google IdP & Calendar:** For authentication and calendar synchronization.
- **Email/SMS Providers:** For notifications, invitations and alerts.
- **Design & Creative Tools:** Figma (embed), Loom (embed), Adobe Creative Cloud (webhook integration, future).
- **Communication Tools:** Slack (webhook notifications, future).

---

## Security & Compliance

- **Tenant & Organization Isolation:** Enforced at all API, storage and UI layers.
- **Stateless JWT Authentication:** All services validate tokens independently via OpenIddict (backend) and AWS Cognito (frontend).
- **Policy-Based Access Control (PBAC):** Fine-grained permissions for teams, projects and assets with a Redis-flattened permission matrix for O(1) authorization checks.
- **Encryption:** All data encrypted at rest and in transit.
- **Audit Logs:** Immutable event trail for all destructive or sensitive actions.

---

## Summary

Mirama is architected as the operational backbone for creative businesses - starting with the CRM and project management capabilities every agency needs today, and built on a relational data model that supports resource management and financial reporting tomorrow. Its modular design, cloud-native infrastructure and visual-first approach ensure it can grow alongside the teams it serves.
