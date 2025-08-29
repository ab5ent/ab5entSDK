namespace ab5entSDK.Feature.Base
{
    public abstract class BaseFeature : IFeature
    {
        public EFeatureType FeatureType { get; protected set; } = EFeatureType.None;

        public IFeatureContainer Container { get; protected set; }

        public string GetKey()
        {
            return GetType().Name.ToLower();
        }

        public abstract void ResetUserData();

        protected BaseFeature()
        {
        }
    }
}