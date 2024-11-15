using UnityEngine;
using UnityEngine.UI;

namespace ab5entSDK.Runtime.UI
{
    namespace CustomizedCanvasScaler
    {
        public class AutomaticCanvasScaler : MonoBehaviour
        {
            [SerializeField]
            private CanvasScalerConfigure canvasScalerConfigure;

            private void Awake()
            {
                Initialize();
            }

            protected virtual void Initialize()
            {
                CanvasScaler canvasScaler = GetComponent<CanvasScaler>();

                bool isScaleWithScreenSizeMode = canvasScaler.uiScaleMode == CanvasScaler.ScaleMode.ScaleWithScreenSize;
                bool isScreenMatchWidthOrHeight = canvasScaler.screenMatchMode == CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
                bool isMatchResolution = canvasScaler.referenceResolution == canvasScalerConfigure.TargetResolution;

                if (isScaleWithScreenSizeMode && isScreenMatchWidthOrHeight && isMatchResolution)
                {
                    canvasScaler.matchWidthOrHeight = canvasScalerConfigure.GetMatchWidthOrHeight();
                }
            }
        }
    }
}