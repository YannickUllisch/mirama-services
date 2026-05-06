using Mirama.SharedKernel.Abstractions.Domain.Core;

namespace Mirama.Modules.Identity.Domain.Aggregates.Tenant;

public sealed class TenantSettings : Entity<TenantSettingsId>
{
    public Guid TenantId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public bool IsActive { get; private set; } = true;
    public string Timezone { get; private set; } = "UTC";
    public string? BrandingColor { get; private set; }
    public string? LogoUrl { get; private set; }
    public bool ReceiveNotifications { get; private set; } = true;

    private TenantSettings() { }

    internal static TenantSettings Create(Guid tenantId, string name, string? brandingColor, string? logoUrl, bool receiveNotifications, string timezone = "UTC")
    {
        return new TenantSettings
        {
            Id = new TenantSettingsId(Guid.NewGuid()),
            TenantId = tenantId,
            Name = name.Trim(),
            IsActive = true,
            Timezone = timezone,
            BrandingColor = brandingColor,
            LogoUrl = logoUrl,
            ReceiveNotifications = receiveNotifications
        };
    }

    public void Update(string name, string? brandingColor, string? logoUrl, bool receiveNotifications, string timezone)
    {
        Name = name.Trim();
        BrandingColor = brandingColor;
        LogoUrl = logoUrl;
        ReceiveNotifications = receiveNotifications;
        Timezone = timezone;
    }

    public void SetActive(bool isActive)
    {
        IsActive = isActive;
    }
}
