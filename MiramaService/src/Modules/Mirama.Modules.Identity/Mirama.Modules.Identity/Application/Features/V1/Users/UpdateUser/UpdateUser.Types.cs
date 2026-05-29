using System.Text.Json.Serialization;
using ErrorOr;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;

namespace Mirama.Modules.Identity.Application.Features.V1.Users.UpdateUser;

public sealed record UpdateUserCommand : ICommand<ErrorOr<UserResponse>>
{
    [JsonPropertyName("id")]
    public Guid Id { get; init; }

    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;

    [JsonPropertyName("email")]
    public string Email { get; init; } = string.Empty;

    [JsonPropertyName("image")]
    public string? Image { get; init; }

    [JsonPropertyName("role")]
    public string Role { get; init; } = string.Empty;
}
