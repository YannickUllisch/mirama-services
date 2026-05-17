using System.Text.Json.Serialization;
using Mirama.Modules.Identity.Domain.Aggregates.Role;

namespace Mirama.Modules.Identity.Application.Features.V1.AccessControl.Roles;

internal static class RoleMapper
{
    internal static RoleResponse MapResponse(this Role role) => new()
    {
        Id = role.Id.Value,
        Name = role.Name,
        Description = role.Description,
        Scope = role.Scope.ToString(),
        IsSystemRole = role.IsSystemRole,
        PolicyIds = role.Policies.ConvertAll(p => p.Value),
    };
}


public sealed record RoleResponse
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

    [JsonPropertyName("policyIds")]
    public List<Guid> PolicyIds { get; init; } = [];
}
