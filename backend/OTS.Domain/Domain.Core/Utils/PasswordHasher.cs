using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace OTS.Domain.Domain.Core.Utils;

public static class PasswordHasher
{
    public static string Hash(string password)
    {
        var result = KeyDerivation.Pbkdf2(
            password: password,
            salt: RandomNumberGenerator.GetBytes(128/8),
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 10000,
            numBytesRequested: 256/8);

        return Convert.ToBase64String(result);
    }
}