using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ab5entSDK.Core
{
    public static class SingletonContainer
    {
        private static readonly Dictionary<Type, ISingleton> Instances = new Dictionary<Type, ISingleton>();

        private static readonly object Lock = new object();

        static SingletonContainer()
        {
            Debug.Log("[SingletonContainer] initialized");
        }

        public static T Get<T>() where T : class, ISingleton
        {
            lock (Lock)
            {
                Type type = typeof(T);

                if (Instances.TryGetValue(type, out ISingleton existing) && existing is T result)
                {
                    return result;
                }

                return null;
            }
        }

        public static bool TryGet<T>(out T instance) where T : class, ISingleton
        {
            lock (Lock)
            {
                Type type = typeof(T);

                if (Instances.TryGetValue(type, out ISingleton existing) && existing != null)
                {
                    instance = (T)existing;
                    return true;
                }

                instance = null;
                return false;
            }
        }

        public static void Register<T>(T instance) where T : class, ISingleton
        {
            if (instance == null)
            {
                return;
            }

            lock (Lock)
            {
                Instances[typeof(T)] = instance;
                Debug.Log("[SingletonContainer] registered instance: " + typeof(T).Name);
            }
        }

        public static bool Exists<T>() where T : class, ISingleton
        {
            lock (Lock)
            {
                Type type = typeof(T);
                return Instances.TryGetValue(type, out ISingleton existing) && existing != null;
            }
        }

        public static void Unregister<T>() where T : class, ISingleton
        {
            lock (Lock)
            {
                Type type = typeof(T);
                Instances.Remove(type);
                Debug.Log("[SingletonContainer] removed instance: " + typeof(T).Name);
            }
        }
    }

    public interface ISingleton
    {
        void Register();

        void Unregister();
    }
}