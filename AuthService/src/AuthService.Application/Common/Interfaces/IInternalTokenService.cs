
using Microsoft.AspNetCore.Http;

namespace AuthService.Application.Common.Interfaces;

public interface IInternalTokenService
{
    string IssueInternalAccessToken(List<string> scopes, string audience);
}
