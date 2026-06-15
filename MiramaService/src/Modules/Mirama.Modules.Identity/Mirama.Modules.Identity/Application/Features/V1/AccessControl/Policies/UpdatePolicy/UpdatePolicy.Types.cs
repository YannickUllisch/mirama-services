using System.Text.Json.Serialization;
using ErrorOr;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;

namespace Mirama.Modules.Identity.Application.Features.V1.AccessControl.Policies.UpdatePolicy;

public sealed record UpdatePolicyCommand : ICommand<ErrorOr<PolicyResponse>>
{
    [JsonPropertyName("id")]
    public Guid Id { get; init; }

    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;

    [JsonPropertyName("description")]
    public string? Description { get; init; }

    [JsonPropertyName("addStatements")]
    public IReadOnlyList<StatementDto> AddStatements { get; init; } = [];

    [JsonPropertyName("removeStatementIds")]
    public IReadOnlyList<Guid> RemoveStatementIds { get; init; } = [];
}

public sealed record StatementDto(
    [property: JsonPropertyName("action")] string Action,
    [property: JsonPropertyName("resource")] string Resource = "*",
    [property: JsonPropertyName("effect")] string Effect = "Allow");
