using Mirama.Modules.Identity.Domain.Aggregates.Policy;

namespace Mirama.Modules.Identity.Application.Features.V1.AccessControl.Policies;

internal static class PolicyMapper
{
    internal static PolicyResponse MapResponse(this Policy policy) => new()
    {
        Id = policy.Id.Value,
        Name = policy.Name,
        Description = policy.Description,
        Scope = policy.Scope.ToString(),
        IsManaged = policy.IsManaged,
        IsSystemPolicy = policy.TenantId is null,
        Statements = policy.Statements.ConvertAll(s => new PolicyStatementResponse
        {
            Id = s.Id.Value,
            Action = s.Action,
            Resource = s.Resource,
            Effect = s.Effect.ToString(),
        }),
    };
}
