using UnityEngine;

namespace ab5entSDK.Core
{
    [CreateAssetMenu(menuName = "Game/Assets/Assets", fileName = "New Game Asset", order = 0)]
    public class GameAssetDefinition : ScriptableObject
    {
        [SerializeField, HideInInspector] protected string id;

        [SerializeField] protected GameAssetType typeOfAsset;

        [SerializeField] protected GameAssetScope scope;

        public string Id => id;

        public GameAssetScope Scope => scope;

        public GameAssetType TypeOfAssetOfGameAsset => typeOfAsset;

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