# Phase 1: The Monolith (Next.js)

Welcome to the documentation for the first architectural iteration of the Mirama Platform. This phase represents the foundation of the system, built as a high-performance, standalone Next.js application designed to balance rapid feature delivery with high engineering standards.

---

## Overview

In this phase, Mirama was developed as a **Backend-for-Frontend (BFF)** architecture. While the UI and API logic coexist within the same repository, they are strictly decoupled to allow for the eventual migration to microservices. The focus here was on creating a *snappy* user experience for hierarchical data while maintaining rigorous type safety and good observability.

---

## Core Documentation Modules

### Architecture & Strategy

- **The Clean Monolith:**  
  An exploration of how the Next.js App Router is utilized to handle server-side rendering (SSR) and API route handling in a unified entry point.

---

### Data Persistence & Caching

- **Prisma ORM & Schema Design:**  
  How we model entire Database via the Prisma ORM as well as design decisions behind the Data model and Schema design.

- **Custom Redis Extension:**  
  A technical deep-dive into the `createPrismaRedisCache` logic used to intercept Prisma queries for sub-millisecond read performance.

- **Cache Invalidation:**  
  Analysis of the automated purge logic that ensures data consistency across related models during mutations.

---

### The Recursive Task Engine

- **Recursive React Components:**  
  How the frontend renders deeply nested task structures efficiently without performance bottlenecks.

- **Optimistic UI with React Query:**  
  Implementation details of the "mutate-and-rollback" pattern that keeps the agile board feeling instantaneous.

- **End-to-End Type Safety:**  
  Using Zod to bridge the gap between frontend form validation and backend database constraints.

---

### Security & Identity

- **AWS Cognito Integration:**  
  The implementation of secure, cloud-native identity management for user pools and OIDC flows.

- **Authentication with Next-Auth**: tilizing Next-Auth as the integration layer between the Next.js frontend and Cognito, handling session callbacks and JWT augmentation.

- **JWT & Middleware:**  
  How Next.js Middleware is used to verify tokens and protect sensitive routes at the edge before they hit the application logic.

---

### Observability & Logging

- **Structured Logging with Pino:**  
  Moving beyond `console.log` to a machine-readable JSON format.

- **Cloud-Native Formatting:**  
  Why we map log levels to severity keys to ensure 100% compatibility with AWS CloudWatch and ELK stacks.

- **Contextual Metadata:**  
  Using the `mirama_app` base tag to maintain traceability during the transition to a multi-service architecture.

---

## Key Technologies

| Category           | Technology                       |
|--------------------|----------------------------------|
| Framework          | Next.js (App Router)             |
| Database           | PostgreSQL via Prisma ORM        |
| Caching            | Redis (Custom Prisma Extension)  |
| State Management   | React Query (TanStack)           |
| Auth               | AWS Cognito, Next-Auth           |
| Logging            | Pino                             |
