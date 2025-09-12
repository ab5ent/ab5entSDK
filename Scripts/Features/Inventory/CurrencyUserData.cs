using System;
using System.Collections.Generic;
using ab5entSDK.Core;
using ab5entSDK.Features.StorableData;
using UnityEngine;

namespace ab5entSDK.Features.Inventory
{
    [Serializable]
    public class CurrencyUserData : BaseStorableData
    {
        #region Fields

        [field: SerializeField] private readonly List<GameAssetInstance> _data = new List<GameAssetInstance>();

        private readonly Dictionary<string, GameAssetInstance> _global = new Dictionary<string, GameAssetInstance>();

        #endregion

        #region Properties

        public GameAssetInstance this[CurrencyDefinition definition]
        {
            get
            {
                if (_global.TryGetValue(definition.Id, out GameAssetInstance instance))
                {
                    return instance;
                }

                instance = new GameAssetInstance(definition);

                _global.Add(definition.Id, instance);
                _data.Add(instance);

                DataChanged();
                return instance;
            }
        }

        public void ChangeQuantity(CurrencyDefinition definition, int amount)
        {
            this[definition].ChangeQuantity(amount);
            DataChanged();
        }

        public bool HasEnough(CurrencyDefinition definition, float amount = 1)
        {
            return this[definition].Quantity >= amount;
        }

        #endregion

        #region Methods

        public override void ResetData()
        {
            _global.Clear();
            base.ResetData();
        }

        public override void PopulateFields()
        {
            foreach (GameAssetInstance gameAssetInstance in _data)
            {
                _global[gameAssetInstance.DefinitionId] = gameAssetInstance;
            }
        }

        #endregion
    }
}