using System.Text.Json.Serialization;
using Mirama.Modules.Identity.Application.Features.V1.AccessControl.Policies;
using Mirama.Modules.Identity.Domain.Aggregates.Policy;
using Mirama.Modules.Identity.Domain.Aggregates.Role;

namespace Mirama.Modules.Identity.Application.Features.V1.AccessControl.Roles.GetRolesWithPolicies;

internal static class RoleWithPoliciesMapper
{
    internal static RoleWithPoliciesResponse MapResponse(this Role role, IReadOnlyDictionary<PolicyId, Policy> policyLookup) => new()
    {
        Id = role.Id.Value,
        Name = role.Name,
        Description = role.Description,
        Scope = role.Scope.ToString(),
        IsSystemRole = role.IsSystemRole,
        Policies = role.Policies
            .Where(policyLookup.ContainsKey)
            .Select(pid => policyLookup[pid].MapResponse())
            .ToList(),
    };
}

public sealed record RoleWithPoliciesResponse
{
    [JsonPropertyName("id")]
    public Guid Id { get; init; }

    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;

    [JsonPropertyName("description")]
    public string? Description { get; init; }

    [JsonPropertyName("scope")]
    public string Scope { get; init; } = string.Empty;

    [JsonPropertyName("isSystemRole")]
    public bool IsSystemRole { get; init; }

    [JsonPropertyName("policies")]
    public List<PolicyResponse> Policies { get; init; } = [];
}
