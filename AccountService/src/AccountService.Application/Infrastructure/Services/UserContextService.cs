
using System.Security.Claims;
using AccountService.Application.Infrastructure.Common.Interfaces;
using Microsoft.AspNetCore.Http;

namespace AccountService.Application.Infrastructure.Services;

internal class UserContextService(IHttpContextAccessor httpContextAccessor) : IUserContextService
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public string UserId => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier)!;
}