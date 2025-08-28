using UnityEngine;

namespace ab5entSDK.Features.StorableData
{
    public class UnityJsonConvert : IJsonConvert
    {
        public T ConvertToObject<T>(string json)
        {
            return JsonUtility.FromJson<T>(json);
        }

        public string ConvertToJson<T>(T value)
        {
            return JsonUtility.ToJson(value);
        }
    }
}