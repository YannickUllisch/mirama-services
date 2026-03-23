# Mirama Platform

## Project Name

**Mirama Platform**  
Domain: [mirama.yannickullisch.com]  
Unique Identifier: `mirama-platform`

---

## Project Description

Mirama is a next-generation project and task management platform designed specifically for creative teams and organizations that work with visual assets at the core of their process. The platform’s primary goal is to centralize the management of projects, tasks and creative deliverables—such as images, mockups and design files, while enabling seamless collaboration between internal teams and external stakeholders.

A key part of Mirama’s development was the close collaboration with **Mirage.xyz**, a creative design studio based in London. By working directly with them, we were able to observe real creative processes, gather authentic requirements and solve genuine problems faced by creative professionals, instead of just addressing hypothetical developer assumptions. This partnership ensured that Mirama’s features and design are grounded in real-world needs and deliver practical value to creative teams.

Mirama stands out by making visual assets first-class citizens throughout the entire project lifecycle. It helps teams track the evolution of creative work, from early drafts to final approvals, all in a single, unified workspace. The platform is built to streamline feedback and approval cycles, reduce communication friction and ensure that everyone from designers, managers and clients, are always working from the same source of truth.

Over time, Mirama has grown into a broader platform and technical playground, used to experiment with system design, scalability and maintainability, while always keeping the user experience visual-first and collaborative.

The platform is structured to support:

- Centralized asset management and versioning
- Visual project and task tracking
- Integrated client feedback and approval
- Secure, scalable infrastructure for creative organizations

Key objectives and purposes of Mirama include:

- **Visual-First Workflow:** Centralize and organize all creative assets, making it easy to manage, review and approve work visually.
- **Collaboration Hub:** Integrate clients and collaborators directly into the workflow, eliminating scattered feedback and email threads.
- **Scalability & Security:** Support multi-tenant organizations, strict data isolation, and robust access control for teams of any size.
- **Iterative Evolution:** Serve as a technical playground for exploring modern system design, distributed architectures, and best practices, while remaining practical and usable for real-world creative teams.

Mirama is intentionally iterative and adaptable, evolving alongside the needs of its users and the creative industry. It is both a production-ready tool for creative professionals and a platform for ongoing technical exploration and learning.

---

## Project Evolution

Mirama’s development is intentionally phased, reflecting how real-world systems grow and adapt:

### Phase 1: Pragmatic, Visual-First Application

A standalone Next.js application focused on rapid delivery of core visual workflow features. This phase prioritized simplicity, speed and a user-friendly interface for managing creative assets, galleries, and feedback.

### Phase 1.5: Platform & Infrastructure (Terraform & AWS)

A critical infrastructure pivot, moving from managed PaaS (Vercel) to a custom AWS cloud environment. This phase established the foundation for future scalability, security and microservices, though the infrastructure is not currently active due to personal project cost constraints.

### Phase 2: Microservices as Technical Exploration

**Status: Far from complete (as of 23/03/2026).**  
Phase 2 is not a production requirement but serves as a technical exploration and learning phase for me, the developer. It investigates distributed systems, service boundaries and advanced security patterns, mirroring how microservices often emerge organically in maturing systems. The documentation for this phase is a work in progress and primarily targets technical audiences interested in architecture and system evolution.

---

## Architecture Philosophy

Mirama is guided by the principle that **architecture should serve the problem, not the ego**.  
Key ideas include:

- Start simple and evolve intentionally as needs grow
- Align complexity with real domain requirements
- Avoid premature abstractions
- Prioritize clarity, maintainability, and practical value

Different parts of the system use layered, clean or domain-driven architectures as appropriate, always aiming for the right tool for the job.

---

## Documentation Structure

- **Root:** High-level overview of Mirama, including project vision, requirements, future enhancements and the domain model.
- **Phase 1 & 1.5:** Documentation of the Next.js monolith and the supporting infrastructure. Includes core architecture, file handling, integration with S3, SNS, and SQS, as well as cloud setup, IaC (Terraform), AWS architecture and all foundational components that enable scalability and reliability.
- **Phase 2** Technical documentation for the C# .NET backend microservice structure, focusing on distributed system patterns, service boundaries, and advanced security.

The working version of Mirama is based on Phase 1, with some infrastructure elements from Phase 1.5.  
This entry point provides a high-level understanding of Mirama’s vision, goals and evolution. For detailed technical or implementation-specific information, refer to the respective sections in the documentation.
