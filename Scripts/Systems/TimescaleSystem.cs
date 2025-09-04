using System.Collections;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

namespace ab5entSDK.Common.Systems
{
    public class TimescaleSystem : MonoBehaviour
    {
        public static bool IsPaused() => Time.timeScale <= 0;

        public void Pause(float duration = 0)
        {
            StartCoroutine(ChangeTimeScale(1, 0, duration));
        }

        public void Resume(float duration = 0)
        {
            StartCoroutine(ChangeTimeScale(0, 1, duration));
        }

        private IEnumerator ChangeTimeScale(float startValue, float endValue, float duration)
        {
            float elapsedTime = 0;

            while (elapsedTime < duration)
            {
                float t = startValue > endValue
                    ? Easing.InSine(elapsedTime / duration)
                    : Easing.OutSine(elapsedTime / duration);
                Time.timeScale = Mathf.Lerp(startValue, endValue, t);
                yield return null;

                elapsedTime += Time.unscaledDeltaTime;
            }

            Time.timeScale = endValue;
        }
    }
}