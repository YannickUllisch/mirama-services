
using System.Security.Claims;
using AuthService.Application.Common.Interfaces;
using AuthService.Application.Common.Options;
using AuthService.Application.Infrastructure.OpenIddict;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Server;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace AuthService.Application.Services;

public sealed class InternalTokenService(
    OpenIddictServerDispatcher applicationManager,
    IOptions<OpenIddictOptions> options) 
    : IInternalTokenService
{
    private readonly OpenIddictServerDispatcher _dispatcher = applicationManager;

    private readonly IOptions<OpenIddictOptions> _options = options;

    public string IssueInternalAccessToken(List<string> scopes, string audience)
    {
        var claims = scopes.Select(a => new Claim(Claims.Scope, a)).ToList();
        var claimsIdentity = new ClaimsIdentity(claims);

        var signingCertificate = CertificateManager.GetCertificateFile(
                _options.Value.SigningCertificatePath,
                _options.Value.SigningCertificatePassword);

        var signingCredentials = new SigningCredentials(
             new X509SecurityKey(signingCertificate),
            SecurityAlgorithms.RsaSha256);

        var descriptor = new SecurityTokenDescriptor
        {
            Claims = claims.ToDictionary(a => a.Type, a => (object)a.Value),
            Expires = DateTime.UtcNow.AddMinutes(5),
            IssuedAt = DateTime.UtcNow,
            Issuer = _options.Value.Issuer,
            SigningCredentials = signingCredentials,
            Subject = claimsIdentity,
            TokenType = JsonWebTokenTypes.AccessToken,
            Audience = audience,
        };
        var token = new JsonWebTokenHandler().CreateToken(descriptor);

        return token;
    }   
}