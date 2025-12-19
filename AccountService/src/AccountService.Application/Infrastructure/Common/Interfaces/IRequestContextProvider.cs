
namespace AccountService.Application.Infrastructure.Common.Interfaces;

public interface IRequestContextProvider
{
    /// <summary>
    /// UserId of authenticated user
    /// </summary>
    Guid UserId { get; }

    /// <summary>
    /// Tenant Id extracted from Claims, always has to be part of the JWT. If OrganizationId is defined
    /// then Tenant Id is dependent on what Organization is currently set as active in the JWT. Otherwise the base user TenantId is assumed
    /// </summary>
    Guid TenantId { get; }

    /// <summary>
    /// Organization Id extracted from Claims. Optional since we potentially have endpoints that are purely Tenant based 
    /// and not related to Organization
    /// </summary>
    Guid? OrganizationId { get; }
}