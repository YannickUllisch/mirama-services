
using System.Linq.Expressions;
using AccountService.Application.Domain.Abstractions.Core;
using AccountService.Application.Domain.Aggregates.Organization;
using Microsoft.EntityFrameworkCore;

namespace AccountService.Application.Common.Extensions;

internal static class ModelBuilderExtensions
{
    /// <summary>
    /// Applies global query filters for all ITenantOwned and IOrganizationOwned entities
    /// if corresponding IDs are provided.
    /// </summary>
    public static void ApplyGlobalFilters(this ModelBuilder modelBuilder, Guid? tenantId, Guid? organizationId)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var clrType = entityType.ClrType;

            // Tenant filter
            if (tenantId.HasValue && typeof(ITenantOwned).IsAssignableFrom(clrType))
            {
                var parameter = Expression.Parameter(clrType, "e");
                var property = Expression.Property(parameter, nameof(ITenantOwned.TenantId));
                var tenantValue = Expression.Constant(tenantId.Value);
                var body = Expression.Equal(property, tenantValue);
                var lambda = Expression.Lambda(body, parameter);

                entityType.SetQueryFilter(lambda);
            }

            // Organization filter
            if (organizationId.HasValue && typeof(IOrganizationOwned).IsAssignableFrom(clrType))
            {
                var parameter = Expression.Parameter(clrType, "e");
                var orgProperty = Expression.Property(parameter, nameof(IOrganizationOwned.OrganizationId));

                // Extract the underlying Guid from OrganizationId wrapper
                var valueProperty = Expression.Property(orgProperty, nameof(OrganizationId.Value));

                var orgIdValue = Expression.Constant(organizationId.Value);
                var body = Expression.Equal(valueProperty, orgIdValue);

                var lambda = Expression.Lambda(body, parameter);
                entityType.SetQueryFilter(lambda);
            }
        }
    }
}
