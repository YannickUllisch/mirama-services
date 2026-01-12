
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AuthService.Application.Infrastructure.HttpServices.Contracts.Requests;

public sealed record CreateUser
{
    [Required]
    [JsonPropertyName("providerId")]
    public string ProviderId { get; init; } = string.Empty;

    [Required]
    [JsonPropertyName("provider")]
    public string Provider { get; init; } = string.Empty;

    [Required]
    [JsonPropertyName("email")]
    public string Email { get; init; } = string.Empty;

    [Required]
    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;

    [Required]
    [JsonPropertyName("emailVerified")]
    public bool EmailVerified { get; init; }

    [JsonPropertyName("image")]
    public string? Image { get; init; } = string.Empty;
}