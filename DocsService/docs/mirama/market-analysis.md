# Mirama Market Analysis

> This document is market and customer analysis. It explains who Mirama is built for and why, backed by the research behind that focus. The product scope itself lives in [Requirements](requirements.md) and [Project Description](index.md). The technical response to this scope lives in [System Architecture](system-architecture.md).

---

## The Opportunity

This analysis, and the go-to-market it supports, starts in the EU. The product itself is not geographically limited, and nothing in the persona model below is EU-specific, but the initial market, first customer conversations, and launch positioning are built around the EU independent workforce, with expansion beyond it treated as a later question, not a launch requirement.

The EU's independent workforce is large and still growing. Self-employed workers made up 14.2% of total EU employment in 2024, with self-employment without employees (the closest proxy for freelancers proper) at 7.4% and still growing, up 141,000 people over the year (Eurostat, Self-employment statistics). The Netherlands and Malta sit highest among member states at 13 to 17% depending on the exact measure, with Germany the single largest national market for freelance platform spend in Europe (KBV Research, Europe Freelance Platforms Market). The wider Upwork Freelancing Stats research, largely US-sourced, puts the freelance economy at roughly $1.5 trillion in earnings globally and points to the same underlying trend outside the US. This is not a shrinking market. It is a large and fragmenting one, which is exactly why a horizontal "do everything for every freelancer" position is so crowded.

Inside that market sits a specific, currently unowned relationship: the triangle between a lead freelancer, the subcontractors they bring in to help deliver client work, and the client who only ever sees the lead. Every adjacent tool solves one leg of that triangle and quietly breaks down at the others.

---

## Who Already Owns Part of the Problem

The dominant freelancer business software category, HoneyBook, Bonsai, Dubsado, Moxie (formerly Hectic) and Plutio, is built around a single freelancer working with a single client: proposals, contracts, invoicing, a client portal. Team features exist but are shallow add-ons rather than a core design assumption. All five are US-headquartered products built US-first; none advertise native multi-currency payouts, EU VAT-compliant invoicing, or GDPR-first data handling as a core design point, which is a real localization gap for an EU-first entrant but should be validated directly with EU buyers rather than assumed from product marketing alone.

Moxie is the clearest proof point. Independent review coverage of its paid Teams tier states plainly that it caps at five members with no custom roles, no per-task permissions and no contractor-specific access controls, and that everyone sees identical data including client financials. That is the exact failure mode this niche targets. The moment a freelancer brings in a second person, financial and scope isolation collapses.

Scope creep and change orders are a validated, monetizable pain point on their own. ScopePilot is a standalone product, priced at $14.99 a month, that does nothing but scope documents, revision tracking and auto-generated change orders, and states outright that it is not a full project management or time-tracking tool. That is a tell. Operators pay separately for scope protection because no integrated platform does it well, not because the need is small.

Proofing tools and contractor payment platforms both solve one slice in isolation. Frame.io and Approval Studio are confirmed to be purely review and annotation platforms, with no billing, invoicing, scope-creep or change-order features of any kind. Contractor payment platforms such as Deel and Routable handle payroll and compliance but carry no project or creative context. Nobody connects "the client approved this" to "release the file, pay the subcontractor their split, bill the change," with a trail back to the approval.

| Capability | HoneyBook / Bonsai / Dubsado | Moxie / Plutio | Frame.io / Approval Studio | ScopePilot | Contractor payroll (Deel, Routable) | Generic PM (Asana, ClickUp) |
|---|---|---|---|---|---|---|
| Solo freelancer CRM and invoicing | Yes | Yes | No | No | No | No |
| Native visual proofing and version history | No | No | Yes | No | No | No |
| Subcontractor-scoped access and financial isolation | No | Partial, Teams tier caps at 5, no isolation | No | No | Payroll only, no project context | No |
| Automated split payouts on milestone approval | No | No | No | No | Manual setup, no delivery trigger | No |
| Scope guardrails and auto change orders | No | No | No | Yes, standalone only | No | No |
| Dependency or blast-radius impact analysis | No | No | No | No | No | Basic dependency lines only |

Frame.io carries an additional distribution risk worth naming, and a marketable weakness alongside it. Post-Adobe-acquisition pricing analysis reports small studios being routed toward Enterprise quotes earlier than their size justifies. A studio-sized, fairly-priced alternative can market directly against that.

---

## Who We Should Not Build For

This is the part of the research that most changes how the opportunity should be approached. "Freelancers" as a broad buyer category is a weaker bet than the size of the freelance economy suggests.

Self-employed workers see roughly 30% month-to-month income variation, against about 14% for salaried employees, more than double the volatility, according to JPMorgan Chase Institute research (US-sourced, cited here for the general pattern rather than an EU-specific figure). Income unpredictability is freelancers' single biggest reported stressor in Upwork's own Freelance Forward research, ahead of clients or isolation, and Malt's Freelancing in Europe 2024 survey of over 5,000 freelancers across Germany, France, Spain, the Netherlands, Belgium and the UK shows the same structural shift toward established operators: freelancer time spent serving large corporate clients, the more stable, repeat-engagement end of the market, rose sharply across those countries, for example from 20% to 31% in Germany and 15% to 26% in France. The consistent finding across both the US and EU research is that freelancers who derive most of their revenue from repeat clients report significantly lower volatility than project-to-project operators, meaning the established, retainer-based operator is a structurally different and better software customer than "freelancers" as a whole.

HoneyBook, the closest and best-funded comparable, is explicit about this itself. It states that it targets profitable independent business owners who demonstrate high willingness to pay, not freelancers broadly, and still reached $135 million in annual recurring revenue in 2024, up 12% year over year, predominantly from its US customer base. Its most resilient revenue line is not subscriptions. It is payment processing, with more than $5 billion routed through the platform since inception. The EU freelance-platform market itself is smaller today but growing faster off a lower base, projected to reach roughly $5.5 billion in revenue by 2033 at an 16.8 to 18.5% CAGR, with Germany the largest single national market (KBV Research, Europe Freelance Platforms Market), which is the scale HoneyBook's model has to be judged against outside the US, not the US figure itself.

There is also good evidence that subscription pricing alone has real limits, even for the market leader. HoneyBook raised subscription prices by up to 89.5% in February 2025 and the reaction was visible public backlash and stated switching intent, on top of a broader, documented pattern of subscription fatigue among solo operators who actively audit and cut tool spend.

One more data point from the other direction. Established agency management software, Ravetree, Kantata, Workamajig, Accelo and BigTime, already has a mature buyer base among small agencies willing to pay $25 to $100 or more per user per month, and none of it clearly addresses subcontractor-specific access. The gap holds even above the pure-freelancer tier, among buyers who are already comfortable paying for this category of software.

**The conclusion:** target the established, repeat-client operator, whether they legally call themselves a freelancer or a two-person studio, and not the early or volatile-income freelancer who is still finding their first few clients. That second group is the segment most prone to subscription-fatigue cancellation, and it is not who HoneyBook targets either.

---

## Who We Are Building For

Four roles make up the relationship this platform serves. The pain points below hold regardless of whether the deliverable is a brand identity, a piece of code or a fractional CFO's board deck.

**The Delegating Lead.** A senior freelancer or micro-agency owner, roughly one to twenty people, typically with one to five active subcontractors at a time, who owns the client relationship and brings in trusted specialists to deliver part of the work. A designer bringing in a motion artist. A dev lead bringing in a backend contractor. A fractional CMO bringing in a paid-media freelancer. Today, client financials and other subcontractors' rates are either fully visible to everyone with portal access, or the lead resorts to spreadsheets and separate side channels to keep things private, and paying subcontractors their cut is a manual transfer done from memory once the client has paid.

**The Fractional Executive.** A fractional CTO, CMO or CFO running a retainer-plus-overage model across three to eight concurrent clients, sometimes solo, sometimes looping in a specialist subcontractor for execution work the retainer does not cover directly. Today, retainer hours and overage work live in a spreadsheet, and there is no clean way to bill a client one blended rate while paying a looped-in subcontractor a different rate, with an audit trail that survives a client's finance team asking questions.

**The Subcontractor or Collaborator.** Brought onto a specific project by a lead or a fractional executive. Not the account owner, and often working with several different leads across different platforms at the same time. Today, scope ownership is unclear, fee terms are captured over email or a message, payout timing is uncertain, and leads are often reluctant to grant portal access at all because it exposes client budgets and other people's rates.

**The End Client.** Should never need to know or care that a subcontractor exists. They see one coherent delivery relationship, one set of approvals and one invoice.

---

## Reaching Them First

The product being built is the full cross-disciplinary platform described in [Requirements](requirements.md): all four roles above, delegated work, blast-radius analysis and split payouts included, not a narrower single-discipline product with the rest bolted on later. Very little of it exists today beyond the project and task execution core, so there is no meaningful reuse argument for entering through one discipline over another, and the engineering build order that does matter, what to build before what, is a dependency question covered in [System Architecture](system-architecture.md) rather than a market one.

What remains a genuine market question is which discipline the earliest outreach speaks to first, since the persona model in this document applies evenly across creative and non-creative work but a first marketing message usually cannot. A creative-visual studio owner with an existing client roster and a fractional executive running a retainer are both squarely inside the target buyer profile in the section below; choosing between them, or running both, is a messaging decision to make with real customer conversations, not something this analysis can settle on its own. See Open Questions.

---

## Where the Line Sits

In scope: freelancers and micro-agencies, roughly one to twenty people, with an existing repeat-client revenue base, who delegate billable client work to subcontractors, across creative, technical or consulting disciplines alike. This includes a parallel entry point among small agencies who already budget for agency management software and mix salaried staff with contractors, since none of the incumbents in that tier serve the contractor-access case either. Geographically, the launch market is the EU, starting with the larger national freelance markets such as Germany, France and the Netherlands; the product and persona model are not EU-specific, and expansion beyond the EU is expected but is a later-stage question, not part of the initial go-to-market.

Out of scope for now: pre-revenue or early, volatile-income solo freelancers still acquiring their first few clients, the 75 to 200-plus person established firm tier, breadth across every original creative vertical as a launch requirement, and markets outside the EU at launch.

---

## Open Questions

- Which one or two disciplines the earliest outreach should speak to directly. The persona model works across creative and non-creative work by design, but marketing likely still needs a concrete starting point rather than addressing all of them evenly at once.
- What pricing model best fits the payment-flow insight above. A transaction fee on gated releases, change-order payments and split payouts, mirroring HoneyBook's most resilient revenue line, against a flat subscription, against a blend.
- Whether the small-agency parallel entry point above needs its own outreach motion (an agency already comparing Ravetree or Accelo) distinct from the freelancer and micro-agency motion, given they arrive at the product already expecting to pay for this category of software.
- Which EU country or countries to prioritize first within the launch geography. Germany is the largest national freelance-platform market, but that does not automatically make it the easiest first market for outreach, localization or payment rails.

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
