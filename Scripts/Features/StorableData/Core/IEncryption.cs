namespace ab5entSDK.Features.StorableData
{
    public interface IEncryption
    {
        string Encrypt(string plainText);

        string Decrypt(string cipherText);
    }
}