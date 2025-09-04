using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using ab5entSDK.Features.StorableData;
using UnityEngine;
using System.Linq;
using System.Diagnostics;
using System.Security;
using System.Runtime.InteropServices;
using Debug = UnityEngine.Debug;

namespace ab5entSDK.Features.StorableData
{
    public class AESEncryption : IEncryption
    {
        // Sizes in bytes
        private const int SaltSize = 16;
        private const int IvSize = 16; // AES block size = 128 bits
        private const int KeySize = 32; // 256 bits key
        private const int MacSize = 32; // HMAC-SHA256 output bytes

        private readonly SecureString password;
        private readonly int pbkdf2Iterations;

        public AESEncryption(SecureString password, int pbkdf2Iterations = 100_000)
        {
            if (password == null || password.Length == 0)
                throw new ArgumentException("Password must not be null or empty", nameof(password));

            this.password = password.Copy(); // Create a copy to avoid external modifications
            this.pbkdf2Iterations = Math.Max(10_000, pbkdf2Iterations); // enforce minimum
        }

        public AESEncryption(string password, int pbkdf2Iterations = 100_000) : this(ConvertToSecureString(password), pbkdf2Iterations)
        {
        }

        // Output format (all concatenated): salt(16) || iv(16) || hmac(32) || ciphertext(variable)
        public string Encrypt(string plainText)
        {
            if (plainText == null) throw new ArgumentNullException(nameof(plainText));

            byte[] passwordBytes = null;

            try
            {
                passwordBytes = ConvertSecureStringToByteArray(password);

                byte[] salt = new byte[SaltSize];
                byte[] iv = new byte[IvSize];
                RandomNumberGenerator.Fill(salt);
                RandomNumberGenerator.Fill(iv);

                // Derive a 64-byte (512-bit) key material then split: first 32 for AES, next 32 for HMAC
                byte[] keyMaterial = new Rfc2898DeriveBytes(passwordBytes, salt, pbkdf2Iterations, HashAlgorithmName.SHA256)
                    .GetBytes(KeySize * 2);

                byte[] aesKey = keyMaterial.Take(KeySize).ToArray();
                byte[] macKey = keyMaterial.Skip(KeySize).Take(KeySize).ToArray();

                byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
                byte[] cipherBytes;

                using (Aes aes = Aes.Create())
                {
                    aes.Mode = CipherMode.CBC;
                    aes.Padding = PaddingMode.PKCS7;
                    aes.KeySize = KeySize * 8;
                    aes.BlockSize = 128;
                    aes.Key = aesKey;
                    aes.IV = iv;

                    using (var ms = new MemoryStream())
                    using (var encryptor = aes.CreateEncryptor())
                    using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        cs.Write(plainBytes, 0, plainBytes.Length);
                        cs.FlushFinalBlock();
                        cipherBytes = ms.ToArray();
                    }
                }

                // Compute HMAC over (salt || iv || ciphertext)
                byte[] toMac = ConcatAll(salt, iv, cipherBytes);
                byte[] hmac;

                using (var h = new HMACSHA256(macKey))
                {
                    hmac = h.ComputeHash(toMac);
                }

                // Clear sensitive data
                ClearByteArray(aesKey);
                ClearByteArray(macKey);
                ClearByteArray(keyMaterial);

                byte[] result = ConcatAll(salt, iv, hmac, cipherBytes);
                return Convert.ToBase64String(result);
            }
            catch (Exception ex)
            {
                Debug.LogError($"AESEncryption.Encrypt failed: {ex}");
                throw; // do not silently return plaintext in production
            }
            finally
            {
                if (passwordBytes != null)
                    ClearByteArray(passwordBytes);
            }
        }

        public string Decrypt(string cipherText)
        {
            if (cipherText == null) throw new ArgumentNullException(nameof(cipherText));

            byte[] allBytes;
            byte[] passwordBytes = null;

            try
            {
                allBytes = Convert.FromBase64String(cipherText);
            }
            catch (FormatException fe)
            {
                Debug.LogError($"AESEncryption.Decrypt invalid base64: {fe.Message}");
                throw new CryptographicException("Invalid ciphertext format.", fe);
            }

            // Validate minimum length: salt + iv + hmac
            if (allBytes.Length < SaltSize + IvSize + MacSize)
                throw new CryptographicException("Ciphertext too short.");

            try
            {
                passwordBytes = ConvertSecureStringToByteArray(password);

                int offset = 0;
                byte[] salt = Slice(allBytes, offset, SaltSize);
                offset += SaltSize;
                byte[] iv = Slice(allBytes, offset, IvSize);
                offset += IvSize;
                byte[] hmac = Slice(allBytes, offset, MacSize);
                offset += MacSize;
                byte[] cipherBytes = Slice(allBytes, offset, allBytes.Length - offset);

                // Derive keys
                byte[] keyMaterial = new Rfc2898DeriveBytes(passwordBytes, salt, pbkdf2Iterations, HashAlgorithmName.SHA256)
                    .GetBytes(KeySize * 2);
                byte[] aesKey = keyMaterial.Take(KeySize).ToArray();
                byte[] macKey = keyMaterial.Skip(KeySize).Take(KeySize).ToArray();

                // Verify HMAC
                byte[] toMac = ConcatAll(salt, iv, cipherBytes);
                byte[] expectedHmac;

                using (var h = new HMACSHA256(macKey))
                {
                    expectedHmac = h.ComputeHash(toMac);
                }

                if (!FixedTimeEquals(hmac, expectedHmac))
                {
                    Debug.LogError("AESEncryption.Decrypt HMAC validation failed.");
                    throw new CryptographicException("Message authentication failed (HMAC mismatch).");
                }

                // Decrypt
                try
                {
                    using (Aes aes = Aes.Create())
                    {
                        aes.Mode = CipherMode.CBC;
                        aes.Padding = PaddingMode.PKCS7;
                        aes.KeySize = KeySize * 8;
                        aes.BlockSize = 128;
                        aes.Key = aesKey;
                        aes.IV = iv;

                        using (var ms = new MemoryStream(cipherBytes))
                        using (var decryptor = aes.CreateDecryptor())
                        using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                        using (var sr = new StreamReader(cs, Encoding.UTF8))
                        {
                            return sr.ReadToEnd();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogError($"AESEncryption.Decrypt failed during AES step: {ex}");
                    throw new CryptographicException("Decryption failed.", ex);
                }
                finally
                {
                    // Clear sensitive data
                    ClearByteArray(aesKey);
                    ClearByteArray(macKey);
                    ClearByteArray(keyMaterial);
                }
            }
            finally
            {
                if (passwordBytes != null)
                    ClearByteArray(passwordBytes);
            }
        }

        // Implement IDisposable to properly clean up SecureString
        public void Dispose()
        {
            password?.Dispose();
        }

        #region Helpers

        private static SecureString ConvertToSecureString(string strPassword)
        {
            if (string.IsNullOrEmpty(strPassword))
            {
                throw new ArgumentException("Password must not be null or empty", nameof(strPassword));
            }

            var secureString = new SecureString();

            foreach (char c in strPassword)
            {
                secureString.AppendChar(c);
            }

            secureString.MakeReadOnly();
            return secureString;
        }

        private static byte[] ConvertSecureStringToByteArray(SecureString secureString)
        {
            IntPtr ptr = IntPtr.Zero;

            try
            {
                ptr = Marshal.SecureStringToGlobalAllocUnicode(secureString);
                string password = Marshal.PtrToStringUni(ptr);
                return Encoding.UTF8.GetBytes(password);
            }
            finally
            {
                if (ptr != IntPtr.Zero)
                    Marshal.ZeroFreeGlobalAllocUnicode(ptr);
            }
        }

        private static void ClearByteArray(byte[] array)
        {
            if (array != null)
            {
                Array.Clear(array, 0, array.Length);
            }
        }

        private static byte[] Slice(byte[] source, int index, int length)
        {
            var dest = new byte[length];
            Buffer.BlockCopy(source, index, dest, 0, length);
            return dest;
        }

        private static byte[] ConcatAll(params byte[][] arrays)
        {
            int length = arrays.Sum(a => a.Length);
            byte[] result = new byte[length];
            int pos = 0;

            foreach (var a in arrays)
            {
                Buffer.BlockCopy(a, 0, result, pos, a.Length);
                pos += a.Length;
            }

            return result;
        }

        // Constant-time comparison to mitigate timing attacks.
        private static bool FixedTimeEquals(byte[] a, byte[] b)
        {
            if (a == null || b == null || a.Length != b.Length) return false;
            #if NETCOREAPP || NET5_0_OR_GREATER
        return System.Security.Cryptography.CryptographicOperations.FixedTimeEquals(a, b);
            #else
            int diff = 0;

            for (int i = 0; i < a.Length; i++)
                diff |= a[i] ^ b[i];
            return diff == 0;
            #endif
        }

        #endregion

    }
}