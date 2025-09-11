using UnityEngine;

namespace ab5entSDK.Core
{
    [CreateAssetMenu(fileName = "Currency", menuName = "Game/Assets/Currency", order = 1)]
    public class CurrencyDefinition : GameAssetDefinition
    {
        #if UNITY_EDITOR

        protected override void OnValidate()
        {
            base.OnValidate();
            typeOfAsset = GameAssetType.Currency;
            scope = GameAssetScope.Global;
        }


        #endif
    }
}