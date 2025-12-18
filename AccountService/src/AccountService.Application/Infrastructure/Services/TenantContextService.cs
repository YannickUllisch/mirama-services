
using System.Security.Claims;
using AccountService.Application.Infrastructure.Common.Interfaces;
using Microsoft.AspNetCore.Http;

namespace AccountService.Application.Infrastructure.Services;

internal class TenantContextService(IHttpContextAccessor httpContextAccessor) : ITenantContextService
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public string UserId => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier)!;

    public Guid OrganizationId
    {
        get
        {
            var claim = _httpContextAccessor.HttpContext?.User?.FindFirst("org_id")?.Value;
            if (string.IsNullOrEmpty(claim)) throw new UnauthorizedAccessException("Tenant not found");
            return Guid.Parse(claim);
        }
    }
}