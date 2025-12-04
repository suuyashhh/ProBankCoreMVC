using System.Security.Cryptography;
using System.Text;

namespace ProBankCoreMVC.Helpers
{
    public static class AesEncryptionHelper
    {
        private const string IV = "0000000000000000";

        public static string Encrypt(string plainText, string key)
        {
            byte[] ivBytes = Encoding.UTF8.GetBytes(IV);
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);

            using var aes = Aes.Create();
            aes.Key = keyBytes;
            aes.IV = ivBytes;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            using var encryptor = aes.CreateEncryptor();
            byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
            byte[] cipherBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

            return Convert.ToBase64String(cipherBytes);
        }

        public static string Decrypt(string cipherText, string key)
        {
            if (string.IsNullOrWhiteSpace(cipherText))
                throw new Exception("Cipher text is empty");

            byte[] ivBytes = Encoding.UTF8.GetBytes(IV);
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);

            using var aes = Aes.Create();
            aes.Key = keyBytes;
            aes.IV = ivBytes;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            byte[] cipherBytes = Convert.FromBase64String(cipherText);

            using var decryptor = aes.CreateDecryptor();
            byte[] plainBytes = decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);

            return Encoding.UTF8.GetString(plainBytes);
        }
    }
}
