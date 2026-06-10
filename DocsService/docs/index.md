---
title: Welcome
hide:
  - navigation
  - toc
---

## Welcome to My Projects & Documentation

Hi, I'm Yannick. 👋  

This is the central hub for documentation of my personal and professional projects. Its purpose is to give you a clear view of the systems I've designed and the platforms I maintain, all in one place.

---

## Current Focus: The Mirama Platform

The primary project documented here is the **Mirama Platform** - an end-to-end client and project operating system built for freelancers and service-based teams.

Where generic project management tools treat work as a collection of text tickets, Mirama is built around the full lifecycle of a client engagement: bringing in a lead, scoping and executing the work, collaborating with the client on visual deliverables and issuing the invoice - all without switching tools.

The niche is intentionally focused. Mirama is built for service businesses that deliver visual output to clients on a project-by-project basis. That covers a wide range of industries operating in this space: design and brand studios, web development agencies (where the deliverable is a website or web app), video and film production companies, marketing and campaign agencies, architecture and interior design firms, and motion or animation studios. The common thread is client work with visual deliverables, a project has a start, a set of things to produce, a client who needs to review and approve them, and an invoice at the end.

The platform is designed to make immediate sense for a solo freelancer and scale to a mid-size agency of 100+ people without requiring a migration, a plan change, or a different tool. The underlying data model and access control work the same at every size.

The platform covers four interconnected pillars:

* **Client & CRM** - intake forms, lead tracking, client portals and a relational client object that stays linked to every project and invoice from day one.
* **Project & Task Management** - visual task boards, Gantt timelines, task dependencies and custom production stages built for how service work actually flows.
* **Asset Collaboration & Proofing** - native annotation on images, PDFs and video, stacked version history, watermarked low-quality client previews that protect the work until delivery.
* **Billing & Financial Visibility** - Stripe-connected invoicing and automatic billing, time tracking, budget burn alerts and project profitability snapshots.

### Current Architecture

Mirama is built as two primary components:

* **Next.js Frontend** - A full-stack React application handling the UI, server-side rendering and client-side state. Uses React Query for optimistic updates, NextAuth for authentication and a Redis-backed permission matrix for zero-round-trip authorization checks.

* **MiramaService Backend (Modular Monolith)** - A C#/.NET backend built as a modular monolith. Domain boundaries are enforced through module interfaces and internal service contracts, making it well-suited for Mirama's interconnected business logic across CRM, project management and billing. Uses Clean Architecture, Vertical Slices and Domain-Driven Design.

The platform is designed to run on AWS infrastructure provisioned with Terraform, using RDS (PostgreSQL), ElastiCache (Redis) and ECS for container orchestration. The frontend currently runs on Vercel and the backend on a containerized VM while the platform is in development.

---

## Technical Highlights

Inside the documentation, you will find deep dives into:

* **Native Proofing & Asset Versioning:** Click-on-spot annotation for images, PDFs and video; stacked version history; automatic watermarking and low-quality proxy generation for client previews.
* **Client & CRM Foundation:** Relational client object linked to briefs, projects, time records and invoices - the data model that makes automatic billing possible without retrofitting.
* **Stripe Billing Integration:** Automatic invoice generation and payment collection connected to tracked time and project milestones.
* **Security & Authorization:** Policy-Based Access Control (PBAC), multi-tenant data isolation and OAuth2-based authorization flows with JWTs.
* **Modular Monolith Design:** Clean Architecture with vertical slices and enforced module boundaries without the overhead of microservices.
* **Analytics & Audit:** Project health dashboards, budget burn tracking, utilization views and an immutable audit trail for accountability.
* **Architecture Decision Records:** Explanations of key choices made in the development and planning phase.

---

## Future Documentation

While the Mirama Platform is the primary project documented now, the goal is to expand this hub to include:

* New personal platforms as they emerge.
* Open-source projects.
* Research or experimental prototypes.

---

## How to Use This Site

* Use the **sidebar navigation** to explore each project and its submodules.
* Each page includes explanations, architecture diagrams and where relevant, code snippets to make the structure and functionality easier to understand.

---

Thank you for visiting. I hope this gives you a clear picture of my work! :)
