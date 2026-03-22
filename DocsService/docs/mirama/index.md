# Mirama Platform

## Overview

Mirama is a project and task management tool built for creative companies that rely heavily on visual assets. I originally built it for a small design startup, **Mirage.xyz**, because generic tools like Monday or Asana didn't fit their image-centric workflow. Mirama stands out by focusing on centralizing images, figures, and mockups at every stage of a project. It allows teams to track the visual evolution of their work, from early mockups to final designs, all in one place. Furthermore, Mirama aims at integrating clients directly into the workflow, creating a single, centralized hub for communication and feedback. This eliminates scattered email threads and ensures everyone is working from the same page. By enabling quick feedback and approval cycles, including features like quick annotation and approval, Mirama streamlines the client review process, reduces friction and helps creative teams deliver high-quality work faster.

Over time, Mirama naturally became a technical playground. Since I didn’t have the time to fully explore advanced ideas during the original project timeline, I continued developing it on my own, using it to experiment with system design, trade-offs, and maintainability. It’s a space to try out new technologies, frameworks and languages, and to explore advanced concepts like distributed systems, event driven microservices and complex authentication flows, all while keeping the project practical and usable, and true to its visual-first identity.

Mirama was never meant to be perfect from day one. It’s intentionally iterative, evolving just like real world systems. Starting simple, accumulating requirements and gradually adding clearer boundaries, stronger security and better scalability.

The main documentation for the system can be found under the Backend section, which includes the general architecture document. The Frontend is also documented, as it has its own API and acts as a simpler, cost-effective client-server model. While less scalable and fine-tunable on its own, it’s still used as a backend-for-frontend (BFF), providing a central API layer and avoiding direct communication between the frontend and microservices. This setup keeps the system organized, efficient and more easy to handle.

---

## Project Evolution

As mentioned above Mirama has been developed in distinct phases, each representing a different stage of system maturity. These phases intentionally coexist, as they demonstrate how architectural needs change as a system grows and as new goals emerge.

---

## Phase 1: Pragmatic, Visual-First Application

The first version of Mirama was built as a **single, standalone Next.js application** to rapidly deliver a tool that addressed the core visual workflow needs of a creative team. With frontend and backend logic living side by side, this approach prioritized speed and simplicity, allowing for the quick implementation of an early-stage product focused on managing creative assets.

This phase was about proving the "visual-first" concept while **learning and experimenting with Next.js**. The goal was to build a modern, highly performant system that could handle image uploads, display visual galleries and support a basic review process. It allowed me to get up to speed on advanced concepts like server-side rendering (SSR) for fast initial page loads, critical for image-heavy galleries, component streaming for a better user experience and optimistic cache updates for snappy UI interactions when managing  assets.

The main development focus was on building a user-friendly UI/UX centered around the creative process:

-   A clear, intuitive interface for uploading and organizing mockups, figures, and other visual assets.
-   Galleries and visual timelines to track the evolution of a design.
-   Proper input validation with Zod to ensure data integrity for project metadata.
-   Integrated, secure authentication to protect client projects.
-   Practical database modeling with an ORM to link assets to tasks and projects.
-   Simple, cost-effective deployment on a PaaS like Vercel.

All APIs, authentication flows, and UI logic were implemented directly in the frontend project, making it **fully self-contained and easy to deploy** as a first step in solving the asset fragmentation problem for creative teams.

---

# Phase 1.5: Platform & Infrastructure (Terraform & AWS)

This phase documents the critical **"Infrastructure Pivot"** where the Mirama Platform moved from a managed PaaS (Vercel) to a custom-architected cloud environment on AWS. This transition was the prerequisite for scaling into a microservices architecture, ensuring that the data layer and networking were robust, repeatable, and version-controlled.

- NOTE: While the Infrastructure has been tested to work, it is currently not active due to the high cost overheads for a personal project. Which is the reason behind the Next.js standalone still being hosted on Vercel at the moment.  

---

## Phase 2: Microservices as a Real-World Evolution

The move to microservices was **not driven by immediate necessity**, but by choice.  

The main goal of this phase was to **emulate how systems evolve in real world contexts**, where a simple application grows in complexity, team size and security requirements over time. It gave me the chance to explore distributed systems, service boundaries and scalability patterns in a practical way.

Rather than jumping straight into heavy infrastructure or abstract designs, the focus was on:

- Understanding *when* and *why* services should be split  
- Exploring different architectural patterns and trade-offs  
- Designing systems to handle growth efficiently without overcomplicating things
- Keeping operational complexity and costs as manageable as possible, even while deploying multiple independent services.
- Performance and security experiments, including stateless JWTs and OAuth flows for high-performance communication across services

The idea behind this is that it mirrors how microservices often emerge organically in production systems, rather than being a starting point.
As part of this, I’ve writte up a simple architecture document, which is still a work in progress as the system continues to evolve in my free time. The documentation is designed to be clear and extensible, accessible to less technical stakeholders while also providing in-depth technical details for each service, geared more toward developers.

---

## Architecture Philosophy

At its core, Mirama is guided by a principle I strongly believe in as a developer,
**architecture should serve the problem, not the ego.**

In practice, that means making decisions that actually help the system work well, rather than showing off complexity. Some key ideas that shaped Mirama include:

- Start simple and evolve intentionally as needs grow
- Align architectural complexity with the actual complexity of the domain
- Avoid premature abstractions that don’t add real value
- Favor clarity and maintainability over theoretical best practices

Some services follow layered architecture, others are based upon clean architecture and domain-driven design practices. Not because one is inherently better, but because I believe that each approach fits the problem it’s solving. The goal is always to choose the right tool for the job, keeping the system understandable, flexible and practical.
