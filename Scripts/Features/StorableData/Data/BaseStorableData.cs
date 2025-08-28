using ab5entSDK.Features.CheckIn;

namespace ab5entSDK.Features.StorableData
{
    public abstract class BaseStorableData : IStorableData
    {
        #region Properties

        public string Key { get; private set; }

        public IStorableData StorableData { get; private set; }

        public IStorageManager StorageManager { get; private set; }

        public bool CanSaveData { get; set; }

        #endregion

        #region Methods

        public void Initialize(string key = "", IStorageManager storageManager = null, IStorableData storableData = null)
        {
            Key = string.IsNullOrEmpty(key) ? GetType().Name : $"{key}_{GetType().Name}";
            StorageManager = storageManager;
            StorableData = storableData ?? this;
        }

        public virtual void DataChanged()
        {
            StorableData.OnDataChanged();
        }

        public virtual void Save()
        {
            StorageManager?.CreateRawRequest(this);
        }

        public virtual void OnDataChanged()
        {
            if (StorableData == this)
            {
                Save();
            }
            else
            {
                StorableData?.OnDataChanged();
            }
        }

        public virtual void ResetData()
        {
            DataChanged();
        }

        #endregion

        public static T CreateInstance<T>(string key = "", IStorageManager storageManager = null, IStorableData storableData = null) where T : BaseStorableData, new()
        {
            if (storageManager != null && !string.IsNullOrEmpty(key))
            {
                var loaded = storageManager.Load<T>(key);

                if (loaded != null)
                {
                    loaded.Initialize(key, storageManager, storableData);
                    return loaded;
                }
            }

            var instance = new T();
            instance.Initialize(key, storageManager, storableData);
            return instance;
        }

        protected BaseStorableData()
        {
            Key = GetType().Name;
        }
    }
}