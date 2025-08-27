namespace ab5entSDK.Feature.Base
{
    public interface IFeature
    {
        public IFeatureContainer Container { get; set; }

        public string GetKey();

        public void SetContainer(IFeatureContainer container);

    }
}