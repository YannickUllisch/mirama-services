# AI Platform Intelligence

> **Status: Planned - Post-Core**
>
> The AI capabilities described in this section are not yet built. They are planned for a dedicated phase of development after the core platform (CRM, project execution, asset proofing, billing) reaches production readiness. This document captures the intended design so that AI integration is treated as a first-class concern during core development rather than a retrofit.

The platform is deliberately architected to support GenAI integration from the start. The data model, module boundaries, API surface, and event system are all designed with the assumption that AI capabilities will be added later - meaning no structural rework will be required when that phase begins. Intake forms produce structured events, assets flow through a processing pipeline, annotations are first-class entities, and project activity is continuously logged. Each of these is a natural integration point for an LLM or vision model.

When built, the AI layer will embed capabilities directly into the workflows where they create leverage. Rather than a generic chat interface bolted onto the side, each planned feature maps to a specific point of friction in the creative agency lifecycle - intake, asset management, client feedback, and project risk - and will augment the operator's existing workflow without replacing their judgment.

The planned AI layer will be built on large language models (LLMs) for text understanding and generation, multimodal vision models for asset analysis, and lightweight predictive models trained on project activity signals. All AI features will be scoped to the active organization and respect the same PBAC permission boundaries as the rest of the platform.

---

## Core Capabilities

### Brief Intelligence

Client intake forms often arrive as freeform text - a few paragraphs describing a campaign, a brand refresh, or a new website. Brief Intelligence will use an LLM to parse that text at the point of intake and extract structured data: deliverable types, timeline signals, budget indicators, and scope complexity markers.

The extracted data will pre-populate the project creation form and surface the most relevant project template from the organization's library. A brand studio that receives a "full identity system" brief will automatically see their *Brand Identity* template suggested, with logo design, brand guidelines, and asset export tasks already scaffolded.

This connects directly to the Lead-to-Project conversion flow (FR-1.114) and project templates (FR-1.121), removing the manual interpretation step between a client submitting a brief and a project being ready to execute.

**Planned behaviors:**

- Parse intake form submissions and brief attachments (text, PDF)
- Extract deliverable types, estimated scope, timeline expectations, and budget signals
- Suggest the top matching project template from the organization's library
- Pre-populate project title, description, milestones and estimated duration
- Surface confidence scores where extraction was uncertain, prompting operator review

---

### Vision-Based Asset Tagging

Asset libraries grow fast. Within months of onboarding, a mid-size agency may have thousands of uploaded files across dozens of projects. File names are unreliable, folders are inconsistent, and global search against filenames alone misses the majority of relevant assets.

Vision AI will process image and PDF assets at upload time using a multimodal model to generate rich descriptive tags: visual content, dominant colors, detected objects and subjects, style characteristics, and document type. These tags will be indexed alongside file metadata and surfaced through global search (FR-1.123), making assets retrievable by what they contain rather than what they were named.

A search for "dark blue logo variation" or "lifestyle photography outdoor" would return relevant assets across all projects the user has access to - scoped to their organization and permissions.

**Planned behaviors:**

- Run automatically on image (JPG, PNG, WebP, PSD preview) and PDF uploads
- Generate tags for: content description, dominant palette, style/mood, document type, detected subjects
- Index tags and expose them via the global search interface
- Allow users to add, edit, or remove AI-generated tags from any asset version
- Process asynchronously and non-blocking; assets remain accessible immediately after upload

---

### Annotation Feedback Summarization

A typical client review session on a complex asset - a brand campaign hero image, a homepage mockup, a product video - produces eight to fifteen scattered point annotations. Each pin contains a short comment, sometimes vague ("make this pop more"), sometimes contradictory across reviewers, sometimes referencing elements from a previous version.

The production team reads through all pins, interprets them, and manually reconstructs a revision list. This takes time and frequently loses context.

Annotation Feedback Summarization will aggregate all point annotations on an asset (or across all versions of a task) and use an LLM to produce a structured revision note list grouped by theme. Common themes in creative work - typography, color, layout, copy, spacing, motion timing - will be detected automatically. The summary will be attached to the task as a generated comment, readable by the team immediately after the client review session closes.

**Planned behaviors:**

- Triggered manually by a "Summarize Feedback" action on any task with two or more annotations
- Aggregate annotations across all visible versions or a specific version (user-selectable)
- Group feedback by theme (color, layout, typography, copy, motion, etc.)
- Preserve direct quotes from individual annotations linked back to the source pin
- Post summary as a task comment attributed to AI, editable by the team

---

### Predictive Risk Detection

Budget burn alerts (FR-1.103) notify operators when a project reaches a spend threshold. That is useful, but reactive. By the time a project is at 75% budget, the over-servicing has already happened.

Predictive Risk Detection will analyze active project signals continuously to surface risk before thresholds are crossed. The system will watch burn rate velocity (how fast budget is being consumed relative to task completion), unresolved task dependencies approaching due dates, team members with blocked tasks and no alternative work, and the ratio of logged hours to estimated hours on in-progress tasks.

When a project's risk score crosses a configurable threshold, a risk card will appear in the project dashboard and an alert will be sent to the project lead with the specific contributing signals - not just "this project is at risk" but "budget burn is 2.3x the expected rate at this milestone stage, and three dependent tasks are blocked."

**Planned behaviors:**

- Run continuously on all active projects with a budget or deadline defined
- Risk signals: burn rate velocity, task completion rate, blocked dependency ratio, logged-vs-estimated hour divergence
- Surface risk score and contributing factors on the project dashboard
- Threshold-based alerting to project lead and organization admin (configurable)
- Track risk history over the project lifecycle for retrospective analysis

---

### Conversational Project Copilot

The Copilot will be a persistent natural language interface accessible from anywhere in the platform. Operators will be able to ask questions, request summaries, and trigger scoped actions without navigating to the relevant screen.

The Copilot will not be a general-purpose AI assistant. It will have structured access to the operator's organization data - projects, tasks, assets, clients, time entries, and invoices - and respond with factual answers grounded in live platform state. All queries will be scoped to the user's active organization and permission set. A user without access to a project will not be able to retrieve data from it through the Copilot.

**Example queries:**

| Query | Response type |
|---|---|
| "What tasks are awaiting client approval across all active projects?" | Filtered task list |
| "How much budget is remaining on the Vogue Campaign project?" | Budget snapshot |
| "Show me all assets uploaded this week by the design team." | Asset list |
| "Create a revision task under the Homepage Redesign milestone for the hero section." | Action (with confirmation) |
| "Summarize the status of all projects due this month." | Project status summary |

**Planned behaviors:**

- Accessible via a global command interface (keyboard shortcut or persistent sidebar panel)
- Read queries return live data from the active organization, scoped by the user's permissions
- Write actions (create task, update status, assign member) require explicit user confirmation before execution
- Responses include deep links back to the relevant entity in the platform
- Query history retained per user session

---

### Smart Automation Suggestions

Configuring workflow automations (FR-1.122) requires operators to identify patterns in their own work and translate them into trigger-action rules. Most teams do not configure automations until they have been on the platform long enough to recognize the patterns - which means weeks of manual repetition before automation kicks in.

Smart Automation Suggestions will observe team workflow patterns across projects and surface automation rule proposals when a pattern is consistent enough to be worth automating. Suggestions will be shown as a proposed rule with a plain-language description and a one-click acceptance flow.

**Example patterns to detect:**

- "Every time a task moves to *Internal Review*, a comment is left and the task is reassigned to the same person. Suggested rule: when task moves to *Internal Review*, notify [person] and auto-assign."
- "New projects always have a *Kickoff Call* task created in the first hour. Suggested rule: when a new project is created, automatically create a *Kickoff Call* task."

**Planned behaviors:**

- Pattern analysis runs on a per-organization basis with a minimum observation window (configurable, default 14 days)
- Rank suggestions by observed frequency and time-saving potential
- Each suggestion shows the detected pattern, proposed rule, and estimated weekly time saving
- Accepted suggestions create a live automation rule (FR-1.122) immediately
- Dismissed suggestions are not re-surfaced unless the pattern significantly changes

---

## Planned Architecture & Model Strategy

The AI capabilities will be implemented as a dedicated AI module within the MiramaService modular monolith boundary. The module will expose an internal interface consumed by other modules (CRM for brief parsing, Assets for tagging, Projects for risk detection) without those modules depending directly on LLM provider APIs. This boundary is intentional - the core platform is being designed with clean event and data surfaces specifically so the AI module can slot in without requiring structural changes to existing modules.

| Capability | Model type | Trigger |
|---|---|---|
| Brief Intelligence | LLM (text) | Intake form submission |
| Asset Tagging | Multimodal vision | Asset upload completion |
| Annotation Summarization | LLM (text) | Manual user action |
| Risk Detection | Heuristic + lightweight ML | Scheduled (continuous) |
| Conversational Copilot | LLM (text) with tool use | User query |
| Automation Suggestions | Pattern detection | Scheduled (per organization) |

**Model provider:** The platform will use the Anthropic Claude API as the primary LLM provider. Claude's extended context window and instruction-following reliability make it well suited for structured extraction tasks (brief parsing, annotation summarization) and the tool-use patterns required by the Copilot. Vision tasks will use a multimodal model from the same provider family where supported, with a fallback to a dedicated vision API for asset types outside the primary model's input limits.

**Privacy and data handling:**

- Asset content sent to vision models will be processed at proxy/preview resolution, never the original full-resolution file.
- Client brief text will be sent to the LLM provider for processing. Organizations operating under data residency constraints will be able to configure the AI features to run against a self-hosted or Azure-hosted model endpoint.
- No organization data will be used to train or fine-tune external models.
- All AI processing will be logged in the platform audit trail (FR-1.133) with the model version, input hash, and output stored for accountability.

---

## Integration Points

| Platform feature | AI capability |
|---|---|
| Intake forms (FR-1.111) | Brief Intelligence |
| Lead-to-project conversion (FR-1.114) | Brief Intelligence |
| Project templates (FR-1.121) | Brief Intelligence |
| Asset upload (FR-1.21–1.24) | Vision-Based Asset Tagging |
| Global search (FR-1.123) | Vision-Based Asset Tagging |
| Click-on-spot annotation (FR-1.64) | Annotation Summarization |
| Budget burn alerts (FR-1.103) | Predictive Risk Detection |
| Capacity & workload view (FR-1.104) | Predictive Risk Detection |
| Task navigation (platform-wide) | Conversational Copilot |
| Automated workflow triggers (FR-1.122) | Smart Automation Suggestions |
