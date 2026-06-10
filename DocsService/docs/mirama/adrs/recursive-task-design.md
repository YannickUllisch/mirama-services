---
title: "ADR-001: Hierarchical Task Model with DFS Subtask Traversal"
date: "2025-03-17"
status: "Accepted"
---

## Context

The platform needs a task management system that supports both high-level planning (epics, features, stories) and granular execution (issues, tests, subtasks). The key challenges were:

- How to enforce a clear hierarchy without allowing unbounded nesting.
- How to efficiently traverse task trees when propagating updates or rendering views.

## Decision

Tasks are split into two categories with explicit rules about nesting:

**Group tasks** (epics, features, stories) act as top-level containers. They cannot have a parent task.

**Individual tasks** (issues, tests) can be nested as subtasks of other individual tasks or assigned under a group task.

When traversing task trees (for updates, validation, or rendering), we use **depth-first search (DFS)**. Excessively deep subtask trees are discouraged by convention and may be enforced by a depth limit in the future.

## Consequences

**Positive:**

- The hierarchy is predictable: group tasks are always roots, individual tasks are always leaves or intermediate nodes. This simplifies both UI rendering and data queries.
- DFS processes one branch at a time, meaning deep changes resolve quickly without waiting on breadth-wide operations.
- The flat upper layer (group tasks with no parents) reduces complexity for recursive operations and lowers the risk of unintended cascading updates.

**Negative:**

- Some workflows may want a hierarchy of group tasks (e.g., an epic containing sub-epics). This model does not support that without a schema change.
- DFS can hit stack depth limits if subtask trees grow very deep. This is mitigated by discouraging deep nesting, but it is not enforced structurally today.

## Alternatives Considered

**Allow group tasks to have parents.** This would enable hierarchies like epic > sub-epic > feature. Rejected because it adds significant complexity to the data model and UI, and the common collaborative workflows we support do not require it.

**Use breadth-first search (BFS) for tree traversal.** BFS processes all nodes at a given depth before going deeper. Rejected because it delays resolution of deep changes and offers no advantage for our update propagation patterns.

**Hybrid DFS/BFS model.** Some systems switch strategies based on tree shape. Rejected because the added complexity was not justified by the use cases we currently support.
