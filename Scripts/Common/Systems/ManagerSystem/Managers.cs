using System;
using System.Collections.Generic;
using UnityEngine;

namespace ab5entSDK.Common.Systems.ManagerSystem
{
    public class Managers : Dictionary<Type, Manager>
    {
        public Managers(RootManager rootManager)
        {
            Manager[] managers = rootManager.GetComponentsInChildren<Manager>(true);

            for (int i = 0; i < managers.Length; i++)
            {
                managers[i].SetRoot(rootManager);
                Add(managers[i].GetType(), managers[i]);
            }

            for (int i = 0; i < managers.Length; i++)
            {
                managers[i].Initialize();
            }
        }

        public virtual T GetManager<T>() where T : Manager
        {
            if (TryGetValue(typeof(T), out Manager component))
            {
                return component as T;
            }
            else
            {
                Debug.LogWarning($"Manager of type {typeof(T)} not found.");
                return null;
            }
        }
    }
}