namespace ab5entSDK.Feature.Base
{
    public interface IFeatureContainer
    {
        public string Key { get; protected set; }

        public void AddFeature(IFeature feature);

        public T GetFeature<T>() where T : class, IFeature;
    }
}