using System;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace ab5entSDK.Core
{
    [CreateAssetMenu(fileName = "GameAssets", menuName = "GameAssets/Root", order = 0)]
    public class GameAssets : ScriptableObject
    {
        [Serializable]
        private class DefinitionGroup
        {
            [field: SerializeField] public string Name { get; private set; }

            [field: SerializeField] public GameAssetDefinition[] Definitions { get; set; }

            public void CreateDefinitions(GameAssetDefinition[] definitions)
            {
                Definitions = definitions;
            }
        }

        [SerializeField] private DefinitionGroup[] group;

        [ContextMenu("Get Definitions")]
        private void GetDefinitions()
        {
            for (int i = 0; i < group.Length; i++)
            {
                string[] guids = AssetDatabase.FindAssets($"t:{group[i].Name}Definition");
                group[i].CreateDefinitions(new GameAssetDefinition[guids.Length]);

                for (int j = 0; j < guids.Length; j++)
                {
                    string path = AssetDatabase.GUIDToAssetPath(guids[j]);
                    group[i].Definitions[j] = AssetDatabase.LoadAssetAtPath<GameAssetDefinition>(path);
                }

                Debug.Log($"Found {group[i].Definitions.Length} Definitions");
            }
        }

        [ContextMenu("Generate GameAssetDefinitions")]
        private void GenerateGameAssetsDefinitions()
        {
            string classContainerName = "GameAssetDefinitions";

            string defaultNamespace = EditorSettings.projectGenerationRootNamespace;
            if (string.IsNullOrEmpty(defaultNamespace))
            {
                defaultNamespace = "DefaultNamespace";
            }

            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"// Auto-generated {classContainerName}");

            sb.AppendLine();
            sb.AppendLine("using ab5entSDK.Core;");
            sb.AppendLine();

            sb.AppendLine($"namespace {defaultNamespace}");
            sb.AppendLine("{");
            sb.AppendLine($"    public static class {classContainerName}");
            sb.AppendLine("    {");

            foreach (DefinitionGroup definitionGroup in group)
            {
                sb.AppendLine($"        public static class {definitionGroup.Name}");
                sb.AppendLine("        {");

                for (var index = 0; index < definitionGroup.Definitions.Length; index++)
                {
                    GameAssetDefinition def = definitionGroup.Definitions[index];

                    if (def == null)
                    {
                        continue;
                    }

                    string typeName = def.GetType().Name;
                    string safeName = def.name.Replace(" ", "_"); // basic sanitization
                    string line = $"            public static readonly {typeName} {safeName} = GameAssetRegistry.Get<{typeName}>(\"{def.Id}\");";
                    sb.AppendLine(line);

                    if (index < definitionGroup.Definitions.Length - 1)
                    {
                        sb.AppendLine();
                    }
                }

                sb.AppendLine("        }");
            }

            sb.AppendLine("    }");
            sb.AppendLine("}");

            string folderPath = "Assets/Scripts/Generated";

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            string filePath = Path.Combine(folderPath, $"{classContainerName}.cs");
            File.WriteAllText(filePath, sb.ToString());

            AssetDatabase.Refresh();

            Debug.Log($"{classContainerName} generated at: {filePath} (namespace: {defaultNamespace})");
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