using System;
using UnityEngine;

namespace ab5entSDK.Core
{
    [Serializable]
    public struct Reward
    {
        [field: SerializeField]
        public GameAssetDefinition GameAsset { get; private set; }

        [field: SerializeField]
        public int Amount { get; private set; }
    }
}