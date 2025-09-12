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

        public virtual void Initialize(string key = "", IStorageManager storageManager = null, IStorableData storableData = null)
        {
            Key = string.IsNullOrEmpty(key) ? GetType().Name : $"{key}_{GetType().Name}";
            StorageManager = storageManager;
            StorableData = storableData ?? this;

            PopulateFields();
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

        public abstract void PopulateFields();

        protected BaseStorableData()
        {
            Key = GetType().Name;
        }

        #endregion

        public static T Load<T>(string key = "", IStorageManager storageManager = null, IStorableData storableData = null) where T : BaseStorableData, new()
        {
            if (storageManager != null && !string.IsNullOrEmpty(key))
            {
                string validatedKey = string.IsNullOrEmpty(key) ? typeof(T).Name : $"{key}_{typeof(T).Name}";
                T loaded = storageManager.Load<T>(validatedKey);

                if (loaded != null)
                {
                    loaded.Initialize(key, storageManager, storableData);
                    return loaded;
                }
            }

            T instance = new T();
            instance.Initialize(key, storageManager, storableData);
            return instance;
        }
    }
}