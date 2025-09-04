using ab5entSDK.Core;

namespace ab5entSDK.System
{
    public abstract class ResourceSystem : ISingleton
    {

        protected ResourceSystem()
        {
            Register();
        }

        #region Methods

        #region ISingleton Implementation

        public void Register()
        {
            SingletonContainer.Register(this);
        }

        public void Unregister()
        {
            SingletonContainer.Unregister(this);
        }

        #endregion

        #endregion

    }
}