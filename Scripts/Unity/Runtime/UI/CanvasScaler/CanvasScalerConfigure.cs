using System;
using System.Collections.Generic;
using UnityEngine;

namespace ab5entSDK.Runtime.UI
{
    namespace CustomizedCanvasScaler
    {
        [CreateAssetMenu(fileName = "CanvasScalerConfigure", menuName = "UI/Configures/CanvasScalerConfigure")]
        public class CanvasScalerConfigure : ScriptableObject
        {
            [Serializable]
            public class CanvasScaleConfigure
            {
                [SerializeField]
                private Vector2 aspectRatio;

                public Vector2 AspectRatio => aspectRatio;

                [SerializeField, Range(0.0f, 1.0f)]
                private float matchWidthOrHeight;

                public float MatchWidthOrHeight => matchWidthOrHeight;
            }

            [field: SerializeField]
            public Vector2 TargetResolution { get; set; }

            [field: SerializeField]
            private Vector2 TargetAspectRatio { get; set; }

            [SerializeField]
            private List<CanvasScaleConfigure> canvasScaleConfigures;

            private int GCD(int a, int b)
            {
                return (b == 0) ? a : GCD(b, a % b);
            }

            public float GetMatchWidthOrHeight()
            {
                int screenWidth = Screen.width;
                int screenHeight = Screen.height;

                int factor = GCD(screenWidth, screenHeight);

                int wFactor = screenWidth / factor;
                int hFactor = screenHeight / factor;

                Debug.Log($"Width: {screenWidth},  Height: {screenHeight}, Aspect Ratio: {wFactor}:{hFactor} ({(float)screenWidth / screenHeight})");

                float matchWidthOrHeight = 0;

                if (GetCanvasScaleConfigures(wFactor, hFactor, out matchWidthOrHeight))
                {
                    Debug.Log("Use CanvasScalerConfigure MatchWidthOrHeight: " + matchWidthOrHeight);
                    return matchWidthOrHeight;
                }

                //if (GetNearestCanvasScaleConfigure(new Vector2(screenWidth, screenHeight), out matchWidthOrHeight))
                //{
                //Debug.Log("Use Nearest CanvasScalerConfigure MatchWidthOrHeight: " + matchWidthOrHeight);
                //return matchWidthOrHeight;
                //}

                Debug.Log("Use MatchWidthOrHeight Default: 0");
                return 0;
            }

            private bool GetCanvasScaleConfigures(int wFactor, int hFactor, out float result)
            {
                result = -1;

                for (int i = 0; i < canvasScaleConfigures.Count; i++)
                {
                    Vector2 aspectRatio = canvasScaleConfigures[i].AspectRatio;

                    if (wFactor == (int)aspectRatio.x && hFactor == (int)aspectRatio.y)
                    {
                        float matchWidthOrHeight = canvasScaleConfigures[i].MatchWidthOrHeight;
                        result = matchWidthOrHeight;
                        return true;
                    }
                }

                return false;
            }

            //private bool GetNearestCanvasScaleConfigure(Vector2 targetAspectRatio, out float result)
            //{

            //}
        }
    }
}