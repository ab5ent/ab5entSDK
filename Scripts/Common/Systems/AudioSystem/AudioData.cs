using System.Collections.Generic;
using UnityEngine;

namespace ab5entSDK.Common.Systems
{
    [CreateAssetMenu(fileName = "AudioData", menuName = "Data/Audio/AudioData")]
    public class AudioData : ScriptableObject
    {
        private Dictionary<AudioId, AudioClip> audioDictionary;

        [SerializeField] private AudioInformation[] audioInformations;

        [SerializeField] private AudioType audioType;

        public void Initialize()
        {
            audioDictionary = new Dictionary<AudioId, AudioClip>();
            for (int i = 0; i < audioInformations.Length; i++)
            {
                audioDictionary.Add(audioInformations[i].Id, audioInformations[i].Clip);
            }
        }

        public AudioClip GetAudioClip(AudioId id)
        {
            if (audioDictionary.TryGetValue(id, out AudioClip result))
            {
                return result;
            }
            else
            {
                Debug.LogWarning($"Cannot find audio id {id}");
                return null;
            }
        }
    }
}