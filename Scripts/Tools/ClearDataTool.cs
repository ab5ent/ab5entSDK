using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace ab5entSDK.Tools
{
    public class ClearDataTool : MonoBehaviour
    {
        [MenuItem("Tools/Clear Data")]
        public static void ClearData()
        {
            ClearPlayerPrefs();
            ClearUserPersistentData();
        }

        private static void ClearPlayerPrefs()
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
        }

        private static void ClearUserPersistentData()
        {
            try
            {
                string rootPath = Application.persistentDataPath;

                DeleteAllFilesInDirectory(rootPath);
                DeleteAllDirectoriesInDirectory(rootPath);
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
            }
        }

        private static void DeleteAllFilesInDirectory(string directoryPath)
        {
            string[] files = Directory.GetFiles(directoryPath);

            foreach (string file in files)
            {
                File.Delete(file);
                Console.WriteLine($"File {file} deleted successfully");
            }
        }

        private static void DeleteAllDirectoriesInDirectory(string directoryPath)
        {
            string[] folders = Directory.GetDirectories(directoryPath);

            foreach (string folder in folders)
            {
                Directory.Delete(folder, true);
                Console.WriteLine($"Folder {folder} deleted successfully");
            }
        }
    }
}