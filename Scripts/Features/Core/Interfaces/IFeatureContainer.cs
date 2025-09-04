namespace ab5entSDK.Features.Core
{
    public interface IFeatureContainer
    {
        public string Key { get; protected set; }

        public void AddFeature(IFeature feature);
    }
}