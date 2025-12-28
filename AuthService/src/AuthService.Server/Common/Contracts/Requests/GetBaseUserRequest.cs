
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AuthService.Server.Common.Contracts.Requests;

public sealed record GetBaseUserRequest
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
}