
namespace Mirama.SharedKernel.Abstractions.Persistence;

public interface IRequestContextProvider
{
    Guid UserId { get; }

    /// <summary>
    /// Tenant Id from "tid" claim. If OrganizationId is set, reflects the org's tenant.
    /// </summary>
    Guid? TenantId { get; }

    /// <summary>
    /// Organization Id from "oid" claim. Null for purely tenant-scoped endpoints.
    /// </summary>
    Guid? OrganizationId { get; }

    /// <summary>
    /// Project Id from "projectId" route value. Null when not on a project-scoped route.
    /// </summary>
    Guid? ProjectId { get; }
}