namespace ab5entSDK.Features.Core
{
    public abstract class BaseFeature : IFeature
    {

        #region Fields

        private readonly string _key;

        private IFeatureContainer _container;

        #endregion

        #region Properties

        public EFeatureType FeatureType { get; protected set; }

        public IFeatureContainer Container { get; protected set; }

        public string Key => string.IsNullOrEmpty(_key) ? FeatureType.ToString() : _key;

        #endregion

        #region Constructors

        protected BaseFeature(string key, IFeatureContainer container)
        {
            _key = key;
            SetContainer(container);
        }

        protected BaseFeature(string key)
        {
            _key = key;
        }

        #endregion

        #region Methods

        public void SetContainer(IFeatureContainer container)
        {
            Container = container;
        }

        public abstract void ResetUserData();

        #endregion

    }
}