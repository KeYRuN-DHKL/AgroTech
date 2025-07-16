using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace AgroTechProject.Helpers;

public static class PasswordHasher
{
    public static string Hash(string password)
    {
        byte[] salt = new byte[128 / 8];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(salt);

        var hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password, salt, KeyDerivationPrf.HMACSHA256, 10000, 256 / 8));

        return $"{Convert.ToBase64String(salt)}.{hashed}";
    }

    public static bool Verify(string hashedPasswordWithSalt, string password)
    {
        var parts = hashedPasswordWithSalt.Split('.');
        if (parts.Length != 2) return false;

        var salt = Convert.FromBase64String(parts[0]);
        var hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password, salt, KeyDerivationPrf.HMACSHA256, 10000, 256 / 8));

        return parts[1] == hashed;
    }
}