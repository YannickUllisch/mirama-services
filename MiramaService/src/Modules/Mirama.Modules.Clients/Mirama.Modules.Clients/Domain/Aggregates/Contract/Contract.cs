using Mirama.Modules.Clients.Domain.Events;
using Mirama.SharedKernel.Abstractions.Domain.Core;

namespace Mirama.Modules.Clients.Domain.Aggregates.Contract;

// Deliberately placed inside the Clients module for now rather than a
// separate Commercial module. Client is currently the only party that needs
// a contract, and standing up a whole new project ahead of that need is the
// kind of premature investment this codebase's own docs already warn against
// elsewhere (see the Build Sequence note in system-architecture.md).
//
// PartyType and a raw PartyId, not a typed ClientId, are what keep a future
// extraction mechanical rather than a redesign: nothing in this file
// references Client internals, so moving this folder into its own module
// once a Subcontractor party type exists is a move, not a rewrite.
public class Contract : OrganizationAggregateRoot<ContractId>
{
    public ContractPartyType PartyType { get; private set; }
    public Guid PartyId { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public ContractStatus Status { get; private set; }
    public DateTime EffectiveFrom { get; private set; }
    public DateTime? EffectiveTo { get; private set; }
    public List<ContractTerm> Terms { get; private set; } = [];

    private Contract() { }

    private Contract(ContractDetails details)
    {
        this.PartyType = details.PartyType;
        this.PartyId = details.PartyId;
        this.Title = details.Title.Trim();
        this.Status = ContractStatus.Draft;
        this.EffectiveFrom = details.EffectiveFrom;
        this.EffectiveTo = details.EffectiveTo;
    }

    public static Contract Create(ContractDetails details)
    {
        if (details.PartyId == Guid.Empty)
            throw new ArgumentException("A contract must belong to a party.", nameof(details));

        var contract = new Contract(details) { Id = new ContractId(Guid.NewGuid()) };
        contract.AddDomainEvent(new ContractCreated(contract.Id.Value, details.PartyType.ToString(), details.PartyId));
        return contract;
    }

    public ContractTerm AddTerm(ContractTermType type, decimal value, DateTime effectiveFrom, DateTime? effectiveTo = null)
    {
        var term = ContractTerm.Create(type, value, effectiveFrom, effectiveTo);
        this.Terms.Add(term);
        return term;
    }

    public void RemoveTerm(ContractTermId termId)
    {
        this.Terms.RemoveAll(t => t.Id == termId);
    }

    public void Activate()
    {
        if (this.Status != ContractStatus.Draft)
            throw new InvalidOperationException("Only a draft contract can be activated.");

        this.Status = ContractStatus.Active;
        AddDomainEvent(new ContractActivated(Id.Value, PartyType.ToString(), PartyId));
    }

    public void Terminate()
    {
        this.Status = ContractStatus.Terminated;
        AddDomainEvent(new ContractTerminated(Id.Value, PartyType.ToString(), PartyId));
    }

    public void MarkExpired()
    {
        if (this.Status == ContractStatus.Active)
            this.Status = ContractStatus.Expired;
    }

    public decimal? GetActiveTermValue(ContractTermType type, DateTime asOf)
    {
        return Terms
            .Where(t => t.Type == type && t.IsActiveOn(asOf))
            .Select(t => (decimal?)t.Value)
            .FirstOrDefault();
    }
}
