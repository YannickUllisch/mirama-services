using System.Text.Json.Serialization;
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


public sealed record PolicyResponse
{
    [JsonPropertyName("id")]
    public Guid Id { get; init; }

    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;

    [JsonPropertyName("description")]
    public string? Description { get; init; }

    [JsonPropertyName("scope")]
    public string Scope { get; init; } = string.Empty;

    [JsonPropertyName("isManaged")]
    public bool IsManaged { get; init; }

    [JsonPropertyName("isSystemPolicy")]
    public bool IsSystemPolicy { get; init; }

    [JsonPropertyName("statements")]
    public List<PolicyStatementResponse> Statements { get; init; } = [];
}

public sealed record PolicyStatementResponse
{
    [JsonPropertyName("id")]
    public Guid Id { get; init; }

    [JsonPropertyName("action")]
    public string Action { get; init; } = string.Empty;

    [JsonPropertyName("resource")]
    public string Resource { get; init; } = string.Empty;

    [JsonPropertyName("effect")]
    public string Effect { get; init; } = string.Empty;
}
