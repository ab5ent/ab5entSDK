using UnityEngine;

namespace ab5entSDK.Features.StorableData
{
    public class PlayerPrefsStorageProvider : IStorageProvider
    {
        private string _prefix;

        public PlayerPrefsStorageProvider(string prefix = "")
        {
            SetPrefix(prefix);
        }

        public void SetPrefix(string prefix)
        {
            _prefix = prefix;
        }

        public void Set(string key, string value)
        {
            PlayerPrefs.SetString(_prefix + key, value);
        }

        public string Get(string key)
        {
            return PlayerPrefs.GetString(_prefix + key, null);
        }

        public bool HasKey(string key)
        {
            return PlayerPrefs.HasKey(_prefix + key);
        }

        public void DeleteKey(string key)
        {
            PlayerPrefs.DeleteKey(_prefix + key);
        }

        public void DeleteAll()
        {
            PlayerPrefs.DeleteAll();
        }

        public void FlushToDisk()
        {
            PlayerPrefs.Save();
        }
    }
}