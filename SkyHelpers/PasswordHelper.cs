namespace SkyHelpers
{
    public class PasswordHelper
    {
        // Make: bcrypt with Laravel-style $2y$ prefix
        public static string HashPassword(string password, int cost = 10)
        {
            var hash = BCrypt.Net.BCrypt.HashPassword(password, workFactor: cost);

            // Normalize prefix for Laravel
            if (hash.StartsWith("$2a$") || hash.StartsWith("$2b$"))
                hash = string.Concat("$2y$", hash.AsSpan(4));

            return hash;
        }

        // Check: accept $2y$/$2ay$ by mapping to a prefix BCrypt.Net understands
        public static bool VerifyPassword(string password, string laravelHash)
        {
            var verifyHash = laravelHash;

            if (verifyHash.StartsWith("$2y$") || verifyHash.StartsWith("$2ay$"))
                verifyHash = string.Concat("$2a$", verifyHash.AsSpan(4)); // BCrypt.Net accepts $2a$

            return BCrypt.Net.BCrypt.Verify(password, verifyHash);
        }
    }
}
