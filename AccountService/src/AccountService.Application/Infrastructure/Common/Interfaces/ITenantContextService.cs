
namespace AccountService.Application.Infrastructure.Common.Interfaces;

public interface ITenantContextService
{
    Guid OrganizationId { get; }
}
