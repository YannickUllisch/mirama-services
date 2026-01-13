

using System.Security.Cryptography.X509Certificates;
using AuthService.Application.Common.Options;
using AuthService.Application.Domain.Scopes;
using AuthService.Application.Infrastructure.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AuthService.Application.Infrastructure.OpenIddict;

public static class OpenIddictConfiguration
{
    public static OpenIddictServerBuilder ConfigureServer(
        this OpenIddictServerBuilder options, IConfiguration config, IHostEnvironment environment)
    {
        var openidconfig = config.GetSection(OpenIddictOptions.Key).Get<OpenIddictOptions>()
            ?? throw new InvalidOperationException("OpenIddict option configuration is missing or invalid.");

        options.SetAccessTokenLifetime(TimeSpan.FromMinutes(15));
        options.SetRefreshTokenLifetime(TimeSpan.FromDays(30));

        options.RegisterScopes(
            ScopeType.OpenId,
            ScopeType.Roles,
            ScopeType.OfflineAccess,
            ScopeType.Profile,
            ScopeType.Email,
            ScopeType.Tenant,
            ScopeType.Organization,
            ScopeType.AccountWrite,
            ScopeType.AccountRead,
            ScopeType.ProjectWrite,
            ScopeType.ProjectRead,
            ScopeType.LLMWrite,
            ScopeType.LLMRead);

        options.RegisterAudiences(
                ResourceType.Account,
                ResourceType.Project,
                ResourceType.LLM);

        // Issuer refers to this Auth Server
        options.SetIssuer(new Uri(openidconfig.Issuer));

        if (environment.IsDevelopment())
        {
            CertificateManager.GenerateDevCertificate(
                openidconfig.SigningCertificatePath,
                "CN=OpenIddict Signing Dev",
                X509KeyUsageFlags.DigitalSignature,
                openidconfig.SigningCertificatePassword);

            CertificateManager.GenerateDevCertificate(
                openidconfig.EncryptionCertificatePath,
                "CN=OpenIddict Encryption Dev",
                X509KeyUsageFlags.KeyEncipherment,
                openidconfig.EncryptionCertificatePassword);

            var signingCert = CertificateManager.GetCertificateFile(
                openidconfig.SigningCertificatePath,
                openidconfig.SigningCertificatePassword);

            var encryptionCert = CertificateManager.GetCertificateFile(
                openidconfig.EncryptionCertificatePath,
                openidconfig.EncryptionCertificatePassword);
                
            options.AddSigningCertificate(signingCert)
                    .AddEncryptionCertificate(encryptionCert);

            // options.AddDevelopmentSigningCertificate()
            //        .AddDevelopmentEncryptionCertificate();
        }
        
        // TODO: Add Certificates for prod environment
        return options;
    }
}
