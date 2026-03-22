# System Requirements & Design Constraints

This document outlines the functional and non-functional requirements for the Mirama Platform. It serves as the "North Star" for our architectural decisions, ensuring that as the system evolves from a monolith to a distributed network of services, we stay true to the original goal: **practicality over complexity.**

---

The Mirama Platform is engineered to provide a seamless, high-performance project management experience. The following core functionalities define the system:

### 1.1 Multi-Tenant Identity & Workspace Hierarchy
The system manages complex relational boundaries between **Tenants, Organizations, and Teams**.
* **Identity Management:** Users can securely authenticate, manage personal profiles (including localized preferences like `preferredDateType`), and navigate between multiple Organizations. 
* **Context Switching:** Users may belong to multiple Tenants or Organizations, with the system providing fluid context-switching while maintaining strict data isolation.
* **RBAC (Role-Based Access Control):** Fine-grained permissions are enforced at every level (Owner, Admin, User, Freelance, Observer), ensuring users interact only with the data pertinent to their specific role. Furthermore, we dicern between Read and Write Permissions for each Microservice, for a simple but effective security system.

### 1.2 Core Project & Task Lifecycle
Mirama provides a robust engine for work management, supporting the full CRUD lifecycle of projects and tasks.
* **Complex Task Modeling:** Supports diverse task types (**Epics, Features, Stories, Tasks, Issues**) with recursive sub-tasking for deep hierarchy.
* **Project Milestones & Financials:** Includes **Milestone tracking** for project roadmapping and a comprehensive **Expense Management** system (Labor, Materials, Licensing, etc.) to track project budgets vs. actuals.
* **Collaboration & Favorites:** Integrated threaded commenting with reply support and an "Activity Feed" for transparency. Users can "Favourite" specific Projects, Tasks, or Routes for rapid navigation.

### 1.3 Distributed System Architecture & Orchestration
The backend is a suite of **independent microservices** (Account, Project, Auth, Expense) coordinated by a communication layer.
* **BFF (Backend-for-Frontend):** A centralized API layer acts as a mediator, orchestrating complex data aggregation from downstream microservices into a unified interface.
* **Event-Driven Integration:** We utilize asynchronous messaging SNS+SQS to propagate state changes across service boundaries, ensuring system-wide **eventual consistency**.
* **API Integrity:** All services support versioned REST and high-performance gRPC endpoints, utilizing Protobuf definitions for strict type safety in service-to-service communication.

### 1.4 Real-Time Collaboration & Communication
To eliminate stale data in the browser, Mirama facilitates instant feedback loops between team members.
* **Granular Notification System:** Users receive real-time notifications for relevant events (e.g., task assignments, mentions in comments, or deadline shifts). Notifications are state-managed (Read/Unread) and linked to specific resources.
* **Presence & Activity Feeds:** A system-wide activity log captures high-level changes (e.g., project status updates), while granular task-level "Audit Logs" track individual field changes for accountability.
* **Live Updates:** Leveraging WebSockets or Server-Sent Events (SSE) via the BFF layer to ensure that when a project manager moves a task to "Done," the team sees the change instantly without a manual refresh.

### 1.5 Advanced Search & Discovery
As data grows from dozens to thousands of entities, Mirama ensures information remains discoverable.
* **Fast, Indexed Search:** High-performance queries across Projects, Tasks, and Tags, utilizing database indexing (and future-proofing for elastic search) to allow for "type-ahead" discovery.
* **Hierarchical Filtering:** The ability to filter views by complex criteria, such as "Tasks assigned to Freelancers in Project X with High Priority."
* **Personalized Shortcuts:** The "Favourites" system allows users to pin specific Routes or Entities (like a high-priority Epic) to their sidebar for rapid access.

### 1.6 Resource & Asset Management
Project management often requires more than just text; it requires documentation and financial oversight.
* **File & Attachment Support:** Secure association of files with Projects and Tasks. The architecture supports secure storage via AWS S3 with access controls that respect the Tenant/Organization boundary.
* **Expense Tracking & Budgeting:** A dedicated domain for tracking project costs. This allows for the categorization of expenses (Labor, Materials, Software) and simple "Budget vs. Actual" reporting at the Project level.
* **Milestone Tracking:** A roadmap-level feature allowing users to set critical dates and visualize progress toward major delivery goals.

### 1.7 Developer-Centric Integration & Extensibility
Mirama is built to be a first-class citizen in a modern tech stack.
* **Integration-Ready Webhooks:** The system is designed to trigger external webhooks for key events, allowing integration with tools like Slack, Discord, or custom automation scripts.
* **API Versioning:** All backend endpoints are versioned (e.g., `/api/v1/`) to ensure the Frontend or third-party integrators do not break when service logic is updated.
* **Self-Service Administration:** Admins can manage the organization’s lifecycle, including inviting users via secure tokens and managing team roles.

---

## 2. Non-Functional Requirements (Quality Attributes)

These attributes define how the system should perform and evolve. We prioritize these to ensure the system is maintainable by a single developer while remaining "production-ready."

### 2.1 Performance & Edge Optimization
Mirama is built for "instant" feel. We leverage **Next.js with SSR, PPR, and Component Streaming** to reduce time-to-first-byte. 
* **Edge-Ready:** API routes are optimized for edge runtimes to provide ultra-low latency globally.
* **Optimistic UI:** The frontend assumes success for task updates, providing immediate visual feedback while syncing in the background.

### 2.2 Availability & The CAP Theorem
In the context of the **CAP Theorem**, Mirama prioritizes **Availability over Consistency** ($AP$ over $CP$). 

> **Design Decision:** It is functionally superior for a user to interact with a "stale" task status than to experience a system outage. We favor a highly available, responsive UI where data converges eventually via background event-bus synchronization.

### 2.3 Security: Stateless & Scalable
Security is baked into the network architecture rather than being an afterthought.
* **Stateless JWTs:** We use custom, stateless JSON Web Tokens for highly efficient authentication. This removes the need for services to "call home" to the Auth service for every request, significantly reducing latency and improving horizontal scalability.
* **Multi-Provider Auth:** Support for multiple authentication strategies (Custom JWT, OAuth2, or external providers like Cognito) to allow for gradual system migration.
* **Encryption:** All data is encrypted in transit (TLS) and sensitive identity data is hashed at rest. 

### 2.4 Reliability & Maintainability
To ensure the "Playground" doesn't become a "Graveyard," we enforce high engineering standards:
* **Testing Strategy:** A multi-layered testing suite including unit tests for domain logic, integration tests for service boundaries, and E2E tests for critical user flows.
* **Standards:** We apply **Clean Architecture** and **Vertical Slices** to keep features isolated and testable.
* **Automated CI/CD:** Continuous Integration and Deployment pipelines ensure that every change is validated and deployed containerized via Docker for environment parity.

---

## 4. Summary of Design Principles

For Mirama, we live with **eventual consistency** to gain **high availability**. We live with the **overhead of microservices** to gain **clear domain boundaries**. We avoid **premature abstraction** until the domain logic actually demands it.
