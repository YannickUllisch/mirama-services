
namespace AuthService.Application.Common.Options;

public class OpenIddictOptions
{
    public const string Key = "Openiddict";

    public string Issuer { get; set; } = string.Empty;
    public string SigningCertificatePath { get; set; } = string.Empty;
    public string SigningCertificatePassword { get; set; } = string.Empty;
    public string EncryptionCertificatePath { get; set; } = string.Empty;
    public string EncryptionCertificatePassword { get; set; } = string.Empty;
}