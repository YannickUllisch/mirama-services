
using AccountService.Application.Domain.Abstractions.Core;
using AccountService.Application.Domain.Abstractions.Tenant;

namespace AccountService.Application.Domain.Aggregates.Tenant;

public sealed class TenantSettings : ValueObject
{
    public string Name { get; private set;} = string.Empty;

    public int QuotaTeams { get; private set; } = 3;

    public int QuotaUsers { get; private set; } = 3;

    public int QuotaOrganizations { get; private set; } = 3;

    public Guid TenantId => throw new NotImplementedException();

    public void SetTenantId(Guid tenantId)
    {
        throw new NotImplementedException();
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        throw new NotImplementedException();
    }
}