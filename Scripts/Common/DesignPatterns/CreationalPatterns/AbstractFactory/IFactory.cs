namespace ab5entSDK.Common.DesignPatterns.CreationalPatterns
{
    public interface IFactory<out T> where T : IProduct
    {
        public T Create();
    }
}