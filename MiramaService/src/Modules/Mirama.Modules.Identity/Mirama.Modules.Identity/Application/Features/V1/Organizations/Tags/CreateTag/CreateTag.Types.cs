using System.Text.Json.Serialization;
using ErrorOr;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;

namespace Mirama.Modules.Identity.Application.Features.V1.Organizations.Tags.CreateTag;

public sealed record CreateTagCommand : ICommand<ErrorOr<TagResponse>>
{
    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;

    [JsonPropertyName("color")]
    public string? Color { get; init; }

    [JsonPropertyName("description")]
    public string? Description { get; init; }

    [JsonPropertyName("scope")]
    public int Scope { get; init; }
}
