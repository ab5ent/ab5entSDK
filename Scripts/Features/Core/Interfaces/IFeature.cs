namespace ab5entSDK.Features.Core
{
    public interface IFeature
    {
        public EFeatureType FeatureType { get; }

        public IFeatureContainer Container { get; }

        public string Key { get; }

        public void SetContainer(IFeatureContainer container);

        public void ResetUserData();
    }
}