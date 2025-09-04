using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace ab5entSDK.Tools
{
    public class SceneBrowser : EditorWindow
    {
        private Vector2 scrollPos;
        private List<string> scenePaths;
        private List<string> sceneNames;
        private string searchFilter = "";

        private const float rowHeight = 22f; // Row height
        private const float headerHeight = 40f; // Toolbar + padding

        private float maxLabelWidth = 200f; // Dynamically calculated

        [MenuItem("Tools/Ab5snt/Scene Browser")]
        public static void ShowWindow()
        {
            var window = GetWindow<SceneBrowser>("Scene Browser");
            window.minSize = new Vector2(300, 100);
        }

        private void OnEnable()
        {
            RefreshScenes();
        }

        private void RefreshScenes()
        {
            scenePaths = new List<string>();
            sceneNames = new List<string>();

            string[] guids = AssetDatabase.FindAssets("t:Scene");

            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                scenePaths.Add(path);
                sceneNames.Add(Path.GetFileNameWithoutExtension(path));
            }

            CalculateMaxLabelWidth();
        }

        private void CalculateMaxLabelWidth()
        {
            maxLabelWidth = 200f;
            GUIStyle labelStyle = EditorStyles.miniLabel;

            foreach (string path in scenePaths)
            {
                Vector2 size = labelStyle.CalcSize(new GUIContent(path));

                if (size.x > maxLabelWidth)
                    maxLabelWidth = size.x;
            }
        }

        private void OnGUI()
        {
            // Adjust window size dynamically (height and width)
            int visibleScenes = Mathf.Clamp(string.IsNullOrEmpty(searchFilter) ? scenePaths.Count : GetFilteredCount(), 3, 10);
            float wantedHeight = (visibleScenes * rowHeight);

            float wantedWidth = 200f + maxLabelWidth + 40f; // button + label + margin
            wantedWidth = Mathf.Clamp(wantedWidth, 300, 1000);

            maxSize = new Vector2(wantedWidth, wantedHeight);

            // Search bar
            GUILayout.BeginHorizontal(EditorStyles.toolbar);

            searchFilter = GUILayout.TextField(searchFilter, GUI.skin.FindStyle("ToolbarSearchTextField"), GUILayout.Width(200));

            if (GUILayout.Button("x", EditorStyles.toolbarButton, GUILayout.Width(20)))
            {
                searchFilter = "";
                GUI.FocusControl(null);
            }

            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Refresh", EditorStyles.toolbarButton))
            {
                RefreshScenes();
            }

            GUILayout.EndHorizontal();

            GUILayout.Space(5);

            // Scrollable list
            scrollPos = GUILayout.BeginScrollView(scrollPos);

            for (int i = 0; i < scenePaths.Count; i++)
            {
                if (!string.IsNullOrEmpty(searchFilter) && !sceneNames[i].ToLower().Contains(searchFilter.ToLower()))
                    continue;

                EditorGUILayout.BeginHorizontal(GUILayout.Height(rowHeight));

                if (GUILayout.Button(sceneNames[i], GUILayout.Width(180)))
                {
                    if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                    {
                        EditorSceneManager.OpenScene(scenePaths[i]);
                    }
                }

                EditorGUILayout.LabelField(scenePaths[i], EditorStyles.miniLabel, GUILayout.Width(maxLabelWidth));

                EditorGUILayout.EndHorizontal();
            }

            GUILayout.EndScrollView();
        }

        private int GetFilteredCount()
        {
            int count = 0;

            for (int i = 0; i < sceneNames.Count; i++)
            {
                if (sceneNames[i].ToLower().Contains(searchFilter.ToLower()))
                    count++;
            }

            return count;
        }
    }
}