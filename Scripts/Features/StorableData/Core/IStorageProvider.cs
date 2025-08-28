namespace ab5entSDK.Features.StorableData
{
    public interface IStorageProvider
    {
        void SetPrefix(string prefix);

        void Set(string key, string value);

        string Get(string key);

        bool HasKey(string key);

        void DeleteKey(string key);

        void DeleteAll();

        void FlushToDisk();
    }
}