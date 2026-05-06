namespace Mirama.Modules.Identity.Domain.Aggregates.User;

public sealed record UserDetails(
    string Name,
    string Email,
    TenantRole Role = TenantRole.User,
    string? Image = null
);
