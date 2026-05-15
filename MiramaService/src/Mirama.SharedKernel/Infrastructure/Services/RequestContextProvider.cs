using Microsoft.AspNetCore.Http;
using Mirama.SharedKernel.Abstractions.Persistence;

namespace Mirama.SharedKernel.Infrastructure.Services;

internal class RequestContextProvider(IHttpContextAccessor httpContextAccessor) : IRequestContextProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public Guid UserId
    {
        get
        {
            var claim = _httpContextAccessor.HttpContext?.User?.FindFirst("sub")?.Value;
            if (string.IsNullOrEmpty(claim)) throw new UnauthorizedAccessException("UserId not found");
            return Guid.Parse(claim);
        }
    }

    public Guid? TenantId
    {
        get
        {
            var claim = _httpContextAccessor.HttpContext?.User?.FindFirst("tenantId")?.Value;
            return Guid.TryParse(claim, out var guid) ? guid : (Guid?)null;
        }
    }

    public Guid? OrganizationId
    {
        get
        {
            var claim = _httpContextAccessor.HttpContext?.User?.FindFirst("organizationId")?.Value;
            return Guid.TryParse(claim, out var guid) ? guid : (Guid?)null;
        }
    }

    public Guid? ProjectId
    {
        get
        {
            var raw = _httpContextAccessor.HttpContext?.Request.RouteValues["projectId"] as string;
            return Guid.TryParse(raw, out var guid) ? guid : (Guid?)null;
        }
    }
}