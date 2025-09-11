using System.Collections.Generic;

namespace ab5entSDK.Core
{
    public static class GameAssetRegistry
    {
        private static readonly Dictionary<string, GameAssetDefinition> _definitions = new();

        public static void Register(GameAssetDefinition def)
        {
            if (string.IsNullOrEmpty(def.Id))
            {
                UnityEngine.Debug.LogError($"GameAssetDefinition {def.name} has no Id!");
                return;
            }

            if (_definitions.TryAdd(def.Id, def))
            {
                return;
            }

            UnityEngine.Debug.LogError($"Duplicate GameAssetDefinition Id: {def.Id}");
        }

        public static GameAssetDefinition Get(string id)
        {
            return _definitions.GetValueOrDefault(id);
        }

        public static IEnumerable<GameAssetDefinition> GetAll()
        {
            return _definitions.Values;
        }

        public static void Clear()
        {
            _definitions.Clear();
        }
    }
}