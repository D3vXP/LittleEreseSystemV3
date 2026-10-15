using System;
using System.Security.Cryptography;

namespace LittleEreseSystemV3.Helpers
{
    /// <summary>
    /// Turns a plain password into a salted PBKDF2 hash and checks a typed password against a stored hash.
    /// Passwords are NEVER stored in plain text.
    ///
    /// Stored format (one line of text, safe for the password_hash column):
    ///     PBKDF2$100000$&lt;base64 salt&gt;$&lt;base64 hash&gt;
    /// The iteration count sits inside the text, so old rows keep working if we change it later.
    /// </summary>
    internal static class PasswordHasher
    {
        private const string Prefix = "PBKDF2";
        private const int Iterations = 100000;
        private const int SaltSize = 16;   // 128-bit random salt
        private const int KeySize = 32;    // 256-bit derived key

        // The seed script ships with this placeholder for demo accounts; it must never log anyone in.
        private const string Placeholder = "REPLACE_WITH_HASH";

        /// <summary>Creates a brand new hash. A fresh random salt is generated every time.</summary>
        public static string Hash(string password)
        {
            if (string.IsNullOrEmpty(password)) return string.Empty;

            byte[] salt = new byte[SaltSize];
            using (RNGCryptoServiceProvider generator = new RNGCryptoServiceProvider())
            {
                generator.GetBytes(salt);
            }

            byte[] key = Derive(password, salt, Iterations);

            return string.Join("$", new string[]
            {
                Prefix,
                Iterations.ToString(),
                Convert.ToBase64String(salt),
                Convert.ToBase64String(key)
            });
        }

        /// <summary>
        /// Checks a typed password against a stored hash. Returns false instead of throwing when the
        /// stored value is null, empty, still the placeholder, or does not follow the format above -
        /// so a broken row in the database simply cannot log in.
        /// </summary>
        public static bool Verify(string password, string stored)
        {
            if (string.IsNullOrEmpty(password)) return false;
            if (string.IsNullOrEmpty(stored)) return false;
            if (stored.Trim().Equals(Placeholder, StringComparison.OrdinalIgnoreCase)) return false;

            try
            {
                string[] parts = stored.Trim().Split('$');
                if (parts.Length != 4) return false;
                if (!parts[0].Equals(Prefix, StringComparison.OrdinalIgnoreCase)) return false;

                int iterations;
                if (!int.TryParse(parts[1], out iterations)) return false;
                if (iterations <= 0) return false;

                byte[] salt = Convert.FromBase64String(parts[2]);
                byte[] expected = Convert.FromBase64String(parts[3]);
                if (salt.Length == 0 || expected.Length == 0) return false;

                byte[] actual = Derive(password, salt, iterations);
                return FixedTimeEquals(expected, actual);
            }
            catch
            {
                // Malformed base64 or an unreadable stored value: treat it as "wrong password".
                return false;
            }
        }

        /// <summary>PBKDF2-HMAC-SHA256. The only place the key derivation settings live.</summary>
        private static byte[] Derive(string password, byte[] salt, int iterations)
        {
            using (Rfc2898DeriveBytes derive = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256))
            {
                return derive.GetBytes(KeySize);
            }
        }

        /// <summary>
        /// Compares two byte arrays without stopping at the first difference, so the time taken
        /// says nothing about how much of the hash matched. (CryptographicOperations.FixedTimeEquals
        /// does not exist in .NET Framework, hence the manual loop.)
        /// </summary>
        private static bool FixedTimeEquals(byte[] left, byte[] right)
        {
            if (left == null || right == null) return false;
            if (left.Length != right.Length) return false;

            int difference = 0;
            for (int i = 0; i < left.Length; i++)
            {
                difference |= left[i] ^ right[i];
            }
            return difference == 0;
        }
    }
}
