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

    public bool IsSystemRole => this.TenantId is null;

    public List<PolicyId> Policies { get; private set; } = [];

    private Role(RoleDetails details)
    {
        this.Name = details.Name.Trim();
        this.Description = details.Description?.Trim();
        this.TenantId = details.TenantId;
        this.Scope = details.Scope;
    }

    private Role() { }

    public static Role Create(RoleDetails details)
        => new Role(details) { Id = new RoleId(Guid.NewGuid()) };

    public void Update(RoleDetails details)
    {
        this.Name = details.Name.Trim();
        this.Description = details.Description?.Trim();
    }

    public ErrorOr<Success> AttachPolicy(PolicyId policyId, AccessScope policyScope)
    {
        if (policyScope != this.Scope)
            return Error.Validation("Role.Policy.ScopeMismatch",
                $"Policy scope '{policyScope}' does not match role scope '{this.Scope}'.");
        if (this.Policies.Contains(policyId))
            return Error.Conflict("Role.Policy.Duplicate", "Policy already attached to this role.");
        this.Policies.Add(policyId);
        return Result.Success;
    }

    public ErrorOr<Deleted> DetachPolicy(PolicyId policyId)
    {
        if (!this.Policies.Remove(policyId))
            return Error.NotFound("Role.Policy.NotFound", "Policy not attached to this role.");
        return Result.Deleted;
    }
}
