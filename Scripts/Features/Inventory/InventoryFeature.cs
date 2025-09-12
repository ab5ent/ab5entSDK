using ab5entSDK.Core;
using ab5entSDK.Features.Inventory;
using ab5entSDK.Features.StorableData;

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

        private void ValidateInventoryData()
        {
        }

        public override void ResetUserData()
        {
            _userData.ResetData();
        }

        #region Currency

        public GameAssetInstance GetCurrency(CurrencyDefinition currencyDefinition)
        {
            return _userData.Currency[currencyDefinition];
        }

        public void ChangeCurrencyQuantity(CurrencyDefinition currencyDefinition, int amount)
        {
            _userData.Currency.ChangeQuantity(currencyDefinition, amount);
        }

        public bool HasEnoughCurrency(CurrencyDefinition currencyDefinition, int amount)
        {
            return _userData.Currency.HasEnough(currencyDefinition, amount);
        }

        #endregion

        #endregion
    }
}