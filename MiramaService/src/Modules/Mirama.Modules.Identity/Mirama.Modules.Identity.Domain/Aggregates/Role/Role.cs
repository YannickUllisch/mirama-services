using ErrorOr;
using Mirama.Modules.Identity.Domain.Aggregates.Policy;
using Mirama.Modules.Identity.Domain.Enums;
using Mirama.SharedKernel.Abstractions.Domain.Core;

namespace Mirama.Modules.Identity.Domain.Aggregates.Role;

public sealed class Role : AggregateRoot<RoleId>
{
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public Guid? TenantId { get; private set; }
    public AccessScope Scope { get; private set; }

    public bool IsSystemRole => TenantId is null;

    public List<PolicyId> Policies { get; private set; } = [];

    private Role() { }

    public static Role Create(string name, string? description, Guid? tenantId, AccessScope scope)
    {
        return new Role
        {
            Id = new RoleId(Guid.NewGuid()),
            Name = name.Trim(),
            Description = description?.Trim(),
            TenantId = tenantId,
            Scope = scope
        };
    }

    public void AttachPolicy(PolicyId policyId)
    {
        if (!this.Policies.Contains(policyId))
        {
            this.Policies.Add(policyId);
        }
    }

    public ErrorOr<Deleted> DetachPolicy(PolicyId policyId)
    {
        if (!this.Policies.Remove(policyId))
        {
            return Error.NotFound("Role.Policy.NotFound", "Policy not attached to this role.");
        }

        return Result.Deleted;
    }

    public void Update(string name, string? description)
    {
        this.Name = name.Trim();
        this.Description = description?.Trim();
    }
}
