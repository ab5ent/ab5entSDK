using System;
using ab5entSDK.Core;
using ab5entSDK.Features.CheckIn;
using ab5entSDK.Features.Inventory;
using ab5entSDK.Features.StorableData;
using Unity.VisualScripting.Dependencies.NCalc;

namespace ab5entSDK.Features.Core.Inventory
{
    public class InventoryFeature : BaseFeature
    {

        #region Fields

        private InventoryUserData _userData;

        #endregion

        #region Properties

        #endregion

        #region Methods

        public InventoryFeature(string key, IFeatureContainer container) : base(key, container)
        {
            FeatureType = EFeatureType.Inventory;
        }

        public InventoryFeature(string key) : base(key)
        {
            FeatureType = EFeatureType.Inventory;
        }

        public void Initialize()
        {
            _userData = BaseStorableData.Load<InventoryUserData>(Key);
        }

        public GameAssetInstance this[CurrencyDefinition definition] => _userData.Currency[definition];

        #endregion

        // public event Action OnInventoryChanged;
        //
        // private readonly System.Collections.Generic.Dictionary<EResourceType, TResource> _resources = new();
        //
        // public void AddResource(TResource resource)
        // {
        //     if (_resources.ContainsKey(resource.TypeOfResource))
        //     {
        //         _resources[resource.TypeOfResource] = resource;
        //     }
        //     else
        //     {
        //         _resources.Add(resource.TypeOfResource, resource);
        //     }
        //
        //     OnInventoryChanged?.Invoke();
        // }
        //
        // public bool RemoveResource(EResourceType resourceType)
        // {
        //     if (_resources.ContainsKey(resourceType))
        //     {
        //         _resources.Remove(resourceType);
        //         OnInventoryChanged?.Invoke();
        //         return true;
        //     }
        //
        //     return false;
        // }
        //
        // public TResource GetResource(EResourceType resourceType)
        // {
        //     if (_resources.TryGetValue(resourceType, out var resource))
        //     {
        //         return resource;
        //     }
        //
        //     return null;
        // }
        //
        // public System.Collections.Generic.IReadOnlyDictionary<EResourceType, TResource> GetAllResources()
        // {
        //     return _resources;
        // }

        private void ValidateInventoryData()
        {

        }

        public override void ResetUserData()
        {
            _userData.ResetData();
        }
    }

}