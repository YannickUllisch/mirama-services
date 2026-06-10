# Architecture Decision Records

An **Architecture Decision Record (ADR)** is a short document that captures an important decision made about the architecture or design of the system - what was decided, why, and what the trade-offs are.

## Why we write ADRs

Code tells you *what* the system does. ADRs tell you *why* it was built that way. When someone joins the team, reads the codebase six months later, or proposes changing something, ADRs answer the question: "Was this a deliberate choice, or did it just end up that way?"

## ADR lifecycle

Each ADR has a status:

| Status | Meaning |
|---|---|
| **Proposed** | Under discussion, not yet accepted |
| **Accepted** | Decision is in effect |
| **Deprecated** | Was accepted, but no longer applies |
| **Superseded by ADR-XXX** | Replaced by a newer decision |

## How to write a new ADR

1. Copy the template below.
2. Name the file `ADR-NNN-short-description.md` (e.g., `ADR-002-api-versioning-strategy.md`).
3. Add it to `.pages` under `arrange`.

**Template:**

```markdown
---
title: "ADR-NNN: Short Decision Title"
date: "YYYY-MM-DD"
status: "Proposed"
---

## Context

What problem or situation forced this decision? What constraints or requirements shaped the options?

## Decision

What was decided? State it clearly and directly.

## Consequences

What becomes easier or harder as a result? Include both positive and negative outcomes.

## Alternatives Considered

What other options were evaluated, and why were they rejected?
```

## Records

| ADR | Title | Status |
|---|---|---|
| [ADR-001](recursive-task-design.md) | Hierarchical Task Model with DFS Subtask Traversal | Accepted |
