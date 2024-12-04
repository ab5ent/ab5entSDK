using System;
using System.IO;
using UnityEngine;

namespace ab5entSDK.Tools
{
    public class ScreenCaptureTool : MonoBehaviour
    {
        [SerializeField]
        private string folderName;

        [SerializeField]
        private KeyCode keyCode = KeyCode.F3;

        private void Update()
        {
            if (Input.GetKeyDown(keyCode))
            {
                CaptureScreenShot();
            }
        }

        [ContextMenu("Capture screenshot")]
        private void CaptureScreenShot()
        {
            string directoryName = Application.persistentDataPath + $"/Screenshots/{folderName}";

            if (!Directory.Exists(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }

            string storagePath = string.IsNullOrEmpty(folderName) ? $"{Application.persistentDataPath}/Screenshots" : $"{Application.persistentDataPath}/Screenshots/{folderName}";
            string fileName = DateTime.Now.ToString("MM-dd-yyyy_hh-mm-ss");
            string fileExtension = ".png";

            ScreenCapture.CaptureScreenshot($"{storagePath}/{fileName}{fileExtension}");
        }

#if UNITY_EDITOR


        [ContextMenu("Open storages")]
        private void OpenStorage()
        {
            string directoryName = Application.persistentDataPath + $"/Screenshots/{folderName}";
            UnityEditor.EditorUtility.RevealInFinder(directoryName);
        }

#endif

    }
}
