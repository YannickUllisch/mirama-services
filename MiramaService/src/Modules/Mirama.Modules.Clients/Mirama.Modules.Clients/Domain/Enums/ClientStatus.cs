namespace Mirama.Modules.Clients.Domain.Enums;

// Prospect is deliberately kept as the entry stage name rather than renamed to
// Lead, it is what the existing intake and client creation flow already writes
// and requirements.md already documents Prospect as functionally a lead.
public enum ClientStatus
{
    Prospect,
    Qualified,
    ProposalSent,
    Active,
    Archived
}
