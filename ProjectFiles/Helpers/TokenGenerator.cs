using System.Security.Cryptography;

namespace AgroTechProject.Helpers;

public static class TokenGenerator
{
    public static string GenerateRefreshToken()
    {
        var randomBytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }
}