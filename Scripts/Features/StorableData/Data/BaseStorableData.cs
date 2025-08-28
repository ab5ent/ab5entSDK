namespace ab5entSDK.Features.StorableData
{
    public abstract class BaseStorableData : IStorableData
    {
        #region Properties

        public string Key { get; set; }

        public IStorableData StorableData { get; protected set; }

        public IStorageManager StorageManager { get; protected set; }

        public bool CanSaveData { get; set; }

        #endregion

        #region Methods

        protected BaseStorableData(IStorageManager storageManager = null, IStorableData storableData = null)
        {
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

        public virtual T LoadData<T>() where T : class
        {
            return StorageManager.Load<T>(Key);
        }

        #endregion
    }
}