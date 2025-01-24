using System;
using UnityEngine;

namespace ab5entSDK.Common.Systems
{
    [Serializable]
    public class AudioInformation
    {
        [field: SerializeField]
        public AudioId Id { get; private set; }

        [field: SerializeField]
        public AudioClip Clip { get; private set; }
    }
}