
using System.Security.Claims;
using AccountService.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;

namespace AccountService.Application.Infrastructure.Services;

internal class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public string UserId => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier)!;
}