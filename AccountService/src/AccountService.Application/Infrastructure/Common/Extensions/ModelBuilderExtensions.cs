
using System.Linq.Expressions;
using AccountService.Application.Domain.Abstractions.Tenant;
using AccountService.Application.Infrastructure.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AccountService.Application.Infrastructure.Common.Extensions;

internal static class ModelBuilderExtensions
{
    /// <summary>
    /// Extension on Model builder for DB context which applies automatic Global Query Filter based on OrganizationId
    /// on all ITenantScoped Entities.
    /// </summary>
    /// <param name="modelBuilder">Class to extend</param>
    /// <param name="tenantContext">Context to provide currently active Organization/Tenant ID for filter condition</param>
    /// <returns></returns>
    /// <summary>
    public static ModelBuilder ApplyTenantQueryFilter(this ModelBuilder modelBuilder, ITenantContextService tenantContext)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (!typeof(ITenantScoped).IsAssignableFrom(entityType.ClrType))
                continue;

            var parameter = Expression.Parameter(entityType.ClrType, "e");

            var property = Expression.Property(parameter, nameof(ITenantScoped.OrganizationId));
            var tenantValue = Expression.Constant(tenantContext.OrganizationId);

            var body = Expression.Equal(property, tenantValue);

            var lambda = Expression.Lambda(body, parameter);

            entityType.SetQueryFilter(lambda);
        }

        return modelBuilder;
    }
}
