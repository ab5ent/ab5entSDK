using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using UnityEditor;
using UnityEngine;

namespace ab5entSDK.Common.Systems
{
    public class SaveSystem<T> : MonoBehaviour where T : class
    {
        private enum SaveType
        {
            Binary,
            Json,
            XML,
        }

        [Header("Configure")]
        [SerializeField]
        private string fileName;

        [SerializeField]
        private SaveType saveType;

        [SerializeField]
        private T data;

        private string _filePath;

        protected virtual void Awake()
        {
            _filePath = $"{Application.persistentDataPath}/{fileName}.save";
        }

        protected virtual void OpenPersistentDataPath()
        {
            string dataPath = Application.persistentDataPath;
            EditorUtility.RevealInFinder(dataPath);
        }

        public virtual void Save()
        {
            switch (saveType)
            {
                case SaveType.Binary:
                    {
                        SaveToBinaryFile();
                        break;
                    }
                case SaveType.XML:
                    {
                        SaveToXMLFile();
                        break;
                    }
                case SaveType.Json:
                    {
                        SaveToJsonFile();
                        break;
                    }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public virtual void Load()
        {
            if (!File.Exists(_filePath))
            {
                Debug.LogWarning($"Couldn't find data to load");
            }

            switch (saveType)
            {
                case SaveType.Binary:
                    {
                        LoadFromBinaryFile();
                        break;
                    }
                case SaveType.XML:
                    {
                        LoadFromXMLFile();
                        break;
                    }
                case SaveType.Json:
                    {
                        LoadFromJsonFile();
                        break;
                    }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public virtual void Delete()
        {
            File.Delete(_filePath);
        }

        #region Binary

        private void SaveToBinaryFile()
        {
            string json = JsonUtility.ToJson(data);
            FileStream stream = new FileStream(_filePath, FileMode.Create);
            new BinaryFormatter().Serialize(stream, json);
            stream.Close();
        }

        private void LoadFromBinaryFile()
        {
            FileStream stream = new FileStream(_filePath, FileMode.Open);
            string jsonStr =  new BinaryFormatter().Deserialize(stream) as string;
            data = JsonUtility.FromJson<T>(jsonStr);
            stream.Close();
        }

        #endregion

        #region Json

        private void SaveToJsonFile()
        {
            File.WriteAllText(_filePath, JsonUtility.ToJson(data));
        }

        private void LoadFromJsonFile()
        {
            data = JsonUtility.FromJson<T>(File.ReadAllText(_filePath));
        }

        #endregion

        #region XML

        private void SaveToXMLFile()
        {
            FileStream stream = new FileStream(_filePath, FileMode.Create);
            new XmlSerializer(typeof(T)).Serialize(stream, data);
            stream.Close();
        }

        private void LoadFromXMLFile()
        {
            FileStream stream = new FileStream(_filePath, FileMode.Open);
            data = new XmlSerializer(typeof(T)).Deserialize(stream) as T;
            stream.Close();
        }

        #endregion
    }
}