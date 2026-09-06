# Mirama Market Analysis

> This document is market and customer analysis. It explains who Mirama is built for, why, what it costs them not to have it, who else is trying to solve the same problem, and how the business itself makes money doing it. The product scope lives in [Requirements](requirements.md) and [Project Description](index.md). The technical response to this scope lives in [System Architecture](system-architecture.md).

---

## 1. Executive Summary

Independent creatives, boutique agencies and the delegating leads who run them operate in a state of chronic **fragmented tool fatigue**. Executing a single client project today means stitching together four to six disconnected platforms: one for moodboards and ideation, one for tasks and timelines, one for asset review and versioning, one for file storage, and email or chat for everything client-facing. None of them talk to each other.

That fragmentation has two distinct costs, and Mirama is built to eliminate both from the same data model rather than as two separate products bolted together:

- **The horizontal cost, paid by every visual professional:** disconnected ideation and execution, revision chaos with no version lock, unbilled scope creep, and enterprise project-management tools that were never designed for people who think in images rather than rows.
- **The vertical cost, paid the moment a solo operator stops working alone:** bringing in a subcontractor or collaborator today means either giving them full visibility into client budgets and other people's rates, or managing a parallel spreadsheet and side-channel just to keep that information private, with payouts settled from memory once the client pays.

Mirama unifies free-form visual brainstorming, rigorous asset versioning, structured project management (Kanban and Gantt), client-facing delivery portals, and — for operators who delegate — scoped subcontractor access and split payouts, into a single cohesive workspace. The core positioning is visual-first project execution for creative freelancers, boutique agencies and independent studios; the delegation layer is what makes that positioning durable once a customer grows past working alone, rather than the whole story on day one.

---

## 2. The Opportunity

This analysis, and the go-to-market it supports, starts in the EU. The product itself is not geographically limited, but the initial market, first customer conversations, and launch positioning are built around the EU independent workforce, with expansion beyond it treated as a later question, not a launch requirement.

The EU's independent workforce is large and still growing. Self-employed workers made up 14.2% of total EU employment in 2024, with self-employment without employees (the closest proxy for freelancers proper) at 7.4% and still growing, up 141,000 people over the year (Eurostat, Self-employment statistics). The Netherlands and Malta sit highest among member states at 13 to 17% depending on the exact measure, with Germany the single largest national market for freelance platform spend in Europe (KBV Research, Europe Freelance Platforms Market). The wider Upwork Freelancing Stats research, largely US-sourced, puts the freelance economy at roughly $1.5 trillion in earnings globally and points to the same underlying trend outside the US.

Within that market, two overlapping demand pools matter to Mirama:

- **The visual-first execution gap.** Every independent creative and boutique studio, regardless of whether they ever delegate work, currently pays a "tool sprawl tax" stitching together a moodboard tool, a task tracker, a file host and a communication channel. This is the larger, more horizontal demand pool and the one the product should be positioned against first.
- **The delegation triangle.** A narrower, currently unowned relationship between a lead freelancer, the subcontractors they bring in, and the client who only ever sees the lead. Every adjacent tool solves one leg of that triangle and quietly breaks down at the others. This is a real, validated pain point (see Section 4) and a strong differentiator once a customer is evaluating alternatives, but it is not, on its own, a broad enough wedge to lead top-of-funnel positioning.

---

## 3. Core Industry Pain Points

- **The Tool Sprawl Tax.** Creatives pay for and manage separate subscriptions for moodboards, task boards, storage repositories and communication channels. This inflates monthly overhead and introduces friction as users constantly copy-paste assets across apps just to keep a project coherent.
- **Disconnected Workflows.** Moodboards and creative brainstorming live in a completely different system than the operational project tracker. An image that inspires a design has no direct link to the task or deliverable it produces, so project execution drifts from creative intent the moment work actually starts.
- **Revision Chaos & Scope Creep.** Without rigid, version-controlled deliverables (V1, V2, Final), client reviews descend into email chains and scattered chat comments. This produces endless unbilled revisions and undermines the ability to enforce scope boundaries at all.
- **Clunky Enterprise Bloat.** Existing project-management heavyweights (Asana, ClickUp, Monday) are built for corporate software teams. They lean on dense, text-heavy spreadsheet and database views that feel uninspiring and administratively heavy for visual professionals who think in images, not rows.
- **The Delegated Work Blind Spot.** The moment a lead brings in a subcontractor to help deliver client work, none of the tools above have an answer. Financial and scope isolation either doesn't exist or is bolted on badly (see Section 4), and payout happens from memory, off-platform, once the client has paid.

---

## 4. Competitive Landscape

No single competitor addresses more than one or two of the five pain points above. The market is a set of point solutions, each solving one slice cleanly and breaking down everywhere else.

**Visual ideation & moodboarding** (Milanote, Mural, PureRef): strong at free-form visual brainstorming, no structured task execution, no versioning discipline, no client delivery layer. An idea captured here has no path to becoming a tracked, billable deliverable.

**Generic project management** (Asana, ClickUp, Monday): mature Kanban/Gantt tooling and broad integration ecosystems, but built around text-first, database-style work items. No native visual-asset versioning, no watermarked client review flow, and priced and structured for internal software teams, not client-facing creative delivery.

**Proofing & asset review** (Frame.io, Approval Studio): confirmed to be purely review-and-annotation platforms, with no task management, billing, invoicing, scope-creep or change-order features of any kind. Post-Adobe-acquisition pricing analysis of Frame.io also reports small studios being routed toward Enterprise quotes earlier than their size justifies — a marketable weakness a fairly-priced, studio-sized alternative can position directly against.

**Freelancer business suites** (HoneyBook, Bonsai, Dubsado, Moxie/Hectic, Plutio): built around a single freelancer working with a single client — proposals, contracts, invoicing, a client portal. Team features exist but are shallow add-ons, not a core design assumption. Moxie's paid Teams tier is the clearest proof point: independent review coverage states it caps at five members with no custom roles, no per-task permissions and no contractor-specific access controls, and that everyone sees identical data including client financials. All five are US-headquartered and US-first; none advertise native multi-currency payouts, EU VAT-compliant invoicing, or GDPR-first handling as a core design point, a real localization gap for an EU-first entrant, though this should be validated directly with EU buyers rather than assumed from marketing alone.

**Scope & change-order tools** (ScopePilot): a standalone product, priced at $14.99/month, that does nothing but scope documents, revision tracking and auto-generated change orders, and states outright it is not a full project-management or time-tracking tool. Operators pay separately for this because no integrated platform does it well — proof the need is real, not proof the need is small.

**Contractor payroll** (Deel, Routable): handle payroll and compliance with no project or creative context whatsoever. Nobody connects "the client approved this" to "release the file, pay the subcontractor their split, bill the change," with an audit trail back to the approval.

### Capability Matrix

| Capability | Milanote / Mural | Asana / ClickUp / Monday | Frame.io / Approval Studio | HoneyBook / Bonsai / Dubsado / Moxie | ScopePilot | Deel / Routable | **Mirama** |
|---|---|---|---|---|---|---|---|
| Visual moodboarding / ideation | Yes | No | No | No | No | No | Yes |
| Moodboard-to-task linking | No | No | No | No | No | No | Yes |
| Kanban / Gantt execution | No | Yes | No | No | No | No | Yes |
| Native asset versioning & annotation | Partial | No | Yes | No | No | No | Yes |
| Watermarked client review portal | No | No | Partial | Partial | No | No | Yes |
| Solo freelancer CRM & invoicing | No | No | No | Yes | No | No | Yes |
| Scope guardrails & auto change orders | No | No | No | No | Yes (standalone) | No | Yes |
| Subcontractor-scoped access & financial isolation | No | No | No | Partial (Moxie, capped at 5, no isolation) | No | Payroll only, no project context | Yes |
| Automated split payouts on milestone approval | No | No | No | No | No | Manual, no delivery trigger | Yes |

The pattern holds across every row: competitors are strong verticals and weak horizontals. Mirama's bet is that the same relational data model — client, project, task, asset version, milestone, invoice — that makes visual-first execution coherent for a solo creative is the same model that makes subcontractor scoping and split payouts fall out almost for free once an operator delegates. Nobody else has that model; they have five separate ones stapled together by the user.

---

## 5. Who We Should Not Build For

"Freelancers" as a broad buyer category is a weaker bet than the size of the freelance economy suggests. Self-employed workers see roughly 30% month-to-month income variation, against about 14% for salaried employees, more than double the volatility (JPMorgan Chase Institute, US-sourced, cited for the general pattern). Income unpredictability is freelancers' single biggest reported stressor in Upwork's Freelance Forward research, ahead of clients or isolation. Malt's Freelancing in Europe 2024 survey of over 5,000 freelancers across Germany, France, Spain, the Netherlands, Belgium and the UK shows a structural shift toward established operators: freelancer time spent serving large corporate clients, the more stable, repeat-engagement end of the market, rose sharply — from 20% to 31% in Germany and 15% to 26% in France. Freelancers who derive most of their revenue from repeat clients report significantly lower volatility than project-to-project operators, meaning the established, retainer-based operator is a structurally different and better software customer than "freelancers" as a whole.

HoneyBook, the closest and best-funded comparable, is explicit about this itself: it targets profitable independent business owners with high willingness to pay, not freelancers broadly, and reached $135 million ARR in 2024 (up 12% year over year) predominantly from its US base, with payment processing — not subscriptions — as its most resilient revenue line ($5B+ routed since inception). HoneyBook also raised subscription prices by up to 89.5% in February 2025 and drew visible public backlash and stated switching intent, on top of a documented pattern of subscription fatigue among solo operators who actively audit and cut tool spend. Subscription pricing alone has real limits, even for the market leader.

**The conclusion:** target the established, repeat-client operator — whether they call themselves a freelancer, a two-person studio or a boutique agency — not the early or volatile-income freelancer still finding their first few clients. That second group is the segment most prone to subscription-fatigue cancellation, and it is not who HoneyBook targets either.

---

## 6. Who We Are Building For

**The Independent Creative / Boutique Studio Owner.** The core buyer. Runs client work solo or with a very small team, thinks visually, and currently loses deep-work time to context-switching between a moodboard tool, a task tracker and a file host. Wants speed, minimalist aesthetics and a workspace that doesn't feel like corporate software.

**The Delegating Lead.** A senior freelancer or micro-agency owner, roughly one to twenty people, typically with one to five active subcontractors at a time, who owns the client relationship and brings in trusted specialists to deliver part of the work — a designer bringing in a motion artist, a dev lead bringing in a backend contractor. Today, client financials and other subcontractors' rates are either fully visible to everyone with portal access, or the lead resorts to spreadsheets and side channels to keep things private, and paying subcontractors is a manual transfer done from memory once the client has paid.

**The Fractional Executive.** Runs a retainer-plus-overage model across three to eight concurrent clients, sometimes looping in a specialist subcontractor for execution work the retainer doesn't cover directly. Today, retainer hours and overage work live in a spreadsheet with no clean way to bill one blended client rate while paying a looped-in subcontractor a different rate.

**The Subcontractor or Collaborator.** Brought onto a specific project by a lead. Not the account owner, often working across several different leads and platforms at once. Today, scope ownership is unclear, fee terms are captured over email, payout timing is uncertain, and leads are often reluctant to grant portal access at all because it exposes client budgets and other people's rates.

**The End Client.** Reviews structured, version-locked deliverables through a clean portal, without ever needing to know a subcontractor exists or being exposed to internal team backlogs or messy ideation drafts.

---

## 7. Cost-Benefit & Financial ROI Analysis

To justify a recurring subscription, Mirama has to actively protect client margins and eliminate tangible operational expense, not just look nicer than a spreadsheet. The figures below are directional targets grounded in the pain points and competitor pricing in Sections 3–4; they should be validated with real customer interviews and usage data as part of pricing work, not treated as measured outcomes yet.

### Financial and Operational Impact Matrix

| Traditional Workflow Pain Point | The Mirama Solution | Quantifiable Value & ROI (directional, to validate) |
| :--- | :--- | :--- |
| **Subscription bloat** (moodboard tool + task tracker + file host, e.g. Milanote/Mural + Asana/ClickUp + Dropbox/Frame.io) | Replaces 3–4 disparate tools with one workspace | Saves an estimated **$40–$90/month** per user in direct software overhead |
| **Unbilled scope creep** | Version-locked deliverables (V1 vs. Final) with trackable client sign-off states, plus scope guardrails and auto-generated change orders | Eliminates an estimated **3–5 hours of unbilled revision work per project** |
| **Admin lag & context switching** | Moodboards linked directly to tasks and milestones; one workspace instead of five | Recovers an estimated **4–6 billable hours per week** previously lost to manual asset sorting and status updates |
| **Client feedback friction** | Streamlined presentation portals with centralized inline commentary and version comparison | Shortens project delivery lifecycles by an estimated **20%**, accelerating invoice approval and cash flow |
| **Manual subcontractor payout tracking** (delegating leads only) | Automatic split payouts via Stripe Connect on milestone approval, with an audit trail back to the approval | Removes the manual reconciliation step entirely; payout timing becomes deterministic rather than memory-dependent |

### Professional Positioning Value

Beyond direct software savings, Mirama raises the perceived maturity of independent freelancers and small studios. Presenting clients with a fast, structured, version-controlled delivery space lets smaller operators project the polish of an enterprise agency — a qualitative effect that, combined with faster approval cycles, supports the case for higher project rates over time.

---

## 8. Margin Analysis

Two different margins matter here: the customer's project margin, and Mirama's own unit economics as a SaaS business. They are related — the pricing model has to sit comfortably below the value delivered in Section 7 while still supporting a healthy gross margin — but they are not the same analysis.

### 8.1 Customer Margin Protection

The ROI matrix in Section 7 is, functionally, a margin-protection argument: every line item is either avoided software spend or recovered billable time, both of which drop straight to a freelancer's or studio's bottom line since there is no large fixed-cost base to amortize against. The most direct lever is scope creep: 3–5 hours of unbilled revision work per project, multiplied across a studio running even 4–6 concurrent projects a month, represents a meaningful share of monthly billable capacity being given away for free today. Scope guardrails and version-locked sign-off states convert that from "quietly absorbed" to "either billed as a change order or explicitly declined," which is margin recovered rather than margin created from nothing — an easier claim to defend to a skeptical buyer than a vague productivity promise.

### 8.2 Mirama Unit Economics & Pricing Strategy

Mirama is asset-heavy relative to a typical text-first SaaS product (large file uploads, version stacks, watermarked proxies, video), which puts real weight on storage and delivery cost (S3, CloudFront) as a share of COGS. This argues for a pricing and packaging strategy that ties naturally to asset volume and seat count rather than a single flat per-seat price, so heavy-asset studios don't erode gross margin relative to light, text-mostly operators on the same plan:

- **Base subscription, tiered by seats and active projects** — the primary, predictable revenue line, comparable to HoneyBook's or Plutio's subscription tier, priced to sit clearly below the $40–$90/month in replaced tool spend from Section 7.
- **Storage/asset-volume tier or overage** — protects margin against the small share of customers with unusually heavy video or high-resolution asset libraries, without penalizing the typical user on the base tier.
- **Payment and payout processing** — a transaction-based line on invoicing and, for delegating leads, on split payouts via Stripe Connect. This mirrors HoneyBook's most resilient revenue line (subscriptions vs. $5B+ processed since inception) and is structurally attractive here specifically because delegation is a differentiated capability nobody else offers — competitors can't easily match a payout fee they have no product surface to charge for.

The subscription-only model has a documented ceiling: HoneyBook's 89.5% price increase in February 2025 drew public backlash and stated switching intent (Section 5), and solo operators are shown to actively audit and cut subscription spend. A blended model — subscription for predictable baseline revenue, transaction share for the highest-value, highest-willingness-to-pay moments (client payment, subcontractor payout) — is likely more durable than either alone, though the exact split remains an open pricing question (Section 10) to settle with real usage data rather than in this document.

---

## 9. Core Product Pros

- **Visual-first, not visual-only.** Tasks and timelines are natively linked to visual assets — a task can be spawned directly from a piece of inspiration on a moodboard — while the same platform works cleanly as a plain task tracker for non-visual, non-creative delivery work.
- **Speed and minimalist aesthetics.** Keyboard-driven navigation, no administrative lag, built to keep creators in deep work rather than pulling them into dense database views.
- **Version-locked deliverables.** V1/V2/Final states with explicit client sign-off replace messy email chains and unbilled revision spirals.
- **Controlled client portals.** Clean, professional viewing links; clients review structured, version-locked deliverables without exposure to internal backlogs, messy drafts, budgets or subcontractor identities.
- **Delegation-aware by design.** The moment a lead brings in a subcontractor, access and payouts scope down automatically — a capability no direct competitor offers at any price point (Section 4).
- **One relational data model.** Client, project, task, asset version, milestone and invoice are connected from day one, which is what makes billing, reporting and delegation fall out of the same system instead of requiring a bolt-on module later.
- **Freelancer-first scaling.** Every feature makes sense for a single-person operation first; team and delegation capabilities are extensions of that foundation, not a separate mode requiring migration.

---

## 10. Where the Line Sits

In scope: independent creatives, boutique agencies and micro-agencies, roughly one to twenty people, with an existing repeat-client revenue base, across creative, technical or consulting disciplines alike. This includes a parallel entry point among small agencies who already budget for agency management software (Ravetree, Kantata, Workamajig, Accelo, BigTime) and mix salaried staff with contractors — none of those incumbents serve the visual-first or contractor-access case either. Geographically, the launch market is the EU, starting with the larger national freelance markets such as Germany, France and the Netherlands; expansion beyond the EU is expected but is a later-stage question.

Out of scope for now: pre-revenue or early, volatile-income solo freelancers still acquiring their first few clients, the 75-to-200-plus person established firm tier, and markets outside the EU at launch.

---

## 11. Open Questions

- Whether top-of-funnel messaging should lead with the horizontal visual-first pitch (Sections 2–3) in all channels, or whether the delegation wedge (Section 4, Section 8.2) should lead specifically in channels reaching delegating leads and fractional executives, who are more likely to already be evaluating agency-management alternatives.
- What pricing model best fits the payment-flow insight in Section 8.2 — a transaction fee on gated releases, change-order payments and split payouts, against a flat subscription, against a blend, and where the split should sit.
- Whether the small-agency parallel entry point (Section 10) needs its own outreach motion distinct from the freelancer and micro-agency motion, given they arrive already expecting to pay for this category of software.
- Which EU country or countries to prioritize first within the launch geography. Germany is the largest national freelance-platform market, but that does not automatically make it the easiest first market for outreach, localization or payment rails.
- What the real, measured version of the Section 7 ROI figures looks like once validated against actual customer usage, rather than the directional estimates used here.

---

## Sources

- [Upwork, Freelancing Stats 2026](https://www.upwork.com/resources/freelancing-stats)
- [Moxie alternatives comparison, noting Teams-tier access control gaps](https://www.plutio.com/alternatives/moxie)
- [ScopePilot, standalone scope-creep and change-order product](https://www.getscopepilot.com/)
- [Deel, contractor management and payment software roundup](https://www.deel.com/blog/best-contractor-management-software-for-global-payments/)
- [Worksuite, contractor payment software roundup 2026](https://worksuite.com/resources/insights/best-contractor-payment-software)
- [HoneyBook vs Bonsai comparison](https://www.honeybook.com/honeybook-vs-bonsai)
- [Dubsado vs HoneyBook comparison](https://www.plutio.com/compare/honeybook-vs-dubsado)
- [JPMorgan Chase Institute freelancer income volatility research, via planwith.ai](https://planwith.ai/blog/research-on-freelancer-income-stability)
- [Sacra, HoneyBook revenue analysis](https://sacra.com/c/honeybook/)
- [HoneyBook 2025 price increase and user reaction](https://taskip.net/honeybook-pricing/)
- [Ravetree, agency management software landscape](https://www.ravetree.com/blog/the-5-best-agency-management-software-solutions)
- [Frame.io pricing, scope and Adobe enterprise-upsell pattern](https://playpause.io/blogs/frame-io-pricing)
- [Approval Studio review, features, pricing, scope](https://thedigitalprojectmanager.com/tools/approval-studio-review/)
- [Eurostat, Self-employment statistics](https://ec.europa.eu/eurostat/statistics-explained/index.php/Self-employment_statistics)
- [KBV Research, Europe Freelance Platforms Market size and forecast](https://www.kbvresearch.com/europe-freelance-platforms-market/)
- [Malt, Freelancing in Europe 2024 economic report](https://pages.malt.com/freelancing-in-europe-2024)
