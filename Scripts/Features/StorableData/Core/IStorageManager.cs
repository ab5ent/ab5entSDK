namespace ab5entSDK.Features.StorableData
{
    public interface IStorageManager
    {
        void CreateRawRequest(IStorableData data, bool forceFlush = false);

        void TryToAddSaveRequest();

        void TryToFlushToDisk();

        void FlushToDisk(bool forceFlush = false);

        T Load<T>(string key) where T : class;

        bool TryLoad<T>(string key, out T value);
    }
}