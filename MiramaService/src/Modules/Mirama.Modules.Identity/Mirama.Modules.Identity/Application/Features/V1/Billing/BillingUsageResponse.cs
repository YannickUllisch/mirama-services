using System.Text.Json.Serialization;

namespace Mirama.Modules.Identity.Application.Features.V1.Billing;

public sealed record BillingUsageResponse
{
    [JsonPropertyName("organizations")]
    public int Organizations { get; init; }

    [JsonPropertyName("members")]
    public int Members { get; init; }

    [JsonPropertyName("projects")]
    public int Projects { get; init; }
}
