using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace ab5entSDK.Features.StorableData
{
    // doing nothing
    public class FileSaveProvider
    {
        private readonly string _saveDirectory;

        private readonly IEncryption _encryption;

        private readonly Dictionary<string, string> _cachedSaveData;
        private readonly Queue<(string, string)> _pendingSaveData;

        public FileSaveProvider(string saveDirectory = null)
        {
            _saveDirectory = saveDirectory ?? Path.Combine(Application.persistentDataPath, "SaveData");

            _cachedSaveData = new Dictionary<string, string>();
            _pendingSaveData = new Queue<(string, string)>();

            if (!Directory.Exists(_saveDirectory))
            {
                Directory.CreateDirectory(_saveDirectory);
            }
        }

        private string GetFilePath(string key)
        {
            return Path.Combine(_saveDirectory, $"{key}.json");
        }

        public void Save<T>(string key, T data)
        {
            try
            {
                string json = JsonUtility.ToJson(data);
                string encryptedJson = _encryption.Encrypt(json);

                // write to file
                File.WriteAllText(GetFilePath(key), encryptedJson);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to save to file: {ex.Message}");
            }
        }

        public T Load<T>(string key)
        {
            try
            {
                string filePath = GetFilePath(key);
                if (File.Exists(filePath))
                {
                    string encryptedJson = File.ReadAllText(filePath);
                    string json = _encryption.Decrypt(encryptedJson);
                    return JsonUtility.FromJson<T>(json);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to load from file: {ex.Message}");
            }

            return default(T);
        }

        public bool HasKey(string key)
        {
            return File.Exists(GetFilePath(key));
        }

        public void DeleteKey(string key)
        {
            try
            {
                string filePath = GetFilePath(key);
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to delete file: {ex.Message}");
            }
        }

        public void DeleteAll()
        {
            try
            {
                if (Directory.Exists(_saveDirectory))
                {
                    Directory.Delete(_saveDirectory, true);
                    Directory.CreateDirectory(_saveDirectory);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to delete all files: {ex.Message}");
            }
        }

        public void FlushToDisk()
        {
            // doing nothing because it had been stored to file whenever save
        }
    }
}