

using ErrorOr;
using Microsoft.AspNetCore.Http;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;

namespace Mirama.SharedKernel.Models.Decorators;

public class AuthorizationDecorator<TRequest, TResponse>(
    IRequestHandler<TRequest, TResponse> inner,
    IHttpContextAccessor httpContextAccessor)
    : IRequestHandler<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : IErrorOr
{
    public async Task<TResponse> HandleAsync(TRequest request, CancellationToken ct)
    {
        var context = httpContextAccessor.HttpContext;
        var user = context?.User;

        // Get organizationId from the route if it exists
        if (context?.Request.RouteValues.TryGetValue("organizationId", out var routeOrgId) == true)
        {
            var claimOrgId = user?.FindFirst("organizationId")?.Value;

            if (claimOrgId == null || !string.Equals(claimOrgId, routeOrgId?.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                return (TResponse)(dynamic)Error.Forbidden(
                    code: "Auth.UnauthorizedOrganization",
                    description: "Route organization ID does not match user claims.");
            }
        }

        // Get plantId from the route if it exists
        if (context?.Request.RouteValues.TryGetValue("plantId", out var routePlantId) == true)
        {
            var claimPlantId = user?.FindFirst("plantId")?.Value;
            var claimOrgRole = user?.FindFirst("organizationRole")?.Value;
            var routePlantIdStr = routePlantId?.ToString();

            // If role is plantOwner, the user MUST have a claimPlantId that matches the route param given.
            if (string.Equals(claimOrgRole, "plantOwner", StringComparison.OrdinalIgnoreCase))
            {
                if (string.IsNullOrEmpty(claimPlantId))
                {
                    return (TResponse)(dynamic)Error.Forbidden(
                        code: "Auth.UnauthorizedPlantOwner",
                        description: "As a Plant Owner, you must have a valid claim for this specific plant.");
                }

                var splitPlantId = claimPlantId.Split(",");
                if (splitPlantId.All(x => !string.Equals(x, routePlantIdStr, StringComparison.OrdinalIgnoreCase)))
                {
                    return (TResponse)(dynamic)Error.Forbidden(
                        code: "Auth.UnauthorizedPlantOwner",
                        description: "As a Plant Owner, you must have a valid claim for this specific plant.");
                }
            }

            // If they aren't a plantOwner, but DO have a claimPlantId (e.g., a restricted 'trader'),
            // we still enforce the match.
            else if (!string.IsNullOrEmpty(claimPlantId) &&
                    !string.Equals(claimPlantId, routePlantIdStr, StringComparison.OrdinalIgnoreCase))
            {
                return (TResponse)(dynamic)Error.Forbidden(
                    code: "Auth.UnauthorizedPlant",
                    description: "You do not have access to this specific plant.");
            }
        }

        // Get plantId from the route if it exists
        if (context?.Request.RouteValues.TryGetValue("customerId", out var routeCustomerId) == true)
        {
            var claimCustomerId = user?.FindFirst("customerId")?.Value;
            var claimOrgRole = user?.FindFirst("organizationRole")?.Value;
            var routeCustomerIdStr = routeCustomerId?.ToString();

            // If role is customer, the user MUST have a claimCustomerId that matches the route param given.
            if (string.Equals(claimOrgRole, "customer", StringComparison.OrdinalIgnoreCase))
            {
                if (string.IsNullOrEmpty(claimCustomerId))
                {
                    return (TResponse)(dynamic)Error.Forbidden(
                        code: "Auth.UnauthorizedCustomer",
                        description: "As a customer, you must have a valid claim for this specific customer account.");
                }

                var splitCustomerId = claimCustomerId.Split(",");
                if (splitCustomerId.All(x => !string.Equals(x, routeCustomerIdStr, StringComparison.OrdinalIgnoreCase)))
                {
                    return (TResponse)(dynamic)Error.Forbidden(
                        code: "Auth.UnauthorizedCustomer",
                        description: "As a customer, you must have a valid claim for this specific customer account.");
                }
            }

            // If they aren't a customer, but DO have a claimCustomerId (e.g., a restricted 'trader'),
            // we still enforce the match.
            else if (!string.IsNullOrEmpty(claimCustomerId) &&
                    !string.Equals(claimCustomerId, routeCustomerIdStr, StringComparison.OrdinalIgnoreCase))
            {
                return (TResponse)(dynamic)Error.Forbidden(
                    code: "Auth.UnauthorizedCustomer",
                    description: "You do not have access to this specific customer account.");
            }
        }

        return await inner.HandleAsync(request, ct);
    }
}
