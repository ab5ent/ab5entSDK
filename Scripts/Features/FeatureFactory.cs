using System;
using ab5entSDK.Features.CheckIn;

namespace ab5entSDK.Features.Core
{
    public class FeatureFactory<TReward> where TReward : class
    {
        public FeatureFactory()
        {
        }

        public IFeature Create(string key, IFeatureContainer container, EFeatureType featureType)
        {
            switch (featureType)
            {
                case EFeatureType.None:
                {
                    break;
                }
                case EFeatureType.CheckIn:
                {
                    return new CheckInFeature(key, container);
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(featureType), featureType, null);
            }

            return null;
        }

        public IFeature Create(string key, EFeatureType featureType)
        {
            switch (featureType)
            {
                case EFeatureType.None:
                {
                    break;
                }
                case EFeatureType.CheckIn:
                {
                    return new CheckInFeature(key);
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(featureType), featureType, null);
            }

            return null;
        }
    }
}