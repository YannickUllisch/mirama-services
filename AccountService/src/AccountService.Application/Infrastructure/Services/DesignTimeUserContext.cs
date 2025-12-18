
using AccountService.Application.Infrastructure.Common.Interfaces;

namespace AccountService.Application.Infrastructure.Services;

internal class DesignTimeUserContextService : IUserContextService
{
    public string? UserId => "Migration";
}