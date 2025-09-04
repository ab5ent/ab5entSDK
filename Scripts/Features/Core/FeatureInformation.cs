using System;
using UnityEngine;

namespace ab5entSDK.Features.Core
{
    [Serializable]
    public abstract class FeatureInformation : ScriptableObject
    {
        [field: SerializeField]
        public EFeatureType TypeOfFeature { get; private set; }
    }
}