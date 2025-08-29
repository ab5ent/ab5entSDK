using System;
using ab5entSDK.Features.CheckIn;

namespace ab5entSDK.Feature.Base
{
    public class FeatureFactory<TReward> where TReward : struct
    {
        public FeatureFactory()
        {
        }

        public IFeature Create(EFeatureType featureType)
        {
            switch (featureType)
            {
                case EFeatureType.None:
                    break;
                case EFeatureType.CheckIn:
                {
                    return new CheckInFeature<TReward>();
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(featureType), featureType, null);
            }

            return null;
        }
    }
}