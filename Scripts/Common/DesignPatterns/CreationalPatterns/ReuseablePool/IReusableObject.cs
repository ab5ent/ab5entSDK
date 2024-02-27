namespace ab5entSDK.Common.DesignPatterns.CreationalPatterns.ReusablePool
{
    public interface IReusableObject
    {
        public void SetReusablePool(object reusablePool);

        public void OnCreate();

        public void OnGet();

        public void OnRelease();

        public void OnDiscard();
    }
}