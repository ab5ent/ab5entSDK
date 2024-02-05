namespace ab5entSDK.Common.DesignPatterns
{
    public abstract class Singleton<T> where T : Singleton<T>, new()
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                _instance ??= new T();
                return _instance;
            }
        }
    }
}