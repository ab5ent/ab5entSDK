using UnityEngine;

namespace ab5entSDK.Common.Systems.ManagerSystem
{
    [RequireComponent(typeof(ListManagers))]
    public class RootManager : MonoBehaviour
    {
        protected ListManagers listManagers;

        protected virtual void Awake()
        {
            listManagers = GetComponent<ListManagers>();
            listManagers.Initialize(this);
        }

        public virtual T GetManager<T>() where T : Manager
        {
            return listManagers.GetManager<T>();
        }
    }
}