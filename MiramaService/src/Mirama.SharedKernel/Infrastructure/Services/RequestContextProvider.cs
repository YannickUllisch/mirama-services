using Microsoft.AspNetCore.Http;
using Mirama.SharedKernel.Abstractions.Persistence;

namespace Mirama.SharedKernel.Infrastructure.Services;

internal class RequestContextProvider(
    IHttpContextAccessor httpContextAccessor,
    IAmbientMessageContext ambientContext) : IRequestContextProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly IAmbientMessageContext _ambientContext = ambientContext;

    public Guid ExternalUserId
    {
        get
        {
            var claim = _httpContextAccessor.HttpContext?.User?.FindFirst("sub")?.Value;
            if (string.IsNullOrEmpty(claim)) throw new UnauthorizedAccessException("ExternalUserId not found");
            return Guid.Parse(claim);
        }
    }

    public Guid UserId
    {
        get
        {
            var claim = _httpContextAccessor.HttpContext?.User?.FindFirst("userId")?.Value;
            if (string.IsNullOrEmpty(claim)) throw new UnauthorizedAccessException("UserId not found");
            return Guid.Parse(claim);
        }
    }

    public Guid? TenantId
    {
        get
        {
            if (_httpContextAccessor.HttpContext is null) return _ambientContext.TenantId;

            var claim = _httpContextAccessor.HttpContext.User?.FindFirst("tenantId")?.Value;
            return Guid.TryParse(claim, out var guid) ? guid : (Guid?)null;
        }
    }

    public Guid? OrganizationId
    {
        get
        {
            if (_httpContextAccessor.HttpContext is null) return _ambientContext.OrganizationId;

            var claim = _httpContextAccessor.HttpContext.User?.FindFirst("organizationId")?.Value;
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
