using Mirama.SharedKernel.Abstractions.Domain.Core;

namespace Mirama.Modules.Clients.Domain.Aggregates.Contract;

// A plain Entity, not an OrganizationEntity, it is only ever reached through
// its owning Contract, the same way TenantSettings sits under Tenant in the
// Identity module. It does not need its own organization scoping, it inherits
// that through the aggregate root that owns it.
public class ContractTerm : Entity<ContractTermId>
{
    public ContractTermType Type { get; private set; }
    public decimal Value { get; private set; }
    public DateTime EffectiveFrom { get; private set; }
    public DateTime? EffectiveTo { get; private set; }

    private ContractTerm() { }

    private ContractTerm(ContractTermType type, decimal value, DateTime effectiveFrom, DateTime? effectiveTo)
    {
        this.Type = type;
        this.Value = value;
        this.EffectiveFrom = effectiveFrom;
        this.EffectiveTo = effectiveTo;
    }

    public static ContractTerm Create(ContractTermType type, decimal value, DateTime effectiveFrom, DateTime? effectiveTo = null)
    {
        if (effectiveTo.HasValue && effectiveTo.Value <= effectiveFrom)
            throw new ArgumentException("EffectiveTo must be after EffectiveFrom.", nameof(effectiveTo));

        return new ContractTerm(type, value, effectiveFrom, effectiveTo)
        {
            Id = new ContractTermId(Guid.NewGuid())
        };
    }

    public bool IsActiveOn(DateTime date) =>
        date >= EffectiveFrom && (!EffectiveTo.HasValue || date <= EffectiveTo.Value);
}
