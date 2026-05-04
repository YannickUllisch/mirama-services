
namespace Mirama.Modules.Identity.Domain.Abstractions.Core;

public interface ITenantOwned
{
    Guid TenantId { get; }

    void SetTenantId(Guid tenantId);
}