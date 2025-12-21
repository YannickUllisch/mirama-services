

using System.Text.Json.Serialization;

namespace AccountService.Application.Features.Users;

public sealed record UserResponse
{
    [JsonPropertyName("id")]
    public Guid Id { get; init; }

    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;

    [JsonPropertyName("email")]
    public string Email { get; init; } = string.Empty;

    [JsonPropertyName("image")]
    public string? Image { get; init; } = null;

    [JsonPropertyName("role")]
    public string Role { get; init; } = string.Empty;

    [JsonPropertyName("contactEmail")]
    public string ContactEmail { get; init; } = string.Empty;

    [JsonPropertyName("contactPhoneNumber")]
    public string ContactPhoneNumber { get; init; } = string.Empty;
}