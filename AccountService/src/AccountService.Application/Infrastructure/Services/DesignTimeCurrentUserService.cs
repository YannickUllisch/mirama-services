

using AccountService.Application.Common.Interfaces;

namespace AccountService.Application.Infrastructure.Services;

internal class DesignTimeCurrentUserService : ICurrentUserService
{
    public string? UserId => "Migration";
}