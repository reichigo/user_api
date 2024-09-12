using Konscious.Security.Cryptography;

using System.Security.Cryptography;
using System.Text;

namespace MyPethere.User.CrossCutting;

public static class Password
{
    public static string GetHashPassword(string password)
    {
        var salt = GenerateSalt();

        var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
        {
            Salt = salt,
            DegreeOfParallelism = 8, // número de threads a serem usados
            MemorySize = 65536, // 64 MB
            Iterations = 4 // número de iterações
        };

        return Convert.ToBase64String(argon2.GetBytes(16)) + ":" + Convert.ToBase64String(salt);
    }

    private static byte[] GenerateSalt()
    {
        using (var rng = new RNGCryptoServiceProvider())
        {
            var salt = new byte[32];
            rng.GetBytes(salt);
            return salt;
        }
    }

    public static bool IsPasswordValid(string password, string storedHash)
    {
        // Divide o hash armazenado em hash e salt
        var parts = storedHash.Split(':');
        if (parts.Length != 2)
        {
            throw new FormatException("Formato de hash armazenado inválido.");
        }

        var storedPasswordHash = Convert.FromBase64String(parts[0]);
        var salt = Convert.FromBase64String(parts[1]);

        // Configura o Argon2id com a senha fornecida e o salt armazenado
        var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
        {
            Salt = salt,
            DegreeOfParallelism = 8,
            MemorySize = 65536,
            Iterations = 4
        };

        // Gera o hash da senha fornecida
        var computedHash = argon2.GetBytes(16);

        // Compara o hash calculado com o hash armazenado
        return CryptographicOperations.FixedTimeEquals(computedHash, storedPasswordHash);
    }
}

