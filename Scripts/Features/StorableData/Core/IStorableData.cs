namespace ab5entSDK.Features.StorableData
{
    public interface IStorableData
    {
        #region Properties

        public string Key { get; }

        public IStorableData StorableData { get; }

        public IStorageManager StorageManager { get; }

        public bool CanSaveData { get; set; }

        #endregion

        #region Methods

        public void Initialize(string key = "", IStorageManager storageManager = null, IStorableData storableData = null);

        public void OnDataChanged();

        public void Save();

        public void DataChanged();

        #endregion
    }
}