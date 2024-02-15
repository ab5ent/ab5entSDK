namespace ab5entSDK.Common.DesignPatterns.CreationalPatterns
{
    public interface IReusableObject<T>
    {
        public void SetReusablePool(IReusablePool<T> reusablePool);

        public void OnCreate();

        public void OnGet();

        public void OnRelease();

        public void OnDispose();
    }
}