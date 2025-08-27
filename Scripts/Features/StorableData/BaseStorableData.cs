namespace ab5entSDK.Features.StorableData
{
    public abstract class BaseStorableData : IStorableData
    {

        #region Properties

        public string Key { get; set; }

        public IStorableData StorableData { get; set; }

        public IStorageProvider StorageProvider { get; set; }

        #endregion

        #region Methods

        protected BaseStorableData(IStorageProvider storageProvider, IStorableData storableData = null)
        {
            SetStorageProvider(storageProvider);
            SetStorableData(storableData ?? this);
        }

        #region Setters

        public void SetStorableData(IStorableData storableData)
        {
            StorableData = storableData;
        }

        public void SetStorageProvider(IStorageProvider provider)
        {
            StorageProvider = provider;
        }

        #endregion

        public virtual void DataChanged()
        {
            StorableData.OnDataChanged();
        }

        public virtual void Save()
        {
            StorageProvider?.Save(Key, this);
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

        public virtual T LoadData<T>()
        {
            return StorageProvider.Load<T>(Key);
        }

        #endregion

    }
}