using ab5entSDK.Unity.Editor.Attributes;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;
using UnityEngine;

namespace ab5entSDK.Common.Systems
{
    public class SaveSystem<T> : MonoBehaviour where T : class
    {
        [SerializeField]
        private string filename;

        [SerializeField]
        private ESaveType saveType;

        [SerializeField, MinLength(16), MaxLength(16)]
        private string key;

        [SerializeField]
        private T data;

        protected virtual void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            if (filename != "")
            {
                return;
            }

            filename = $"{name}_{GetType().Name}";
            Debug.LogWarning($"SaveFile's name has not been declared. Using default name {filename}");
        }

        public virtual void SetData(T value)
        {
            data = value;
        }

        public virtual T GetData()
        {
            return data;
        }

        private string GetFilePath()
        {
            switch (saveType)
            {
                case ESaveType.Encrypt:
                case ESaveType.Binary:
                    return $"{Application.persistentDataPath}/{filename}.bin";
                case ESaveType.Json:
                    return $"{Application.persistentDataPath}/{filename}.json";
                case ESaveType.XML:
                    return $"{Application.persistentDataPath}/{filename}.xml";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public virtual void Save()
        {
            switch (saveType)
            {
                case ESaveType.Binary:
                    {
                        SaveToBinaryFile();
                        break;
                    }
                case ESaveType.XML:
                    {
                        SaveToXMLFile();
                        break;
                    }
                case ESaveType.Json:
                    {
                        SaveToJsonFile();
                        break;
                    }
                case ESaveType.Encrypt:
                    {
                        SaveWithEncrypt();
                        break;
                    }
                default:
                    throw new ArgumentOutOfRangeException();
            }

            Debug.Log("Saved Data");
        }

        public virtual void Load()
        {
            if (!File.Exists(GetFilePath()))
            {
                Debug.LogWarning($"Couldn't find data to load");
                return;
            }

            switch (saveType)
            {
                case ESaveType.Binary:
                    {
                        LoadFromBinaryFile();
                        break;
                    }
                case ESaveType.XML:
                    {
                        LoadFromXMLFile();
                        break;
                    }
                case ESaveType.Json:
                    {
                        LoadFromJsonFile();
                        break;
                    }
                case ESaveType.Encrypt:
                    {
                        LoadFromEncryptData();
                        break;
                    }
                default:
                    throw new ArgumentOutOfRangeException();
            }

            Debug.Log("Loaded Data");
        }

        public virtual void Delete()
        {
            File.Delete(GetFilePath());
            Debug.Log("Deleted Data");

        }

        #region Binary

        private void SaveToBinaryFile()
        {
            string json = JsonUtility.ToJson(data);
            FileStream stream = new FileStream(GetFilePath(), FileMode.Create);
            new BinaryFormatter().Serialize(stream, json);
            stream.Close();
        }

        private void LoadFromBinaryFile()
        {
            FileStream stream = new FileStream(GetFilePath(), FileMode.Open);
            string jsonStr = new BinaryFormatter().Deserialize(stream) as string;
            data = JsonUtility.FromJson<T>(jsonStr);
            stream.Close();
        }

        #endregion

        #region Json

        private void SaveToJsonFile()
        {
            File.WriteAllText(GetFilePath(), JsonUtility.ToJson(data));
        }

        private void LoadFromJsonFile()
        {
            data = JsonUtility.FromJson<T>(File.ReadAllText(GetFilePath()));
        }

        #endregion

        #region XML

        private void SaveToXMLFile()
        {
            FileStream stream = new FileStream(GetFilePath(), FileMode.Create);
            new XmlSerializer(typeof(T)).Serialize(stream, data);
            stream.Close();
        }

        private void LoadFromXMLFile()
        {
            FileStream stream = new FileStream(GetFilePath(), FileMode.Open);
            data = new XmlSerializer(typeof(T)).Deserialize(stream) as T;
            stream.Close();
        }

        #endregion

        #region Encrypt

        private static byte[] Encrypt(string plainText, string key)
        {
            using AesManaged aesAlg = new AesManaged();
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            aesAlg.Key = keyBytes;
            aesAlg.IV = GenerateRandomInitializationVector();

            using ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
            using MemoryStream msEncrypt = new MemoryStream();

            msEncrypt.Write(aesAlg.IV, 0, aesAlg.IV.Length);

            using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
            {
                using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                {
                    swEncrypt.Write(plainText);
                }
            }

            return msEncrypt.ToArray();
        }

        private static string Decrypt(byte[] cipherText, string key)
        {
            try
            {
                using AesManaged aesAlg = new AesManaged();
                byte[] keyBytes = Encoding.UTF8.GetBytes(key);
                aesAlg.Key = keyBytes;

                byte[] iv = new byte[16];
                Array.Copy(cipherText, 0, iv, 0, 16);
                aesAlg.IV = iv; // Extract IV from the first 16 bytes

                using ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                using MemoryStream msDecrypt = new MemoryStream(cipherText, 16, cipherText.Length - 16);
                using CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
                using StreamReader srDecrypt = new StreamReader(csDecrypt);

                return srDecrypt.ReadToEnd();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error during decryption: " + ex.Message);
                return null; // Return null to indicate decryption failure
            }

        }

        private static byte[] GenerateRandomInitializationVector()
        {
            using RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] iv = new byte[16];
            rng.GetBytes(iv);
            return iv;
        }

        private void SaveWithEncrypt()
        {
            string plainTextData = JsonUtility.ToJson(data);
            byte[] encryptedData = Encrypt(plainTextData, key);
            File.WriteAllBytes(GetFilePath(), encryptedData);
        }

        private void LoadFromEncryptData()
        {
            byte[] encryptedData = File.ReadAllBytes(GetFilePath());
            string plainTextData = Decrypt(encryptedData, key);
            data = JsonUtility.FromJson<T>(plainTextData);
        }

        #endregion
    }
}
