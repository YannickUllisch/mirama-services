# Mirama Platform – Core Domain Model

This document describes the core domain model for the Mirama Platform, focusing on the main entities, their relationships, and the principles that guide the system’s structure. The model is designed to support visual-first project management, multi-tenant collaboration, and scalable asset workflows for creative teams.

---

## Key Domain Entities

### Tenant

- Represents the centralized platform entity for billing and high-level settings.
- Has a one-to-one relationship with a root user (main admin).
- Can include many organizations, which are billed to the main admin/root user.
- Other users can assume a tenant if invited to a specific organization, enabling multi-tenant workflows.

### Team

- Subdivision within an organization (e.g., Design, Marketing, Finance).
- Teams have access to specific projects and assets.
- Supports granular access control and delegated invitations.

### User

- Any person with access to the platform (designer, manager, client, collaborator).
- Can belong to multiple organizations and teams.
- Role-based permissions (admin, member, external collaborator, client).

### Project

- The primary container for all work, assets, and tasks.
- Belongs to an organization and may be partitioned by team.
- Contains a hierarchy of tasks and associated assets.

### Task

- Represents a unit of work (epic, feature, story, issue, or creative deliverable).
- Supports recursive nesting (N-level deep task trees).
- Each task can have associated assets, comments, and approvals.

### Asset

- Any file or visual deliverable (image, mockup, video, document).
- Linked to a specific task.
- Supports versioning, background processing, and multi-resolution previews.

### Asset Version

- Represents a specific iteration of an asset.
- Tracks uploader, timestamps, approval status, and metadata.
- Enables fast switching and comparison between versions.

### Comment & Annotation

- Threaded discussions attached to tasks or assets.
- Supports visual annotations for precise feedback.
- Drives the review and approval workflow.

### Milestone

- Key delivery checkpoints within a project (e.g., Client Review, Final Export).
- Aggregates progress from associated tasks and assets.
- Visualized in Gantt and timeline views.

---

## Relationships Overview

- **Tenant** can have zero to many **Organizations**.
- **Organization** contains many **Teams** and **Projects**.
- **Team** has many **Users** and access to multiple **Projects**.
- **User** can belong to multiple **Organizations** and **Teams**.
- **User** only owns a single **Tenant**, automatically assigned on sign-up.
- **Project** contains a tree of **Tasks** and a set of **Milestones**.
- **Task** can have child **Tasks**, multiple **Assets**, and **Comments**.
- **Asset** belongs to a **Task** and has multiple **Asset Versions**.
- **Comment** can be attached to a **Task** or **Asset**.
- **Milestone** aggregates progress from **Tasks** and **Assets**.

---

## Domain Principles

- **Visual-First:** Assets and their evolution are central to all workflows.
- **Multi-Tenancy:** Strict isolation between organizations and tenants.
- **Recursive Structure:** Tasks and assets can be deeply nested to match real creative processes.
- **Versioning:** Every asset supports full version history and fast retrieval.
- **Collaboration:** Comments, annotations, and approvals are first-class citizens.
- **Security & Access Control:** Role-based permissions, delegated invitations, and secure sharing.
- **Scalability:** Designed to support organizations of any size, from small studios to large enterprises.

---

This domain model provides the foundation for Mirama’s flexible, visual-first, and collaborative project management experience.
