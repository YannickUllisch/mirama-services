using System.Text.Json.Serialization;
using ErrorOr;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;

namespace Mirama.Modules.Identity.Application.Features.V1.Tenants.UpdateTenantSettings;

public sealed record UpdateTenantSettingsCommand : ICommand<ErrorOr<TenantResponse>>
{
    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;

    [JsonPropertyName("timezone")]
    public string Timezone { get; init; } = "UTC";

    [JsonPropertyName("brandingColor")]
    public string? BrandingColor { get; init; }

    [JsonPropertyName("logoUrl")]
    public string? LogoUrl { get; init; }

    [JsonPropertyName("receiveNotifications")]
    public bool ReceiveNotifications { get; init; } = true;
}
