namespace ab5entSDK.Common.DesignPatterns.CreationalPatterns
{
    public interface IReusablePool<T>
    {
        public int Count { get; }

        public int CountActive { get; }

        public int CountInactive { get; }

        public void SetMaxPoolSize(int value);

        public T Create();

        public T Get();

        public void Release(in T reusable);

        public void Dispose();
    }
}