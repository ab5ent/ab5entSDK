using System;
using UnityEngine;

namespace ab5entSDK.Features.StorableData.StorageProviders
{
    public class PlayerPrefsStorageProvider : IStorageProvider
    {
        public void Save<T>(string key, T data)
        {
            try
            {
                string json = JsonUtility.ToJson(data);
                PlayerPrefs.SetString(key, json);
                PlayerPrefs.Save();
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to save to PlayerPrefs: {ex.Message}");
            }
        }

        public T Load<T>(string key)
        {
            try
            {
                if (PlayerPrefs.HasKey(key))
                {
                    Debug.Log($"Loading from PlayerPrefs: {key}");
                    string json = PlayerPrefs.GetString(key);
                    Debug.Log("Loaded JSON: " + json);
                    return JsonUtility.FromJson<T>(json);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to load from PlayerPrefs: {ex.Message}");
            }

            return default(T);
        }
    }
}