# Phase 1.5: Platform & Infrastructure (Terraform & AWS)

This phase documents the critical **"Infrastructure Pivot"** where the Mirama Platform moved from a managed PaaS (Vercel) to a custom-architected cloud environment on AWS. This transition was the prerequisite for scaling into a microservices architecture, ensuring that the data layer and networking were robust, repeatable, and version-controlled.

- NOTE: While the Infrastructure has been tested to work, it is currently not active due to the high cost overheads for a personal project. Which is the reason behind the Next.js standalone still being hosted on Vercel at the moment.  

---

## Overview

The primary goal of Phase 1.5 was to implement **Infrastructure as Code (IaC)**. By using Terraform, manual "click-ops" of cloud management were replaced with a declarative setup. This allowed the existing Next.js application to communicate with a high-performance RDS and ElastiCache layer, while establishing the networking foundation (VPC) for both the standalone and also for the future C# microservices.

---

## Core Documentation Modules

### Infrastructure as Code Strategy

- **Why Terraform?**  
  An explanation of why HashiCorp Terraform was chosen for resource orchestration, focusing on provider flexibility and state management.

- **Module Structure:**  
  How the infrastructure is broken down into reusable components (Networking, Database, Cache, Identity).

---

### AWS Topology & Phase 1 Architecture

- **The Network Blueprint:**  
  Documentation of the VPC (Virtual Private Cloud), including public/private subnet isolation for security.

- **Component Mapping:**  
  A visual and technical breakdown of how the Next.js app is deployed in ECS in combination with an EC2 Autoscaling Gorup and interacts with Amazon RDS (PostgreSQL) and Amazon ElastiCache (Redis).

---

### Security Design in the Cloud

- **IAM & Least Privilege:**  
  How Identity and Access Management (IAM) roles are used to restrict service permissions.

- **Network Security:**  
  Implementation of Security Groups and NACLs to ensure the database and cache are only accessible by the application layer.

- **Secrets Management:**  
  How sensitive credentials are injected into the environment without being hardcoded. As well as future extensions.

---

### Data Migration Path

- **The "Zero-Downtime" Goal:**  
  The strategy used to migrate the production database from the initial Phase 1 setup to AWS RDS.

- **Schema Alignment:**  
  Ensuring the Prisma-generated schema remained consistent during the transition.

---

### CI/CD & Automation

- **Automated Provisioning:**  
  How GitHub Actions are used to trigger Terraform plans and applies for best practice deployments to production Infrastructure.

---

## Infrastructure Stack

| Resource       | Service            | Purpose                                             |
|----------------|--------------------|-----------------------------------------------------|
| Orchestration  | Terraform          | Infrastructure as Code (IaC)                        |
| Database       | Amazon RDS         | Managed PostgreSQL for persistent storage           |
| Caching        | Amazon ElastiCache | Redis cluster for high-speed data retrieval         |
| Identity       | AWS Cognito        | Managed User Pools and Identity Providers           |
| Networking     | AWS VPC            | Isolated network environment                        |
