using System.Text.Json.Serialization;
using ErrorOr;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;

namespace Mirama.Modules.Identity.Application.Features.V1.AccessControl.Policies.CreatePolicy;

public sealed record CreatePolicyCommand : ICommand<ErrorOr<PolicyResponse>>
{
    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;

    [JsonPropertyName("description")]
    public string? Description { get; init; }

    [JsonPropertyName("scope")]
    public string Scope { get; init; } = string.Empty;

    [JsonPropertyName("statements")]
    public List<CreatePolicyStatementDto> Statements { get; init; } = [];
}

public sealed record CreatePolicyStatementDto
{
    [JsonPropertyName("action")]
    public string Action { get; init; } = string.Empty;

    [JsonPropertyName("resource")]
    public string Resource { get; init; } = "*";

    [JsonPropertyName("effect")]
    public string Effect { get; init; } = "Allow";
}
