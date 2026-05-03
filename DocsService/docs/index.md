---
title: Welcome
hide:
  - navigation
  - toc
---

## Welcome to My Projects & Documentation

Hi, I’m Yannick. 👋  

This is the central hub for the documentation of my personal and professional projects. Its purpose is to give you a clear view of the work I’ve done, the systems I’ve designed and the platforms I maintain, all in one place.

---

## Current Focus: The Mirama Platform

The primary project documented here is the **Mirama Platform**, a task and project management system designed for modern agile workflows. Mirama serves as a technical showcase of system evolution—moving from a rapid-growth monolith to a scalable, distributed architecture.

### The Evolution of Mirama

To understand the platform, you can explore its development through three distinct phases:

* **Phase 1: The Pragmatic Monolith (Next.js)** A self-contained full-stack application focused on high-performance UI and deep-tree recursive task logic. Features **React Query** for optimistic updates, **Prisma ORM**, and **Pino** for structured observability.
  
* **Phase 1.5: Platform & Infrastructure (Terraform & AWS)** The transition from PaaS (Vercel) to managed cloud infrastructure. This phase covers the **Infrastructure as Code (IaC)** used to provision **RDS (Postgres)**, **ElastiCache (Redis)** and **ECS & EC2 Auto-scaling Group**.

* **Phase 2: Microservices Evolution (C#/.NET)** The decoupling of the core logic into independent services. Explore the **MiramaService**, **AuthService** and **ProjectService**, built with Clean Architecture, Domain-Driven Design (DDD) and high-performance cross-service security.

---

## Technical Highlights

Inside the documentation, you will find deep dives into:

* **Recursive Task Engines:** Managing N-level hierarchical data in the UI and Database.
* **Distributed Caching:** Invalidation strategies using Redis and custom ORM extensions.
* **Identity Management:** Secure authentication flows integrated via AWS Cognito, as well as a self-hosted centralized OIDC server using **OpenIddict**.
* **DevOps & Observability:** Structured logging with Pino & Serilog (with plans of extending to OLTP Protocol) and automated provisioning with Terraform.

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
