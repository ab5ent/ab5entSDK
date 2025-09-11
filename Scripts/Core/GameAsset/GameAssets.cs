using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace ab5entSDK.Core
{
    [CreateAssetMenu(fileName = "GameAssets", menuName = "GameAssets/Root", order = 0)]
    public class GameAssets : ScriptableObject
    {
        [field: SerializeField] private GameAssetDefinition[] Definitions { get; set; }

        [ContextMenu("Get Definitions")]
        private void GetDefinitions()
        {
            string[] guids = AssetDatabase.FindAssets("t:GameAssetDefinition");

            Definitions = new GameAssetDefinition[guids.Length];

            for (int i = 0; i < guids.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                Definitions[i] = AssetDatabase.LoadAssetAtPath<GameAssetDefinition>(path);
            }

            Debug.Log($"Found {Definitions.Length} GameAssetDefinitions");
        }

        [ContextMenu("Generate GameAssetDefinitions")]
        private void GenerateGameAssetDefinitions()
        {
            if (Definitions == null || Definitions.Length == 0)
            {
                return;
            }

            string defaultNamespace = EditorSettings.projectGenerationRootNamespace;
            if (string.IsNullOrEmpty(defaultNamespace))
            {
                defaultNamespace = "DefaultNamespace";
            }

            string className = "GameAssetDefinitions";

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("// Auto-generated GameAssetDefinitions");

            sb.AppendLine();
            sb.AppendLine("using ab5entSDK.Core;");
            sb.AppendLine();

            sb.AppendLine($"namespace {defaultNamespace}");
            sb.AppendLine("{");
            sb.AppendLine($"    public static class {className}");
            sb.AppendLine("    {");

            foreach (GameAssetDefinition def in Definitions)
            {
                if (def == null)
                {
                    continue;
                }

                string safeName = def.name.Replace(" ", "_"); // basic sanitization
                string line = $"        public static readonly GameAssetDefinition {safeName} = GameAssetRegistry.Get(\"{def.Id}\");";
                sb.AppendLine(line);
            }

            sb.AppendLine("    }");
            sb.AppendLine("}");

            string folderPath = "Assets/Scripts/Generated";

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            string filePath = Path.Combine(folderPath, $"{className}.cs");
            File.WriteAllText(filePath, sb.ToString());

            AssetDatabase.Refresh();

            Debug.Log($"GameAssetDefinitions generated at: {filePath} (namespace: {defaultNamespace})");
        }

        [RuntimeInitializeOnLoadMethod]
        private static void LoadAssets()
        {
            GameAssetRegistry.Clear();
            var defs = Resources.LoadAll<GameAssetDefinition>("GameAssets");
            foreach (var def in defs)
            {
                GameAssetRegistry.Register(def);
            }
        }
    }
}