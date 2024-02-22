namespace ab5entSDK.Common.DesignPatterns.CreationalPatterns.Factory
{
    public interface IFactory<out T> where T : IProduct
    {
        public T Create();
    }
}