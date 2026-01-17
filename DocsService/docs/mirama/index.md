# Mirama Platform

## Overview

Mirama is a task and project management platform I originally built for a small design startup, namely **Mirage.xyz**. The initial goal was simple, give them a system that fits how they actually work, instead of forcing them into large, generalized tools like Monday or Asana. Those platforms are great, but serving a wide audience often comes at the cost of focus and flexibility.

Over time, Mirama naturally became a technical playground. Since I didn’t have the time to fully explore advanced ideas during the original project timeline, I continued developing it on my own, using it to experiment with system design, trade-offs, and maintainability. It’s a space to try out new technologies, frameworks and languages, and to explore advanced concepts like distributed systems, event driven microservices and complex authentication flows, all while keeping the project practical and usable.

Mirama was never meant to be perfect from day one. It’s intentionally iterative, evolving just like real world systems. Starting simple, accumulating requirements and gradually adding clearer boundaries, stronger security and better scalability.

The main documentation for the system can be found under the Backend section, which includes the general architecture document. The Frontend is also documented, as it has its own API and acts as a simpler, cost-effective client-server model. While less scalable and fine-tunable on its own, it’s still used as a backend-for-frontend (BFF), providing a central API layer and avoiding direct communication between the frontend and microservices. This setup keeps the system organized, efficient and more easy to handle.

---

## Project Evolution

As mentioned above Mirama has been developed in distinct phases, each representing a different stage of system maturity. These phases intentionally coexist, as they demonstrate how architectural needs change as a system grows and as new goals emerge.

---

## Phase 1: Pragmatic, Self-Contained Application

The first version of Mirama was built as a **single, standalone Next.js application**, where frontend and backend logic lived side by side. This approach prioritized speed, simplicity and development efficiency, perfect for an early-stage product like I was trying to create.

This phase was mainly about **learning and experimenting with Next.js**, which I was using frequently at the time. While at the same time getting a modern looking and highly performant system. It allowed me to quickly get up to speed on advanced concepts like server-side rendering (SSR) and partial pre-rendering (PPR), component streaming as well as essential skills such as routing, component structure and data fetching + optimistic cache updates. All while exploring UI design, API integration, and full-stack development.

The main focus points of in terms of development for this stage were:

- Clear separation of UI and API concerns  
- User first / User friendly UI/UX Design
- Proper input validation and type safety in APIs with Zod  
- Integrated secure authentication and authorization flows  
- Practical database modeling with an ORM  
- Deployment choices balancing simplicity and cost  

All APIs, authentication flows, and UI logic were implemented directly in the frontend project, making it **fully self-contained and easy to deploy**.

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
