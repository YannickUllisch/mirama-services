
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Mirama.Modules.Identity.Application.Common.Interfaces;

namespace Mirama.Modules.Identity.Infrastructure.Services;

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
            var claim = _httpContextAccessor.HttpContext?.User?.FindFirst("oid")?.Value;
            return Guid.TryParse(claim, out var guid) ? guid : (Guid?)null;
        }
    }

    public Guid? TenantId
    {
        get
        {
            var claim = _httpContextAccessor.HttpContext?.User?.FindFirst("tid")?.Value;
            return Guid.TryParse(claim, out var guid) ? guid : (Guid?)null;
        }
    }
}