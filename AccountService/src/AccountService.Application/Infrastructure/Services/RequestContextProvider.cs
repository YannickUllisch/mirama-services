
using System.Security.Claims;
using AccountService.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;

namespace AccountService.Application.Infrastructure.Services;

internal class RequestContextProvider(IHttpContextAccessor httpContextAccessor) : IRequestContextProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public Guid UserId
    {
        get
        {
            var claim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(claim)) throw new UnauthorizedAccessException("UserId not found");
            return Guid.Parse(claim);
        }
    }

    public Guid? OrganizationId
    {
        get
        {
            var claim = _httpContextAccessor.HttpContext?.User?.FindFirst("org_id")?.Value;
            return Guid.TryParse(claim, out var guid) ? guid : (Guid?)null;
        }
    }

    public Guid TenantId
    {
        get
        {
            var claim = _httpContextAccessor.HttpContext?.User?.FindFirst("tenant_id")?.Value;
            if (string.IsNullOrEmpty(claim)) throw new UnauthorizedAccessException("TenantId not found");
            return Guid.Parse(claim);
        }
    }
}