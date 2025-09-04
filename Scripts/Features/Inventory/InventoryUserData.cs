using System.Collections.Generic;
using ab5entSDK.Features.StorableData;
using UnityEngine;

namespace ab5entSDK.Features.Inventory
{
    public class InventoryUserData : BaseStorableData
    {

        #region Fields

        [field: SerializeField] private List<GameAssetInstance> _listGameAssets;

        private Dictionary<int, GameAssetInstance> _dictionaryGameAssets;

        #endregion

        #region Properties

        public void AddGameAssetInstance(GameAssetInstance gameAssetInstance)
        {
            if (gameAssetInstance == null) return;

            int hashCode = gameAssetInstance.GetHashCode();

            if (!_dictionaryGameAssets.TryAdd(hashCode, gameAssetInstance))
            {
                Debug.LogWarning($"GameAssetInstance with hash code {hashCode} already exists in the inventory.");
                return;
            }

            _listGameAssets.Add(gameAssetInstance);
            DataChanged();
        }

        public void SetGameAssetQuantity(int hashCode, int newQuantity)
        {
            if (_dictionaryGameAssets.ContainsKey(hashCode))
            {
                _dictionaryGameAssets[hashCode].SetQuantity(newQuantity);
                DataChanged();
            }
        }

        public void ModifyGameAssetQuantity(int hashCode, int modifyQuantity)
        {
            if (_dictionaryGameAssets.ContainsKey(hashCode))
            {
                _dictionaryGameAssets[hashCode].ModifyQuantity(modifyQuantity);
                DataChanged();
            }
        }

        public void GetGameAssetInstance(int hashCode, out GameAssetInstance gameAssetInstance)
        {
            _dictionaryGameAssets.TryGetValue(hashCode, out gameAssetInstance);
        }       

        #endregion

        #region Methods

        public override void Initialize(string key = "", IStorageManager storageManager = null, IStorableData storableData = null)
        {
            base.Initialize(key, storageManager, storableData);

            _dictionaryGameAssets = new Dictionary<int, GameAssetInstance>();

            foreach (GameAssetInstance gameAssetInstance in _listGameAssets)
            {
                _dictionaryGameAssets[gameAssetInstance.GetHashCode()] = gameAssetInstance;
            }

        }

        public override void ResetData()
        {
            _listGameAssets.Clear();
            _dictionaryGameAssets.Clear();

            base.ResetData();
        }

        #endregion

    }
}