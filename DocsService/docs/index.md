---
title: Welcome
hide:
  - navigation
  - toc
---

## Welcome to My Projects & Documentation

Hi, I'm Yannick. 👋  

This is the central hub for the documentation of my personal and professional projects. Its purpose is to give you a clear view of the work I've done, the systems I've designed and the platforms I maintain, all in one place.

---

## Current Focus: The Mirama Platform

The primary project documented here is the **Mirama Platform** - a Creative Operating System designed for the specific chaos of creative work. Where generic project management tools fall short for designers, studios and agencies, Mirama is built from the ground up around visual assets, client collaboration, and the financial accountability creative businesses actually need.

The platform is designed to grow from a CRM and project management foundation into a unified workspace that covers the full operational lifecycle of a creative business - from client intake and brief creation, through production and approval, to time-based billing and resource planning. A lean ERP layer sits on the horizon as the long-term direction, built on the same relational data model from day one.

The initial target is three segments of the creative market, each with distinct needs the platform must serve:

* **Freelancers** - professional intake forms, time tracking and client portals that make solo operators look like agencies.
* **Small Agencies** - project templates, workload visibility and client collaboration without the overhead of stitching together five different tools.
* **Large Creative Firms** - capacity planning, audit trails, fine-grained access control and budget burn reporting at scale.

### Current Architecture

Mirama is built as two primary components:

* **Next.js Frontend** - A full-stack React application handling the UI, server-side rendering and client-side state. Uses React Query for optimistic updates, Prisma ORM, AWS Cognito for authentication and Pino for structured observability.

* **MiramaService Backend (Modular Monolith)** - A C#/.NET backend built as a modular monolith. Domain boundaries are enforced through module interfaces and internal service contracts rather than network calls, making it well-suited for Mirama's complex, interconnected business logic. Uses Clean Architecture, Domain-Driven Design and OpenIddict as a self-hosted OIDC authority.

The platform runs on AWS infrastructure provisioned with Terraform, using RDS (PostgreSQL), ElastiCache (Redis) and ECS for container orchestration.

---

## Technical Highlights

Inside the documentation, you will find deep dives into:

* **Recursive Task Engines:** Managing N-level hierarchical data in the UI and database.
* **Native Proofing & Asset Versioning:** Click-on-spot annotation for images, PDFs and video; stacked version history so teams never lose a revision.
* **CRM & Intake Foundation:** Relational client object linked to briefs, projects and time records - the data model that makes a future billing and ERP layer possible.
* **Security & Authorization:** Policy-Based Access Control (PBAC), multi-tenant data isolation and a self-hosted OIDC server using OpenIddict.
* **Modular Monolith Design:** Clean Architecture with vertical slices and enforced module boundaries without the overhead of microservices.
* **DevOps & Observability:** Structured logging with Pino & Serilog and automated provisioning with Terraform.

---

## Future Documentation

While the Mirama Platform is the primary project documented now, the goal is to expand this hub to include:

* New personal platforms as they emerge.
* Open-source projects
* Research or experimental prototypes

The vision is to provide a **comprehensive view of my technical work**, from code structure to system architecture, in an accessible and navigable format.

---

## How to Use This Site

* Use the **sidebar navigation** to explore each project and its submodules.
* Each page includes explanations, architecture diagrams and when relevant some code snippets to make it easier to understand the structure and functionality.

---

Thank you for visiting. I hope this gives you a clear picture of my work! :)
