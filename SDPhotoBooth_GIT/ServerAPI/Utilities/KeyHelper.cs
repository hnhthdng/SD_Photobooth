using System.Security.Cryptography;

namespace ServerAPI.Helpers
{
    public class KeyHelper
    {

        public static RSA GetPrivateKey(string relativePath)
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string fullPath = Path.Combine(basePath, relativePath);

            var rsa = RSA.Create();
            var privateKey = File.ReadAllText(fullPath);
            rsa.ImportFromPem(privateKey.ToCharArray());
            return rsa;
        }

        public static RSA GetPublicKey(string relativePath)
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string fullPath = Path.Combine(basePath, relativePath);

            var rsa = RSA.Create();
            var publicKey = File.ReadAllText(fullPath);
            rsa.ImportFromPem(publicKey.ToCharArray());
            return rsa;
        }
    }
}
