using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ab5entSDK.Common.SharedComponents
{
    [RequireComponent(typeof(Text))]
    public class NumbericText : MonoBehaviour
    {
        protected const float MINIMUM_CHANGE_THRESHOLD = 3, DEFAULT_CHANGE_DURATION = 1;

        protected static readonly List<NumbericText> numbericTexts = new List<NumbericText>();

        public static void SetValue(string id, int value, float duration = 0)
        {
            foreach (NumbericText numbericText in numbericTexts)
            {
                if (numbericText && numbericText.id == id)
                {
                    numbericText.SetValue(value, duration);
                }
            }
        }

        [SerializeField]
        private string id = "";

        protected Text textComponent;

        protected int current, start, end;

        protected float duration, timer;

        protected virtual void Awake()
        {
            textComponent = GetComponent<Text>();
            numbericTexts.Add(this);
        }

        protected virtual void OnEnable()
        {
            PrepareAnimation();
        }

        protected virtual void Update()
        {
            ProcessAnimation();
        }

        protected virtual void OnDestroy()
        {
            numbericTexts.Remove(this);
        }

        protected virtual void PrepareAnimation()
        {
            if (current == end || Mathf.Abs(start - end) < MINIMUM_CHANGE_THRESHOLD)
            {
                current = end;
                return;
            }

            start = current;
            timer = 0;
        }

        protected virtual void ProcessAnimation()
        {
            if (current == end)
            {
                return;
            }

            if (Mathf.Abs(start - end) < MINIMUM_CHANGE_THRESHOLD)
            {
                current = end;
            }

            if (timer < duration)
            {
                timer += Time.deltaTime;
                current = Mathf.CeilToInt(Mathf.Lerp(start, end, timer / duration));
                textComponent.text = current.ToString();

                if (timer >= duration)
                {
                    duration = 0;
                }
            }
        }

        public virtual void SetValue(int value, float newDuration = 0)
        {
            start = current;
            end = value;
            duration = newDuration <= 0 ? DEFAULT_CHANGE_DURATION : newDuration;
            timer = 0;
        }
    }
}