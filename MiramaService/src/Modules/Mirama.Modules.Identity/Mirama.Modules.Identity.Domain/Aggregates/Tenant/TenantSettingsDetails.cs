

namespace Mirama.Modules.Identity.Domain.Aggregates.Tenant;

public sealed record TenantSettingsDetails
(
    string Name,
    bool ReceiveNotifications,
    string? BrandingColor, 
    string? LogoUrl,
    string Timezone = "UTC"
);