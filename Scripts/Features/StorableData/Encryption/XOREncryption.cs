using System;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace ab5entSDK.Features.StorableData
{
    public class XOREncryption : IEncryption
    {
        private readonly byte[] _key;

        public XOREncryption(string password = null)
        {
            string defaultKey = password ?? "MyGameEncryptionKey2024";
            _key = DeriveKey(defaultKey);
        }

        private static byte[] DeriveKey(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        public string Encrypt(string plainText)
        {
            try
            {
                byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
                byte[] encryptedBytes = new byte[plainBytes.Length];

                for (int i = 0; i < plainBytes.Length; i++)
                    encryptedBytes[i] = (byte)(plainBytes[i] ^ _key[i % _key.Length]);

                return Convert.ToBase64String(encryptedBytes);
            }
            catch (Exception ex)
            {
                Debug.LogError($"XOR Encryption failed: {ex.Message}");
                #if UNITY_EDITOR
                return plainText;
                #else
            throw;
                #endif
            }
        }

        public string Decrypt(string cipherText)
        {
            try
            {
                byte[] encryptedBytes = Convert.FromBase64String(cipherText);
                byte[] decryptedBytes = new byte[encryptedBytes.Length];

                for (int i = 0; i < encryptedBytes.Length; i++)
                    decryptedBytes[i] = (byte)(encryptedBytes[i] ^ _key[i % _key.Length]);

                return Encoding.UTF8.GetString(decryptedBytes);
            }
            catch (Exception ex)
            {
                Debug.LogError($"XOR Decryption failed: {ex.Message}");
                #if UNITY_EDITOR
                return cipherText;
                #else
            throw;
                #endif
            }
        }
    }
}