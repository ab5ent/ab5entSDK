namespace ab5entSDK.Feature.Base
{
    public interface IFeature
    {
        public EFeatureType FeatureType { get; }

        public IFeatureContainer Container { get; }

        public string GetKey();

        public void ResetUserData();
    }
}