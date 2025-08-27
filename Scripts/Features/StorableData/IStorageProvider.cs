namespace ab5entSDK.Features.StorableData
{
    public interface IStorageProvider
    {
        void Save<T>(string key, T data);

        T Load<T>(string key);
    }
}