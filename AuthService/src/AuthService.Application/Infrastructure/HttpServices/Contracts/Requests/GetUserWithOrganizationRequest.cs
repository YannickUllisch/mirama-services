
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AuthService.Application.Infrastructure.HttpServices.Contracts.Requests;

public sealed record GetUserWithOrganizationRequest
{
    [Required]
    [JsonPropertyName("providerId")]
    public string ProviderId { get; init; } = string.Empty;

    [Required]
    [JsonPropertyName("provider")]
    public string Provider { get; init; } = string.Empty;

    [Required]
    [JsonPropertyName("organizationId")]
    public Guid OrganizationId { get; init; } = Guid.Empty;

    [Required]
    [JsonPropertyName("email")]
    public string Email { get; init; } = string.Empty;
}