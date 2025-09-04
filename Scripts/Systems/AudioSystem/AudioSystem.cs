using UnityEngine;

namespace ab5entSDK.Common.Systems
{
    public class AudioSystem : MonoBehaviour
    {
        [Header("BGM")]
        [SerializeField]
        protected AudioSource bgmAudioSource;

        [SerializeField]
        protected AudioData bgmAudioData;

        [Header("UI Sound Effect")]
        [SerializeField]
        protected AudioSource uiSoundEffectAudioSource;

        [SerializeField]
        protected AudioData uiSFXAudioData;

        private void Initialize()
        {
            bgmAudioData.Initialize();
            uiSFXAudioData.Initialize();
        }

        public void PlayBackgroundMusic(AudioId bgmAudioId)
        {
            AudioClip bgmAudioClip = bgmAudioData.GetAudioClip(bgmAudioId);

            if (bgmAudioClip == null)
            {
                return;
            }

            bgmAudioSource.loop = true;
            bgmAudioSource.clip = bgmAudioClip;
            bgmAudioSource.Play();
        }

        public void PlayUISoundEffectt(AudioId uiSoundEffectAudioId)
        {
            AudioClip uiSoundEffectAudioClip = uiSFXAudioData.GetAudioClip(uiSoundEffectAudioId);

            if (uiSoundEffectAudioClip == null)
            {
                return;
            }

            bgmAudioSource.PlayOneShot(uiSoundEffectAudioClip);
        }
    }
}