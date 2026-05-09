using Mirama.SharedKernel.Abstractions.Domain.Core;

namespace Mirama.Modules.Identity.Domain.Aggregates.Policy;

public sealed class PolicyStatement : Entity<PolicyStatementId>
{
    public PolicyId PolicyId { get; private set; } = default!;
    public Effect Effect { get; private set; }
    public string Action { get; private set; } = string.Empty;
    public string Resource { get; private set; } = string.Empty;

    private PolicyStatement() { }

    internal static PolicyStatement Create(PolicyId policyId, string action, string resource, Effect effect)
    {
        return new PolicyStatement
        {
            Id = new PolicyStatementId(Guid.NewGuid()),
            PolicyId = policyId,
            Action = action,
            Resource = resource,
            Effect = effect
        };
    }
}
