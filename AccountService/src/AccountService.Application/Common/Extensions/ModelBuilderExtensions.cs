
using System.Linq.Expressions;
using AccountService.Application.Domain.Abstractions.Core;
using Microsoft.EntityFrameworkCore;

namespace AccountService.Application.Common.Extensions;

internal static class ModelBuilderExtensions
{
    /// <summary>
    /// Extension on Model builder for DB context which applies automatic Global Query Filter based on OrganizationId
    /// on all IOrganizationOwned Entities.
    /// </summary>
    /// <param name="modelBuilder">Class to extend</param>
    /// <param name="tenantContext">Context to provide currently active Organization Id for filter condition</param>
    /// <returns></returns>
    /// <summary>
    public static ModelBuilder ApplyTenantQueryFilter(this ModelBuilder modelBuilder, Guid tenantId)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (!typeof(ITenantOwned).IsAssignableFrom(entityType.ClrType))
                continue;

            var parameter = Expression.Parameter(entityType.ClrType, "e");

            var property = Expression.Property(parameter, nameof(ITenantOwned.TenantId));
            var tenantValue = Expression.Constant(tenantId);

            var body = Expression.Equal(property, tenantValue);

            var lambda = Expression.Lambda(body, parameter);

            entityType.SetQueryFilter(lambda);
        }

        return modelBuilder;
    }

    /// <summary>
    /// Extension on Model builder for DB context which applies automatic Global Query Filter based on TenantId
    /// on all ITenantOwned Entities.
    /// </summary>
    /// <param name="modelBuilder">Class to extend</param>
    /// <param name="tenantContext">Context to provide currently active Tenant Id for filter condition</param>
    /// <returns></returns>
    /// <summary>
    public static ModelBuilder ApplyOrganizationQueryFilter(this ModelBuilder modelBuilder, Guid orgId)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (!typeof(IOrganizationOwned).IsAssignableFrom(entityType.ClrType))
                continue;

            var parameter = Expression.Parameter(entityType.ClrType, "e");
            var orgIdProperty = Expression.Property(parameter, nameof(IOrganizationOwned.OrganizationId));
            var valueProperty = Expression.Property(orgIdProperty, "Value");
            var orgIdValue = Expression.Constant(orgId);

            var body = Expression.Equal(valueProperty, orgIdValue);
            var lambda = Expression.Lambda(body, parameter);

            entityType.SetQueryFilter(lambda);
        }

        return modelBuilder;
    }
}
