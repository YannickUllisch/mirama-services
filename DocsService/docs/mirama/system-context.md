# Mirama Platform – System Context

This document provides a high-level overview of the Mirama Platform, outlining its purpose, stakeholders, key assumptions, core use cases and architectural context. It serves as a foundation for understanding how Mirama fits into the broader business and technical landscape.

---

## Purpose

Mirama is a visual-first, high-performance production environment designed for creative teams managing complex projects, assets and workflows. The platform enables seamless collaboration, asset management and project tracking, supporting both structured and creative processes.

---

## Stakeholders

- **Clients:** Creative agencies, design studios, marketing teams and enterprise customers who use Mirama to manage projects and assets.
- **Developers:** Internal engineering teams responsible for building, maintaining and evolving the platform.
- **Business Owners:** Product managers, project sponsors, and company leadership overseeing platform direction and ROI.
- **External Collaborators:** Freelancers, contractors and reviewers who interact with the platform for specific projects or deliverables.
- **Support & Operations:** Teams responsible for platform uptime, support and customer success.

---

## Key Assumptions

- **Timelines:** The platform will be delivered in iterative phases, with core features prioritized for early releases and advanced features added in later phases.
- **Milestones:** Major milestones include MVP launch, multi-tenant support and advanced analytics integration.
- **Cloud-Native:** The platform will make use of cloud infrastructure (AWS) for scalability, reliability and global reach, while adhering to the AWS Well-Architected framework.
- **Security:** Strict tenant and organization isolation is enforced at all layers.
- **User Base:** The platform must support both small teams and large enterprises, scaling as adoption grows.

---

## Core Use Cases & Features

### Use Case Diagrams

Below are three primary use case diagrams representing the most important workflows in Mirama. Each diagram is followed by a detailed description of the actors, interactions and system boundaries.

#### 1. Project & Asset Lifecycle Management

![Project and Asset Management Use Case](../images/use_case1.png)

**Description:**  
This diagram illustrates how users create projects, manage recursive task and asset hierarchies, upload and version assets, and collaborate through comments and approvals. It highlights the core workflow for creative production teams.

#### 2. Multi-Tenant Access & Collaboration

![Multi-tenant and Collaboration Use Case](../images/use_case2.png)

**Description:**  
This diagram focuses on user authentication, context switching between organizations/tenants, team-based access control, and delegated invitations. It demonstrates how Mirama supports agencies and enterprises working across multiple clients and teams.

#### 3. Review, Approval & Notification Workflow

![Review, Approval and Notification Workflow Use Case](../images/use_case3.png)

**Description:**  
This diagram shows the review and approval process, including stakeholder review modes, secure asset sharing, annotated notifications, and milestone tracking. It covers the feedback loop between internal teams and external collaborators.

---

### Must-Have Core Features

- **Recursive Project & Task Management:** Support for N-level deep task and asset hierarchies, enabling flexible project structures.
- **Large Asset Management:** Direct-to-cloud uploads, chunked/resumable transfers, and original format preservation for files up to 1GB.
- **Asset Versioning:** Fast retrieval, lazy loading, and instant switching between asset versions.
- **Background Processing:** Asynchronous compression, preview generation, and non-blocking uploads.
- **Multi-Tenant & Team-Based Access:** Strict data isolation, context switching, team partitioning, and delegated invitations.
- **Review & Approval Workflow:** Stakeholder review modes, secure asset links, and annotated notifications.
- **Platform Intelligence:** Favorites, Google integration, and stateless JWT authentication.
- **Creative Visualization:** Synchronized Kanban, list, table, and Gantt views; contextual mockup visualization; approval dashboards.
- **Milestone Tracking:** Define, aggregate, and visualize project milestones and timelines.

### Nice-to-Have Features

- **Advanced Analytics:** Usage insights, project health dashboards.
- **Custom Integrations:** Deeper integrations with third-party tools beyond Google.
- **Automated Asset Tagging:** AI-driven metadata extraction and search.

---

## System Context Diagram (C4 Model – System Context)

![C4 System Context Diagram](../images/c4-context-diagram.png)

---

## External Systems & Integrations

- **AWS Cloud:** S3 (asset storage), RDS (database), ElastiCache (caching), ECS (compute), SNS/SQS (queues), CloudFront (CDN).
- **Google IdP & Calendar:** For authentication and calendar synchronization.
- **Email/SMS Providers:** For notifications, invitations and alerts.

---

## Security & Compliance

- **Tenant & Organization Isolation:** Enforced at all API, storage and UI layers.
- **Stateless JWT Authentication:** All services validate tokens independently.
- **Role-Based Access Control:** Fine-grained permissions for teams, projects, and assets.
- **Encryption:** All data encrypted at rest and in transit.

---

## Summary

Mirama is architected to deliver a robust, scalable and secure environment for creative production teams. Its modular design, cloud-native infrastructure and focus on visual workflows ensure it can adapt to evolving business needs while maintaining high standards for performance, security and usability.
