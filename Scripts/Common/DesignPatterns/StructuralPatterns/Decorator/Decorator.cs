using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ab5entSDK.Common.DesignPatterns.StructuralPatterns.Decorator
{
    public abstract class Decorator<T> : IWrapper
    {
        protected T Wrapper;

        protected Decorator(T wrapper)
        {
            Wrapper = wrapper;
        }
    }
}