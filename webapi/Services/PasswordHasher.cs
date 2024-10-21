using System;
using System.Security.Cryptography;
using System.Text;

namespace webapi.Services
{
    public static class PasswordHasher
    {
        public static string HashPassword(string password)
        {
            // Generate a random salt
            byte[] salt = GenerateSalt();

            // Combine the password and salt
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            byte[] combinedBytes = new byte[salt.Length + passwordBytes.Length];
            Array.Copy(salt, 0, combinedBytes, 0, salt.Length);
            Array.Copy(passwordBytes, 0, combinedBytes, salt.Length, passwordBytes.Length);

            // Hash the combined bytes
            byte[] hashBytes = SHA256.HashData(combinedBytes);

            // Convert the salt and hash to base64 strings
            string saltString = Convert.ToBase64String(salt);
            string hashString = Convert.ToBase64String(hashBytes);

            // Return the salt and hash as a single string, separated by a colon
            return $"{saltString}:{hashString}";
        }

        public static bool VerifyPassword(string password, string hashedPassword)
        {
            // Split the hashed password into its salt and hash components
            string[] parts = hashedPassword.Split(':');
            byte[] salt = Convert.FromBase64String(parts[0]);
            byte[] hashBytes = Convert.FromBase64String(parts[1]);

            // Combine the password and salt
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            byte[] combinedBytes = new byte[salt.Length + passwordBytes.Length];
            Array.Copy(salt, 0, combinedBytes, 0, salt.Length);
            Array.Copy(passwordBytes, 0, combinedBytes, salt.Length, passwordBytes.Length);

            // Hash the combined bytes
            byte[] newHashBytes = SHA256.Create().ComputeHash(combinedBytes);

            // Compare the new hash with the stored hash
            return AreByteArraysEqual(hashBytes, newHashBytes);
        }

        private static byte[] GenerateSalt()
        {
            using RandomNumberGenerator rng = RandomNumberGenerator.Create();
            byte[] salt = new byte[16]; // Adjust the size based on your security requirements
            rng.GetBytes(salt);
            return salt;
        }

        private static bool AreByteArraysEqual(byte[] array1, byte[] array2)
        {
            if (array1.Length != array2.Length)
            {
                return false;
            }

            for (int i = 0; i < array1.Length; i++)
            {
                if (array1[i] != array2[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}