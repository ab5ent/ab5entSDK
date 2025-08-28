namespace ab5entSDK.Features.StorableData
{
    public interface IStorableData
    {
        #region Properties

        public string Key { get; set; }

        public IStorableData StorableData { get; }

        public IStorageManager StorageManager { get; }

        public bool CanSaveData { get; set; }

        #endregion

        #region Methods

        public void OnDataChanged();

        public void Save();

        public void DataChanged();

        #endregion
    }
}