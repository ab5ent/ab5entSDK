using System;
using ab5entSDK.Features.StorableData;
using UnityEngine;

namespace ab5entSDK.Features.Inventory
{
    [Serializable]
    public class InventoryUserData : BaseStorableData
    {
        [field: SerializeField] private CurrencyUserData currencyUserData;

        public CurrencyUserData Currency => currencyUserData;

        public override void Initialize(string key = "", IStorageManager storageManager = null, IStorableData storableData = null)
        {
            base.Initialize(key, storageManager, storableData);

            currencyUserData ??= new CurrencyUserData();
            currencyUserData.Initialize(Key, storageManager, this);
        }

        public override void PopulateFields()
        {
        }

        public override void ResetData()
        {
            currencyUserData.ResetData();
            base.ResetData();
        }
    }
}