using UnityEngine;

namespace ab5entSDK.Common.DesignPatterns.CreationalPatterns.Singleton
{
    public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : Component
    {
        [SerializeField]
        protected bool enableDontDestroyOnLoad;

        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance)
                {
                    return _instance;
                }

                T instanceInScene = FindObjectOfType<T>();

                if (instanceInScene != null)
                {
                    CreateInstance(instanceInScene);
                    return _instance;
                }

                T instancePrefab = Resources.Load<T>($"{typeof(T).Name}");

                if (instancePrefab != null)
                {
                    CreateInstance(Instantiate(instancePrefab));
                    return _instance;
                }

                GameObject newInstance = new GameObject();
                CreateInstance(newInstance.AddComponent<T>());
                return _instance;
            }
        }

        private static void CreateInstance(T instance)
        {
            _instance = instance;
            _instance.name = $"<Singleton>{typeof(T).Name}";
        }

        protected virtual void Awake()
        {
            if (_instance == null)
            {
                CreateInstance(this as T);
            }
            else if (_instance != this)
            {
                Destroy(this);
                return;
            }

            if (enableDontDestroyOnLoad)
            {
                DontDestroyOnLoad(this);
            }
        }
    }
}