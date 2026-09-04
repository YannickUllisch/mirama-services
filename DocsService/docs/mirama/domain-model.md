# Domain Model

How Mirama's domain objects fit together across the `Identity`, `Clients`, and `PM` modules (the `Workspace` module is under active design and left out for now).

Diagram sources: `diagrams/Core/DomainModel.mmd` (overview) plus one detail diagram per module — `DomainModel-Identity.mmd`, `DomainModel-Clients.mmd`, `DomainModel-PM.mmd`.

## The hierarchy — a Linear-style pipeline adapted for client work

Linear structures work as `Workspace → Team → Project → Cycle → Issue`. Mirama adapts that same shape into a client-facing delivery pipeline:

| Linear concept | Mirama concept | Role |
|---|---|---|
| Workspace | **Tenant** *(hidden)* | Platform-level root. Not user-facing — exists for billing, multi-org scaling, and future subcontractor management. |
| — | **Organization** | The freelancer/agency's studio. Highest level a user actually sees: manages members, teams, roles, and every client relationship. |
| Team | **Client** | An isolated space per client — backlog, brief, and communication equivalent. Owns intake submissions and CRM history. |
| Project | **Project** | A specific contract or job ("Beachside Villa Remodel"). |
| Cycle | **Milestone** (`Cycle`) | Time-boxed, client-facing phase focused on sign-off rather than sprint velocity ("Phase 1: Concept Board"). |
| Issue | **Deliverable** (`Asset`) | The atomic unit of feedback — a file, 3D embed, video timeline, or mood board item, not text/code. |

`Tenant` sits above `Organization` as a hidden class purely for future scale-out (multiple organizations per tenant, agency-of-agencies billing, etc.) — it is never exposed in product UI today.

### Overview diagram

```mermaid
flowchart TB
    Tenant["Tenant\n(hidden platform root)"]

    subgraph IdentityMod["Identity module"]
        Organization["Organization\nThe Studio\nLinear: Workspace"]
    end

    subgraph ClientsMod["Clients module"]
        Client["Client\nIsolated client space\nLinear: Team"]
    end

    subgraph PMMod["PM module"]
        Project["Project\nContract / job\nLinear: Project"]
        Milestone["Milestone (Cycle)\nPhase, sign-off\nLinear: Cycle"]
        Deliverable["Deliverable (Asset)\nFeedback item\nLinear: Issue"]
        Project --> Milestone --> Deliverable
    end

    Tenant -. owns .-> Organization
    Organization -->|scopes| Client
    Organization -->|scopes, current code| Project
    Client -. target link — NOT YET in code .-> Project

    classDef hidden fill:#eee,stroke:#999,stroke-dasharray: 4 4,color:#666;
    class Tenant hidden;

    note1["Gap: Project is scoped to Organization today, not Client.\nIntakeFormSubmission.ConvertedToClientId is the only\nexisting bridge between Clients and PM."]
    style note1 fill:#fff8dc,stroke:#c9b458,color:#665c1e
```

## Module boundaries

Each level of the hierarchy that is its own module gets its own detail diagram below. Cross-module references go through plain `Guid`/typed-ID fields (`OrganizationId`, `ClientId`, `ProjectId`, …), never direct object references — each module's aggregates stay independently loadable and each bounded context stays isolated, the same pattern Linear uses to keep Teams independently scalable. In each module diagram, classes owned by another module appear only as `<<external>>` stubs carrying their id.

### Identity module (Organization level)

Owns `Tenant` (hidden platform root) and `Organization` — the Linear "Workspace" equivalent — plus everything about who can act: `Member`, `User`, `Team`, `Role`/`Policy`, `Plan`/`Subscription`, `Invitation`, `Tag`.

```mermaid
classDiagram
    direction TB

    namespace Platform_Hidden {
        class Tenant {
            <<AggregateRoot>>
            +Guid Id
            +UserId AdminUserId
            +string Name
            +bool IsActive
        }
    }

    namespace Identity_Module {
        class Organization {
            <<AggregateRoot>>
            +OrganizationId Id
            +Guid TenantId
            +string Name
            +string Slug
            +OrganizationRegion Region
        }
        class Member {
            <<Entity>>
            +MemberId Id
            +UserId UserId
            +List~RoleId~ IamRoleIds
        }
        class User {
            <<AggregateRoot>>
            +UserId Id
            +OrganizationId? DefaultOrganization
        }
        class Team {
            <<AggregateRoot>>
            +TeamId Id
        }
        class TeamMember {
            <<Entity>>
            +TeamMemberId Id
            +MemberId MemberId
        }
        class Invitation {
            <<Entity>>
            +InvitationId Id
            +RoleId IamRoleId
            +InvitationStatus Status
        }
        class Tag {
            <<AggregateRoot>>
            +TagId Id
            +TagScope Scope
        }
        class Role {
            <<AggregateRoot>>
            +RoleId Id
            +Guid? TenantId
            +List~PolicyId~ Policies
        }
        class Policy {
            <<AggregateRoot>>
            +PolicyId Id
            +Guid? TenantId
            +List~PolicyStatement~ Statements
        }
        class Plan {
            <<AggregateRoot>>
            +PlanId Id
        }
        class Subscription {
            <<Entity>>
            +SubscriptionId Id
            +PlanId PlanId
            +SubscriptionStatus Status
        }
    }

    Tenant "1" *-- "1" Subscription : owns
    Subscription "0..*" --> "1" Plan : subscribes to
    Tenant "1" --> "0..*" Organization : owns (TenantId)
    Tenant "1" --> "0..*" Role : issues tenant-custom roles
    Tenant "1" --> "0..*" Policy : issues tenant-custom policies
    Tenant "1" --> "1" User : AdminUserId

    Organization "1" *-- "0..*" Member : has
    Organization "1" *-- "0..*" Team : has
    Organization "1" *-- "0..*" Invitation : has
    Organization "1" *-- "0..*" Tag : has
    Member "0..1" --> "1" User : linked account
    Member "0..*" --> "0..*" Role : IamRoleIds
    Team "1" *-- "0..*" TeamMember : has
    TeamMember "0..*" --> "1" Member : references
    Invitation "0..*" --> "1" Role : grants
    Role "0..*" --> "0..*" Policy : Policies

    note for Tenant "Hidden platform root — not surfaced to end users.\nExists for scaling/ops (billing, multi-org, future subcontractor mgmt)."
    note for Organization "'Organization' = Linear 'Workspace'. The freelancer/agency's studio — highest level a user actually sees."
```

### Clients module (Client level)

Owns the `Client` space — the Linear "Team" equivalent. Intake forms and submissions, CRM (`Contact`, `ClientActivityLogEntry`, `PipelineStageHistoryEntry`), client portal access (`ClientPortalUser`/`ClientPortalInvitation`), and `Contract`s. `Organization` appears only as an external stub, referenced by `OrganizationId`.

```mermaid
classDiagram
    direction TB

    class Organization {
        <<external>>
        +OrganizationId Id
    }

    namespace Clients_Module {
        class Client {
            <<AggregateRoot>>
            +ClientId Id
            +Guid OrganizationId
            +ClientStatus Status
            +ClientType Type
            +Guid OwnerMemberId
        }
        class Contact {
            <<Entity>>
            +ContactId Id
            +ClientId ClientId
        }
        class ClientPortalUser {
            <<Entity>>
            +ClientPortalUserId Id
            +ClientId ClientId
            +Guid ContactId
        }
        class ClientPortalInvitation {
            <<Entity>>
            +ClientPortalInvitationId Id
            +ClientId ClientId
            +Guid ContactId
        }
        class ClientActivityLogEntry {
            <<Entity>>
            +ClientActivityLogEntryId Id
            +ClientId ClientId
        }
        class PipelineStageHistoryEntry {
            <<Entity>>
            +PipelineStageHistoryEntryId Id
            +ClientId ClientId
        }
        class Contract {
            <<AggregateRoot>>
            +ContractId Id
            +Guid OrganizationId
            +Guid PartyId
            +ContractPartyType PartyType
            +ContractStatus Status
        }
        class ContractTerm {
            <<Entity>>
            +ContractTermId Id
            +ContractTermType Type
        }
        class IntakeForm {
            <<AggregateRoot>>
            +IntakeFormId Id
            +Guid OrganizationId
        }
        class IntakeFormField {
            <<Entity>>
            +IntakeFieldType Type
        }
        class IntakeFormSubmission {
            <<AggregateRoot>>
            +IntakeFormSubmissionId Id
            +IntakeFormId IntakeFormId
            +ClientId? ConvertedToClientId
            +IntakeFormSubmissionStatus Status
        }
    }

    Organization "1" --> "0..*" Client : scopes (OrganizationId)
    Client "1" *-- "0..*" Contact : has
    Client "1" *-- "0..*" ClientPortalUser : has
    Client "1" *-- "0..*" ClientPortalInvitation : has
    Client "1" *-- "0..*" ClientActivityLogEntry : has
    Client "1" *-- "0..*" PipelineStageHistoryEntry : has
    ClientPortalUser "0..*" ..> Contact : ContactId
    ClientPortalInvitation "0..*" ..> Contact : ContactId
    Organization "1" --> "0..*" Contract : scopes
    Contract "1" *-- "0..*" ContractTerm : has
    Contract "0..*" ..> Client : PartyId (when PartyType=Client)
    Organization "1" --> "0..*" IntakeForm : scopes
    IntakeForm "1" *-- "0..*" IntakeFormField : has
    IntakeForm "1" --> "0..*" IntakeFormSubmission : receives
    IntakeFormSubmission "0..1" ..> Client : ConvertedToClientId

    note for Client "'Client' = Linear 'Team'. Owns intake, portal access, CRM history — isolated per client."
    note for Organization "External stub — owned by the Identity module. Referenced here only by Guid."
```

### PM module (Project / Milestone / Deliverable level)

Owns `Project` execution — the Linear "Project/Cycle/Issue" equivalent. `Cycle` is the **Milestone**, `Asset` is the **Deliverable**; `Task` stays the internal, text-heavy work item (closer to a Linear Issue) while `Asset` is the client-facing item requiring visual sign-off. Also owns `ProjectTemplate` for reusable project scaffolding. `Organization` and `Client` appear only as external stubs.

```mermaid
classDiagram
    direction TB

    class Organization {
        <<external>>
        +OrganizationId Id
    }
    class Client {
        <<external>>
        +ClientId Id
    }

    namespace PM_Module {
        class Project {
            <<AggregateRoot>>
            +ProjectId Id
            +Guid OrganizationId
            +Guid StatusId
            +Guid PriorityId
        }
        class ProjectMilestone {
            <<Entity>>
            +ProjectMilestoneId Id
            +MilestoneStatus Status
        }
        class ProjectMember {
            <<Entity>>
            +ProjectMemberId Id
        }
        class ProjectTeam {
            <<Entity>>
            +ProjectTeamId Id
        }
        class Cycle {
            <<AggregateRoot>>
            +CycleId Id
            +Guid ProjectId
            +CycleStatus Status
        }
        class KanbanBoard {
            <<AggregateRoot>>
            +KanbanBoardId Id
            +Guid ProjectId
        }
        class KanbanColumn {
            <<Entity>>
            +KanbanColumnId Id
        }
        class WorkflowConfig {
            <<AggregateRoot>>
            +WorkflowConfigId Id
            +ProjectId ProjectId
        }
        class StatusConfig {
            <<Entity>>
            +StatusConfigId Id
            +StatusCategory Category
        }
        class PriorityConfig {
            <<Entity>>
            +PriorityConfigId Id
        }
        class Task {
            <<AggregateRoot>>
            +TaskId Id
            +Guid ProjectId
            +TaskId? ParentTaskId
            +TaskType Type
            +Guid StatusId
            +Guid PriorityId
        }
        class TaskComment {
            <<Entity>>
            +TaskCommentId Id
            +TaskId TaskId
        }
        class TaskDependency {
            <<Entity>>
            +TaskDependencyId Id
            +TaskId BlockingTaskId
            +DependencyType Type
        }
        class TimeLog {
            <<Entity>>
            +TimeLogId Id
        }
        class Asset {
            <<AggregateRoot>>
            +AssetId Id
            +Guid? ProjectId
            +AssetVersionId? CurrentVersionId
            +AssetType Type
            +AssetStatus Status
        }
        class AssetVersion {
            <<Entity>>
            +AssetVersionId Id
        }
        class AssetFeedback {
            <<Entity>>
            +AssetFeedbackId Id
            +AssetFeedbackStatus Status
        }
        class FeedbackReply {
            <<Entity>>
            +FeedbackReplyId Id
        }
        class FeedbackAnnotation {
            <<Entity>>
        }
        class AssetConnection {
            <<Entity>>
            +AssetConnectionId Id
            +AssetConnectionTarget Target
        }
        class ProjectTemplate {
            <<AggregateRoot>>
            +ProjectTemplateId Id
            +Guid OrganizationId
        }
        class MilestoneTemplate {
            <<Entity>>
            +MilestoneTemplateId Id
        }
        class TaskTemplate {
            <<Entity>>
            +TaskTemplateId Id
        }
        class CycleTemplate {
            <<Entity>>
            +CycleTemplateId Id
        }
    }

    Organization "1" --> "0..*" Project : scopes (OrganizationId)
    Client "0..*" ..> Project : target link — NOT YET in code
    Project "1" *-- "0..*" ProjectMilestone : has
    Project "1" *-- "0..*" ProjectMember : has
    Project "1" *-- "0..*" ProjectTeam : has
    Project "1" --> "0..1" WorkflowConfig : configures via
    WorkflowConfig "1" *-- "0..*" StatusConfig : has
    WorkflowConfig "1" *-- "0..*" PriorityConfig : has
    Project "1" --> "0..*" Cycle : ProjectId
    Project "1" --> "0..*" KanbanBoard : ProjectId
    KanbanBoard "1" *-- "0..*" KanbanColumn : has
    Project "1" --> "0..*" Task : ProjectId
    Task "0..1" --> "0..*" Task : ParentTaskId (subtasks)
    Task "1" *-- "0..*" TaskComment : has
    Task "1" *-- "0..*" TaskDependency : has
    Task "1" *-- "0..*" TimeLog : has
    Cycle "0..*" --> "0..*" Task : contains
    Project "0..1" --> "0..*" Asset : ProjectId (deliverables)
    Asset "1" *-- "0..*" AssetVersion : has
    Asset "1" *-- "0..*" AssetFeedback : has
    AssetFeedback "1" *-- "0..*" FeedbackAnnotation : has
    AssetFeedback "1" *-- "0..*" FeedbackReply : has
    Asset "1" *-- "0..*" AssetConnection : has
    Organization "1" --> "0..*" ProjectTemplate : scopes
    ProjectTemplate "1" *-- "0..*" MilestoneTemplate : has
    ProjectTemplate "1" *-- "0..*" TaskTemplate : has
    ProjectTemplate "1" *-- "0..*" CycleTemplate : has

    note for Cycle "'Cycle' = plan's 'Milestone'. Time-boxed, client-facing phase focused on sign-off."
    note for Asset "'Asset' = plan's 'Deliverable'. Task = internal work item (closer to Linear 'Issue')."
    note for Organization "External stub — owned by the Identity module. Referenced here only by Guid."
    note for Client "External stub — owned by the Clients module. Target link not yet implemented."
```

## Current gap vs. the target shape

**`Project` is scoped directly to `Organization` today, not to `Client`.** The `Clients` and `PM` modules don't have a `ClientId` link between them yet — `IntakeFormSubmission.ConvertedToClientId` is the only bridge that exists (a submission converts into a `Client`). Wiring `Project` under `Client` is the next step to close the gap between current code and the target pipeline described above. See the dotted `Client -. target link .-> Project` edge in the overview diagram.
