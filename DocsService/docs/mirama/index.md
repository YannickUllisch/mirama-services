# Mirama Platform

## Project Name

**Mirama Platform**  
Domain: [mirama.yannickullisch.com]  
Unique Identifier: `mirama-platform`

---

## Project Description

Mirama is an end-to-end client and project operating system for freelancers and service-based teams. The platform's core proposition is covering the entire engagement lifecycle in one place: acquiring a client through an intake form, executing the project with visual task boards and asset proofing, collaborating with the client through a protected portal, and closing the engagement with automatic Stripe billing - without switching tools at any stage.

The niche is intentionally focused. Mirama is built for service businesses that deliver visual output to clients on a project-by-project basis - where the project, the client relationship, and the invoice are all connected. It is visual-first by default but works equally well as a pure project and task management tool for teams that don't need the asset layer.

The target space spans several industries that share the same underlying pain points:

| Vertical | What they deliver |
|---|---|
| Design & brand studios | Logos, identity systems, brand guidelines, print and digital assets |
| Web development agencies | Websites, web applications, landing pages, interactive prototypes |
| Video & film production companies | Commercials, branded content, social video, documentary |
| Marketing & campaign agencies | Campaign assets, social content, ad creatives, content calendars |
| Architecture & interior design firms | Renders, blueprints, mood boards, material palettes, presentation decks |
| Motion & animation studios | Explainer videos, motion graphics, title sequences, animated ads |
| Photography studios | Photo galleries, retouched images, lookbooks, editorial shoots |

Across all of these, the operational problem is the same: client intake is handled in email, projects are tracked in one tool, files are shared through another, client review happens in a third, and billing is assembled manually at the end. Mirama replaces that patchwork.

A key part of Mirama's development was close collaboration with **Mirage.xyz**, a creative design studio based in London. Working directly with them grounded the feature set in real operational problems rather than developer assumptions.

The platform is designed to serve four tiers with meaningfully different needs:

- **Solo Freelancers** - professional client intake, a built-in timer for billable work, Stripe-connected invoicing, and a client portal that makes a one-person operation look and run like a full agency.
- **Boutique Studios & Small Teams (2-20)** - project templates, cross-project workload visibility, collaborative client portals, and shared billing workflows without the overhead of stitching together multiple tools.
- **Mid-Size Agencies (20-75)** - multi-team capacity planning, structured approval workflows, detailed per-client profitability reporting, and the ability to run multiple client organizations under one account without losing isolation between them.
- **Established Studios & Firms (75-200+)** - fine-grained access control across departments, audit trails for compliance and accountability, organization-level analytics, and the scale to onboard new clients and projects without adding operational overhead.

The platform is structured around five interconnected capabilities:

- **Client & Intake Management (CRM):** A relational client object linking intake briefs, projects, time records and invoices from day one - the foundation that makes billing and reporting possible without retrofitting.
- **Visual Project & Task Execution:** N-level task hierarchies, Kanban boards, Gantt timelines, task dependencies and custom production statuses designed around service delivery workflows.
- **Asset Collaboration & Proofing:** Native annotation on images, PDFs and video; stacked version history; automatic watermarking and low-quality proxy generation so clients can review and approve work without receiving full-resolution files they haven't paid for.
- **Billing & Financial Visibility:** Stripe-connected automatic invoicing tied to tracked time and project milestones, billable/non-billable time classification, budget burn alerts and project profitability snapshots.
- **Analytics & Audit:** Project health dashboards, utilization reports, client revenue tracking and an immutable audit trail covering all destructive or sensitive actions.

Key objectives and purposes of Mirama:

- **Visual-First, Not Visual-Only:** Every board and dashboard treats visual output as the primary object. The same platform works as a plain task tracker for teams that don't need the asset layer.
- **End-to-End Engagement Flow:** The platform covers the full client lifecycle - lead intake, project execution, client collaboration, billing - so operators run their business from one tool instead of five.
- **Asset Protection by Default:** Client previews are automatically watermarked and served at reduced quality. Full-resolution delivery happens as a deliberate action, not a default.
- **Freelancer-First Scaling:** Every feature makes sense for a single-person operation. Adding team members or clients doesn't require restructuring; the underlying model already supports it.
- **Scalability & Security:** Multi-tenant architecture with strict data isolation, PBAC authorization and audit trails that hold up as the team grows.

Mirama is intentionally iterative and adaptable, evolving alongside the needs of its users. It is both a production-ready tool for service professionals and a platform for ongoing technical exploration.

---

## Architecture Overview

Mirama is structured as two primary components: a **Next.js frontend** and a **MiramaService backend**. This two-component architecture reflects a deliberate choice to keep operations simple and maintainable while enforcing clean domain boundaries.

### Next.js Frontend

The frontend is a standalone Next.js application responsible for the user interface, server-side rendering and client-side state management. It communicates with the MiramaService backend over a well-defined API boundary.

Key characteristics:

- **Next.js App Router** for SSR, API route handling and edge middleware
- **React Query (TanStack)** for optimistic UI updates and server state synchronization
- **Prisma ORM** for database access
- **NextAuth** for authentication and session management
- **Permission Matrix** passed from the backend and stored in React Context for zero-round-trip authorization checks in the UI

### MiramaService Backend (Modular Monolith)

The backend is a **C#/.NET modular monolith**. Rather than splitting into independent microservices, the system enforces domain boundaries through module interfaces and internal service contracts within a single deployable unit.

This architecture was chosen deliberately:

- Mirama's core business logic is complex and frequently spans multiple domains (projects, tasks, organizations, billing, assets). In a microservices model this would require expensive cross-service coordination and distributed transactions.
- A modular monolith preserves clear separation of concerns - each module owns its data and exposes a defined interface - without the operational overhead of service meshes, independent deployments and network-based inter-service calls.
- The platform is intended to grow in business logic complexity, not in service count. A modular monolith scales that dimension well.

Key characteristics:

- **Clean Architecture** with vertical slice organization per feature
- **Domain-Driven Design (DDD)** within module boundaries
- **Inter-module communication** via direct in-process interfaces (no HTTP overhead between domains)
- **OpenIddict** as a self-hosted OIDC authority for centralized token issuance
- Deployed as a single containerized application

### Infrastructure

The platform runs on AWS infrastructure provisioned via Terraform (Infrastructure as Code):

| Resource      | Service              | Purpose                                          |
|---------------|----------------------|--------------------------------------------------|
| Orchestration | Terraform            | Infrastructure as Code (IaC)                     |
| Database      | Amazon RDS           | Managed PostgreSQL for persistent storage        |
| Caching       | Amazon ElastiCache   | Redis for high-speed data retrieval and sessions |
| Storage       | Amazon S3            | Asset storage and proxy/watermark delivery       |
| Networking    | AWS VPC              | Isolated network environment                     |
| Containers    | ECS + EC2 ASG        | Container orchestration and auto-scaling         |

> **Note:** The AWS infrastructure has been tested and validated but is not currently active due to cost overhead for a personal project. The Next.js frontend is currently hosted on Vercel and the MiramaService runs in a containerized environment.

---

## Architecture Philosophy

Mirama is guided by the principle that **architecture should serve the problem, not the ego**.  
Key ideas include:

- Start simple and evolve intentionally as needs grow
- Align complexity with real domain requirements - complex business logic warrants a structured backend, not more services
- Avoid premature abstractions and distributed systems overhead until the scale genuinely demands it
- Prioritize clarity, maintainability, and practical value

---

## Documentation Structure

- **Root:** High-level overview of Mirama, including project vision, system context, security design, and architecture decisions (ADRs).
- **Frontend:** Documentation of the Next.js application. Covers the BFF architecture, recursive task engine, React Query patterns, custom Redis caching extension and structured logging.
- **Infrastructure:** IaC strategy, AWS topology, networking and secrets management.
- **ADRs:** Architecture Decision Records capturing the key decisions, trade-offs and alternatives considered during platform design.
