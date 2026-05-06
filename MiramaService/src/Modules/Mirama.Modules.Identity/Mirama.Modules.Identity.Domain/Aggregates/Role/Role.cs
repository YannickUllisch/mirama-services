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

    private Role(RoleDetails details)
    {
        Name = details.Name.Trim();
        Description = details.Description?.Trim();
        TenantId = details.TenantId;
        Scope = details.Scope;
    }

    private Role() { }

    public static Role Create(RoleDetails details)
    {
        return new Role(details) { Id = new RoleId(Guid.NewGuid()) };
    }

    public void Update(RoleDetails details)
    {
        Name = details.Name.Trim();
        Description = details.Description?.Trim();
    }

    public void AttachPolicy(PolicyId policyId)
    {
        if (!Policies.Contains(policyId))
            Policies.Add(policyId);
    }

    public ErrorOr<Deleted> DetachPolicy(PolicyId policyId)
    {
        if (!Policies.Remove(policyId))
            return Error.NotFound("Role.Policy.NotFound", "Policy not attached to this role.");

        return Result.Deleted;
    }
}
