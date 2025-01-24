using UnityEngine;

namespace ab5entSDK.Common.Systems.ManagerSystem
{
    public class Manager : MonoBehaviour
    {
        private RootManager rootManager;

        public void SetRoot(RootManager newRootManager)
        {
            rootManager = newRootManager;
        }

        public virtual void Initialize()
        {
        }

        public virtual void DeInitialize()
        {
        }

        public T GetManager<T>() where T : Manager
        {
            return rootManager.GetManager<T>();
        }
    }
}