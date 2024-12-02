using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Infrastructure.Hasher
{
    public static class PasswordHasher
    {
        private const int SALT_SIZE = 128 / 8;
        private const int HASH_SIZE = 256 / 8;
        private const int ITERATIONS = 100000;

        public static string GenerateSalt()
        {
            byte[] salt = new byte[SALT_SIZE];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return Convert.ToBase64String(salt);
        }

        public static string GenerateHash(string password, string salt)
        {
            byte[] saltBytes = Convert.FromBase64String(salt);

            byte[] hash = KeyDerivation.Pbkdf2(
                password: password,
                salt: saltBytes,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: ITERATIONS,
                numBytesRequested: HASH_SIZE);

            return Convert.ToBase64String(hash);
        }

        public static bool VerifyPassword(string password, string hash, string salt)
        {
            string computedHash = GenerateHash(password, salt);
            return computedHash == hash;
        }
    }
}
