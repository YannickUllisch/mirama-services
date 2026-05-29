
namespace Mirama.SharedKernel.Abstractions.Domain.Core;

public interface ITenantOwned
{
    Guid TenantId { get; }

    void SetTenantId(Guid tenantId);
}