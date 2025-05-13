using System.Security.Cryptography;
using System.Text;

namespace ServerAPI.Services
{
    public class EncryptionService
    {
        public static string EncryptData(string plainText, string encryptionKey, string encryptionIv)
        {
            if (string.IsNullOrEmpty(plainText))
                throw new ArgumentNullException(nameof(plainText));

            if (string.IsNullOrEmpty(encryptionKey))
                throw new ArgumentNullException(nameof(encryptionKey));

            if (string.IsNullOrEmpty(encryptionIv))
                throw new ArgumentNullException(nameof(encryptionIv));

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(encryptionKey.PadRight(32).Substring(0, 32));
                aesAlg.IV = Encoding.UTF8.GetBytes(encryptionIv.PadRight(16).Substring(0, 16));

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                    }

                    return Convert.ToHexString(msEncrypt.ToArray());
                }
            }
        }

        public static string DecryptData(string encryptedToken, string EncryptionKey, string EncryptionIv)
        {
            if (string.IsNullOrEmpty(encryptedToken))
                throw new ArgumentNullException(nameof(encryptedToken));

            if (string.IsNullOrEmpty(EncryptionKey))
                throw new ArgumentNullException(nameof(EncryptionKey));

            if (string.IsNullOrEmpty(EncryptionIv))
                throw new ArgumentNullException(nameof(EncryptionIv));

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(EncryptionKey);
                aesAlg.IV = Encoding.UTF8.GetBytes(EncryptionIv);

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                byte[] encryptedBytes = Convert.FromHexString(encryptedToken);

                using (MemoryStream msDecrypt = new MemoryStream(encryptedBytes))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            return srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
        }
    }
}
