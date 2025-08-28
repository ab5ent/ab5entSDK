namespace ab5entSDK.Features.StorableData
{
    public interface IJsonConvert
    {
        T ConvertToObject<T>(string json);

        string ConvertToJson<T>(T data);
    }
}