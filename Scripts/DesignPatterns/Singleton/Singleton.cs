namespace ab5entSDK
{
    namespace DesignPatterns
    {
        public abstract class Singleton<T> where T : Singleton<T>, new()
        {
            private static T instance;

            public static T Instance
            {
                get
                {
                    instance ??= new T();
                    return instance;
                }
            }
        }
    }
}