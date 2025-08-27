namespace ab5entSDK.Features.StorableData
{
    public interface IStorableData
    {
        public IStorableData StorableData { get; set; }

        public IStorageProvider StorageProvider { get; set; }

        public void SetStorableData(IStorableData storableData);

        public void SetStorageProvider(IStorageProvider provider);

        public void OnDataChanged();

        public void Save();

        public void DataChanged();

    }
}