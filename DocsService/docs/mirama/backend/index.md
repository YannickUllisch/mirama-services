# Overview

## Backend Services & Architectural Choices

All backend services introduced in this phase are built with **ASP.NET** and containerized using **Docker**. Each service is intentionally designed with a different architectural style based on its responsibility, expected growth, and rate of change.

### Authorization Service

The authorization service acts as a centralized security authority, responsible for authentication, token issuance, and access control.

It was introduced to gain full control over:

- JWT structure and claims  
- Scopes and audiences  
- Security boundaries between services  

A **layered architecture** was chosen deliberately here. Authentication is a well-defined domain that typically doesn’t grow explosively, making a simpler and more predictable structure the right trade-off.

### Account Service

The account service handles identity and organizational concepts such as users, tenants, organizations, and teams—areas that tend to grow and accumulate business rules over time.

To support that evolution, it uses:

- **Clean Architecture**
- A **vertical slice approach**

This keeps features isolated, testable, and easier to reason about as complexity increases.

### Project & Task Management Service

The project and task service represents the core business domain of Mirama and is expected to become the most complex part of the system.

It follows the same clean architecture and vertical slice principles as the account service, preparing it for:

- Feature expansion  
- Complex workflows  
- Domain-driven design considerations  

Each service is documented independently to reflect real-world service ownership and autonomy.
