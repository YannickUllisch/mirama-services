using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.IdentityModel.Tokens;

namespace Mirama.Api.Auth;

public sealed class AuthJsTokenHandler(byte[] devKey, byte[] prodKey, string? issuer, string? audience) : TokenHandler
{
    private const string DevPrefix = "0:";
    private const string ProdPrefix = "1:";

    public static string TagToken(bool isProd, string jwe) =>
        (isProd ? ProdPrefix : DevPrefix) + jwe;

    public override SecurityToken ReadToken(string token) => new JweToken();

    public override Task<TokenValidationResult> ValidateTokenAsync(string token, TokenValidationParameters validationParameters)
    {
        try
        {
            var isProd = token.StartsWith(ProdPrefix, StringComparison.Ordinal);
            var jwe = token[(isProd ? ProdPrefix.Length : DevPrefix.Length)..];
            var key = isProd ? prodKey : devKey;

            var claims = DecryptAndValidate(jwe, key);
            var identity = new ClaimsIdentity(claims, "AuthJs", ClaimTypes.Name, "role");

            return Task.FromResult(new TokenValidationResult
            {
                IsValid = true,
                ClaimsIdentity = identity,
                SecurityToken = new JweToken(),
            });
        }
        catch (Exception ex)
        {
            return Task.FromResult(new TokenValidationResult
            {
                IsValid = false,
                Exception = ex,
            });
        }
    }

    private List<Claim> DecryptAndValidate(string jwe, byte[] key)
    {
        var parts = jwe.Split('.');
        if (parts.Length != 5)
            throw new SecurityTokenException("Not a valid JWE compact serialization.");

        var headerBytes = Encoding.ASCII.GetBytes(parts[0]);
        var iv = Base64UrlEncoder.DecodeBytes(parts[2]);
        var ciphertext = Base64UrlEncoder.DecodeBytes(parts[3]);
        var tag = Base64UrlEncoder.DecodeBytes(parts[4]);

        // A256CBC-HS512: first 32 bytes = MAC key, last 32 = AES-CBC key
        VerifyAuthTag(headerBytes, iv, ciphertext, tag, macKey: key[..32]);

        var plaintext = DecryptAesCbc(ciphertext, iv, encKey: key[32..]);
        var payload = JsonSerializer.Deserialize<JsonElement>(plaintext);

        ValidateClaims(payload);

        return MapClaims(payload);
    }

    private static void VerifyAuthTag(byte[] header, byte[] iv, byte[] ciphertext, byte[] tag, byte[] macKey)
    {
        var al = ToBeInt64Bits(header.Length);

        using var hmac = new HMACSHA512(macKey);
        hmac.TransformBlock(header, 0, header.Length, null, 0);
        hmac.TransformBlock(iv, 0, iv.Length, null, 0);
        hmac.TransformBlock(ciphertext, 0, ciphertext.Length, null, 0);
        hmac.TransformFinalBlock(al, 0, al.Length);

        // Auth tag is the first 32 bytes of the 64-byte HMAC-SHA512 output
        if (!CryptographicOperations.FixedTimeEquals(hmac.Hash.AsSpan(0, 32), tag))
            throw new SecurityTokenException("Invalid JWE authentication tag.");
    }

    private static string DecryptAesCbc(byte[] ciphertext, byte[] iv, byte[] encKey)
    {
        using var aes = Aes.Create();
        aes.Key = encKey;
        aes.IV = iv;
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;
        using var decryptor = aes.CreateDecryptor();
        return Encoding.UTF8.GetString(decryptor.TransformFinalBlock(ciphertext, 0, ciphertext.Length));
    }

    private void ValidateClaims(JsonElement payload)
    {
        if (payload.TryGetProperty("exp", out var exp) && exp.TryGetInt64(out var expVal))
            if (DateTimeOffset.FromUnixTimeSeconds(expVal) < DateTimeOffset.UtcNow)
                throw new SecurityTokenExpiredException("Token expired.");

        if (issuer is not null && payload.TryGetProperty("iss", out var iss) && iss.GetString() != issuer)
            throw new SecurityTokenInvalidIssuerException($"Invalid issuer. Expected: {issuer}");

        if (audience is not null && payload.TryGetProperty("aud", out var aud) && aud.GetString() != audience)
            throw new SecurityTokenInvalidAudienceException($"Invalid audience. Expected: {audience}");
    }

    private static List<Claim> MapClaims(JsonElement payload)
    {
        var claims = new List<Claim>();

        foreach (var prop in payload.EnumerateObject())
        {
            var value = prop.Value.ValueKind == JsonValueKind.String
                ? prop.Value.GetString()
                : prop.Value.GetRawText();
            claims.Add(new Claim(prop.Name, value ?? string.Empty));
        }

        if (payload.TryGetProperty("name", out var name))
            claims.Add(new Claim(ClaimTypes.Name, name.GetString() ?? string.Empty));

        return claims;
    }

    private static byte[] ToBeInt64Bits(int byteLength)
    {
        var bits = BitConverter.GetBytes((long)byteLength * 8);
        if (BitConverter.IsLittleEndian) Array.Reverse(bits);
        return bits;
    }

    private sealed class JweToken : SecurityToken
    {
        public override string Id => string.Empty;
        public override string Issuer => string.Empty;
        public override SecurityKey? SecurityKey => null;
        public override SecurityKey? SigningKey { get => null; set { } }
        public override DateTime ValidFrom => DateTime.MinValue;
        public override DateTime ValidTo => DateTime.MaxValue;
    }
}
