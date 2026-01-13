
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace AuthService.Application.Infrastructure.OpenIddict;

public static class CertificateManager
{
    // Based on https://documentation.openiddict.com/configuration/encryption-and-signing-credentials
    public static void GenerateDevCertificate(string path, string sub, X509KeyUsageFlags usageFlags, string password)
    {
        if (File.Exists(path))
        {
            return;
        }

        Directory.CreateDirectory(Path.GetDirectoryName(path)!);

        using var algorithm = RSA.Create(keySizeInBits: 2048);

        var subject = new X500DistinguishedName(sub);
        var request = new CertificateRequest(
            subject,
            algorithm,
            HashAlgorithmName.SHA256,
            RSASignaturePadding.Pkcs1);
        request.CertificateExtensions.Add(new X509KeyUsageExtension(usageFlags, critical: true));

        var certificate = request.CreateSelfSigned(DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddYears(2));

        var pfxBytes = certificate.Export(X509ContentType.Pfx, password);
        File.WriteAllBytes(path, pfxBytes);
    }

    public static X509Certificate2 GetCertificateFile(string path, string password)
    {
        if (!File.Exists(path))
        {
            throw new FileNotFoundException(
                $"Certificate file not found at '{path}'.");
        }

        var bytes = File.ReadAllBytes(path);

        var certificate = X509CertificateLoader.LoadPkcs12(
            bytes,
            password,
            X509KeyStorageFlags.MachineKeySet |
            X509KeyStorageFlags.Exportable
        );

        ValidateCertificate(certificate);

        return certificate;
    }

    private static void ValidateCertificate(X509Certificate2 certificate)
    {
        if (!certificate.HasPrivateKey)
        {
            throw new CryptographicException($"Certificate '{certificate.Subject}' does not contain a private key.");
        }

        if (DateTime.UtcNow < certificate.NotBefore || DateTime.UtcNow > certificate.NotAfter)
        {
            throw new CryptographicException($"Certificate '{certificate.Subject}' is expired or not yet valid.");
        }

        var keyUsage = certificate.Extensions
            .OfType<X509KeyUsageExtension>()
            .FirstOrDefault();

        if (keyUsage is null)
        {
            throw new CryptographicException($"Certificate '{certificate.Subject}' has no KeyUsage extension.");
        }
    }
}
