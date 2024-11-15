using ab5entSDK.ExtendedEditor.Attributes;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ab5entSDK.Common.Systems.ManagerSystem
{
    public class ListManagers : MonoBehaviour
    {
        [SerializeField, Preview]
        private Manager[] managers;

        protected Dictionary<Type, Manager> managerDictionary;

        public void Initialize(RootManager rootManager)
        {
            managerDictionary = new Dictionary<Type, Manager>();
            managers = GetComponentsInChildren<Manager>(true);

            for (int i = 0; i < managers.Length; i++)
            {
                managers[i].SetRoot(rootManager);
                managerDictionary.Add(managers[i].GetType(), managers[i]);
            }

            for (int i = 0; i < managers.Length; i++)
            {
                managers[i].Initialize();
            }
        }

        public virtual T GetManager<T>() where T : Manager
        {
            if (managerDictionary.TryGetValue(typeof(T), out Manager component))
            {
                return component as T;
            }

            return null;
        }
    }
}