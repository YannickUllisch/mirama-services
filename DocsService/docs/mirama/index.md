# Mirama Platform

## Project Name

**Mirama Platform**  
Domain: [mirama.yannickullisch.com]  
Unique Identifier: `mirama-platform`

---

## Project Description

Mirama is a Creative Operating System - a unified CRM, project management, and operational platform built specifically for how creative businesses actually work. The platform's mission is to eliminate the patchwork of disconnected tools that agencies, studios and freelancers rely on today: a separate task tracker, a file-sharing service, a client feedback tool, a time tracker and a spreadsheet for budgets. Mirama brings these into a single, coherent workspace without sacrificing the visual-first experience that creative work demands.

A key part of Mirama's development was the close collaboration with **Mirage.xyz**, a creative design studio based in London. By working directly with them, we were able to observe real creative processes, gather authentic requirements and solve genuine problems faced by creative professionals, instead of just addressing hypothetical developer assumptions. This partnership ensured that Mirama's features and design are grounded in real-world needs and deliver practical value to creative teams.

The platform is designed to serve three segments with meaningfully different needs:

- **Freelancers** - professional client intake forms, a built-in timer for billable work, and a clean client portal that makes solo operators look like a full agency.
- **Small Agencies** - project templates, workload visibility across the team, and contextual client collaboration without context-switching between tools.
- **Large Creative Firms** - capacity planning across multiple teams, audit logs for accountability, fine-grained access control, and budget burn reporting at scale.

The platform is structured to support:

- **Client & Intake Management (CRM):** A relational client object linking intake briefs, projects, time records and invoices from the start - the foundation that makes future billing and reporting possible.
- **Visual Project & Task Execution:** N-level task hierarchies, Kanban boards, Gantt timelines, task dependencies and custom statuses designed around creative production stages.
- **Asset Collaboration & Proofing:** Native annotation on images, PDFs and video; stacked version history; contextual task-level discussion - all without external tools.
- **Resource & Financial Visibility:** Built-in time tracking with billable/non-billable classification, budget burn alerts and a workload view showing who is overbooked or available.
- **Automation & Scale:** Project templates, automated workflow triggers and a global search engine spanning tasks, files and conversations.

Key objectives and purposes of Mirama include:

- **Creative-First Workflow:** Build around the visual asset, not the text-based ticket. Every view, board and dashboard treats visual output as the primary object.
- **Unified Client Relationship:** The client is a first-class data object - linked to briefs, projects, time entries and (eventually) invoices - not a tag or a folder name.
- **Scalability & Security:** Multi-tenant architecture with strict data isolation, PBAC authorization and audit trails for teams of any size.
- **ERP-Ready Data Model:** Time tracking, project financials and client relationships are modelled correctly from the start so that profitability reporting and invoicing are a natural extension, not a rebuild.

Mirama is intentionally iterative and adaptable, evolving alongside the needs of its users and the creative industry. It is both a production-ready tool for creative professionals and a platform for ongoing technical exploration and learning.

---

## Architecture Overview

Mirama is structured as two primary components: a **Next.js frontend** and a **MiramaService backend**. This two-component architecture reflects a deliberate choice to keep operations simple and maintainable while still enforcing clean domain boundaries.

### Next.js Frontend

The frontend is a standalone Next.js application responsible for the user interface, server-side rendering and client-side state management. It communicates with the MiramaService backend over a well-defined API boundary.

Key characteristics:

- **Next.js App Router** for SSR, API route handling and edge middleware
- **React Query (TanStack)** for optimistic UI updates and server state synchronization
- **Prisma ORM** for database access
- **AWS Cognito + NextAuth** for cloud-native authentication
- **Permission Matrix** passed from the backend and stored in React Context for zero-round-trip authorization checks in the UI

### MiramaService Backend (Modular Monolith)

The backend is a **C#/.NET modular monolith**. Rather than splitting into independent microservices, the system enforces domain boundaries through module interfaces and internal service contracts within a single deployable unit.

This architecture was chosen deliberately:

- Mirama's core business logic is complex and frequently spans multiple domains (projects, tasks, organizations, billing, assets). In a microservices model this would require expensive cross-service coordination and distributed transactions.
- A modular monolith preserves clear separation of concerns-each module owns its data and exposes a defined interface-without the operational overhead of service meshes, independent deployments and network-based inter-service calls.
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
| Identity      | AWS Cognito          | Managed User Pools and Identity Providers        |
| Networking    | AWS VPC              | Isolated network environment                     |
| Containers    | ECS + EC2 ASG        | Container orchestration and auto-scaling         |

> **Note:** The AWS infrastructure has been tested and validated but is not currently active due to cost overhead for a personal project. The Next.js frontend is currently hosted on Vercel and the MiramaService runs in a containerized environment.

---

## Architecture Philosophy

Mirama is guided by the principle that **architecture should serve the problem, not the ego**.  
Key ideas include:

- Start simple and evolve intentionally as needs grow
- Align complexity with real domain requirements-complex business logic warrants a structured backend, not more services
- Avoid premature abstractions and distributed systems overhead until the scale genuinely demands it
- Prioritize clarity, maintainability, and practical value

---

## Documentation Structure

- **Root:** High-level overview of Mirama, including project vision, requirements, domain model, security design, multi-tenancy model and future enhancements.
- **Frontend (Phase 1):** Documentation of the Next.js application. Covers the BFF architecture, recursive task engine, React Query patterns, custom Redis caching extension, Cognito integration and structured logging.
- **Infrastructure:** IaC strategy, AWS topology, networking and secrets management are covered in the root-level `iac-strategy.md`.
