using UnityEngine;

namespace ab5entSDK.Core
{
    [CreateAssetMenu(menuName = "Game/Assets/Assets", fileName = "New Game Asset", order = 0)]
    public class GameAssetDefinition : ScriptableObject
    {
        [SerializeField, HideInInspector] protected string id;

        public string Id => id;

#if UNITY_EDITOR

        protected virtual void OnValidate()
        {
            if (!string.IsNullOrEmpty(id))
            {
                return;
            }

            id = name;
            UnityEditor.EditorUtility.SetDirty(this);
        }

#endif
    }
}