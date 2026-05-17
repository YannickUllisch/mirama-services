using System.Text.Json.Serialization;

namespace Mirama.Modules.Identity.Application.Features.V1.AccessControl.Policies;

public sealed record AvailablePermissionsResponse
{
    [JsonPropertyName("effects")]
    public List<string> Effects { get; init; } = [];

    [JsonPropertyName("groups")]
    public List<PermissionGroupResponse> Groups { get; init; } = [];
}

public sealed record PermissionGroupResponse
{
    [JsonPropertyName("label")]
    public string Label { get; init; } = string.Empty;

    [JsonPropertyName("scope")]
    public string Scope { get; init; } = string.Empty;

    [JsonPropertyName("resourcePattern")]
    public string ResourcePattern { get; init; } = string.Empty;

    [JsonPropertyName("allActionsPattern")]
    public string AllActionsPattern { get; init; } = string.Empty;

    [JsonPropertyName("actions")]
    public List<PermissionActionResponse> Actions { get; init; } = [];
}

public sealed record PermissionActionResponse
{
    [JsonPropertyName("action")]
    public string Action { get; init; } = string.Empty;

    [JsonPropertyName("label")]
    public string Label { get; init; } = string.Empty;
}
