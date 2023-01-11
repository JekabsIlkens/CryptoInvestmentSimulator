using System.Security.Cryptography;

namespace CryptoInvestmentSimulator.Helpers
{
    public static class ConfidenceHelper
    {
        // All alphanumeric characters to pass as allowed characters to RNGCryptoServiceProvider.
        const string alphaNumeric = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

        /// <summary>
        /// Generates a random alpha-numeric key.
        /// Used and requested from user when portfolio is reset to avoid accidental reset.
        /// </summary>
        /// <param name="length">Length of key.</param>
        /// <returns>
        /// Random alpha-numeric key.
        /// </returns>
        public static string GetRandomKey(int length)
        {
            if(length <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(length));
            }

            string key = string.Empty;

            // New instance of RNGCryptoServiceProvider is required.
            #pragma warning disable SYSLIB0023
            using (var rngProvider = new RNGCryptoServiceProvider())
            {
                while (key.Length != length)
                {
                    byte[] oneByte = new byte[1];
                    rngProvider.GetBytes(oneByte);
                    char character = (char)oneByte[0];

                    if (alphaNumeric.Contains(character))
                    {
                        key += character;
                    }
                }
            }

            return key;
        }
    }
}
