using ab5entSDK.Features.StorableData;

namespace ab5entSDK.Features.StorableData
{
    public class NoEncryption : IEncryption
    {
        public string Encrypt(string plainText)
        {
            return plainText;
        }

        public string Decrypt(string cipherText)
        {
            return cipherText;
        }
    }
}