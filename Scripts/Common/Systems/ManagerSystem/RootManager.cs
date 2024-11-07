using System;
using System.Collections.Generic;
using UnityEngine;

namespace ab5entSDK.Common.Systems.ManagerSystem
{
    public class RootManager : MonoBehaviour
    {
        protected Dictionary<Type, Manager> managers;

        protected virtual void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            managers = new Dictionary<Type, Manager>();

            Manager[] allManagers = GetComponentsInChildren<Manager>(true);

            for (int i = 0; i < allManagers.Length; i++)
            {
                allManagers[i].SetRoot(this);
                managers.Add(allManagers[i].GetType(), allManagers[i]);
            }

            for (int i = 0; i < allManagers.Length; i++)
            {
                allManagers[i].Initialize();
            }
        }

        public virtual T GetManager<T>() where T : Manager
        {
            if (managers.TryGetValue(typeof(T), out Manager component))
            {
                return component as T;
            }

            return null;
        }

    }
}