namespace ab5entSDK.Common.DesignPatterns.CreationalPatterns.ReusablePool
{
    public interface IReusablePool<T>
    {
        public int MaxPoolSize { get; set; }

        public int Count { get; }

        public int CountActive { get; }

        public int CountInactive { get; }

        public T Create();

        public T Get();

        public void Release(in T reusableObject);

        public void Discard(in T discardObject);
    }
}