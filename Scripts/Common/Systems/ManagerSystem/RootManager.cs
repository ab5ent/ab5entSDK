using UnityEngine;

namespace ab5entSDK.Common.Systems.ManagerSystem
{
    public class RootManager : MonoBehaviour
    {
        protected Managers managers;

        protected virtual void Awake()
        {
            managers = new Managers(this);
        }

        public virtual T GetManager<T>() where T : Manager
        {
            return managers.GetManager<T>();
        }
    }
}