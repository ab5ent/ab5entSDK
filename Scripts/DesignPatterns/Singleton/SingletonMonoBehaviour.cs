using UnityEngine;

namespace ab5entSDK
{
    namespace DesignPatterns
    {
        public class SingletonMonoBehaviour<T> : MonoBehaviour where T : Component
        {
            [SerializeField]
            protected bool EnableDontDestroyOnLoad;

            private static T instance;

            public static T Instance
            {
                get
                {
                    if (instance)
                    {
                        return instance;
                    }

                    T instanceInScene = FindObjectOfType<T>();

                    if (instanceInScene != null)
                    {
                        CreateInstance(instanceInScene);
                        return instance;
                    }

                    T instancePrefab = Resources.Load<T>($"{typeof(T).Name}");

                    if (instancePrefab != null)
                    {
                        CreateInstance(Instantiate(instancePrefab));
                        return instance;
                    }

                    GameObject newInstance = new GameObject();
                    CreateInstance(newInstance.AddComponent<T>());
                    return instance;
                }
            }

            private static void CreateInstance(T _object)
            {
                instance = _object;
                instance.name = $"<Singleton>{typeof(T).Name}";
                if ((instance as SingletonMonoBehaviour<T>).EnableDontDestroyOnLoad)
                {
                    DontDestroyOnLoad(instance);
                }
            }

            protected virtual void Awake()
            {
                if (instance == null)
                {
                    CreateInstance(this as T);
                }
                else if (instance != this)
                {
                    Destroy(this);
                }
            }
        }
    }
}