using System.Security.Cryptography;

namespace WGU_App_RileyJuniewic.Data.Services;

public static class PasswordHasher
{
    private const int SaltLength = 16;
    private const int HashLength = 32;
    private const int Iterations = 100000;

    private static readonly HashAlgorithmName _hashAlgorithm = HashAlgorithmName.SHA512;

    public static string Hash(string password)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(SaltLength);
        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, _hashAlgorithm, HashLength);
        return Convert.ToBase64String(salt) + ":" + Convert.ToBase64String(hash);
    }

    public static bool Verify(string password, string hash)
    {
        string[] parts = hash.Split(':');
        byte[] salt = Convert.FromBase64String(parts[0]);
        byte[] storedHash = Convert.FromBase64String(parts[1]);

        byte[] newHash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, _hashAlgorithm, HashLength);
        return newHash.SequenceEqual(storedHash);
    }
}