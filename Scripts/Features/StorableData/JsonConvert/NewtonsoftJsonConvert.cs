#if NEWTONSOFT_JSON_SUPPORT
using Newtonsoft.Json;
#endif

namespace ab5entSDK.Features.StorableData
{
    public class NewtonsoftJsonConvert : IJsonConvert
    {
        public T ConvertToObject<T>(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                return default;
            }

#if NEWTONSOFT_JSON_SUPPORT
            try
            {
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (JsonException e)
            {
                UnityEngine.Debug.LogError($"[JsonConvert] Failed to deserialize: {e.Message}");
                return default;
            }
#endif
            return default;
        }

        public string ConvertToJson<T>(T data)
        {
#if NEWTONSOFT_JSON_SUPPORT
            try
            {
                return JsonConvert.SerializeObject(data, Formatting.None);
            }
            catch (JsonException e)
            {
                UnityEngine.Debug.LogError($"[JsonConvert] Failed to serialize: {e.Message}");
                return null;
            }
#endif
            return null;
        }
    }
}