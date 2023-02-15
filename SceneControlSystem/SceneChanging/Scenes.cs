using System.Collections.Generic;
using System.IO;
using UnityEngine.SceneManagement;

namespace ATBS.SceneControlSystem
{
    public static class Scenes
    {
        public static List<string> SceneList 
        {
            get
            {
                if(sceneList != null || sceneList.Count > 0) return sceneList;
                sceneList = new();
                for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
                    sceneList.Add(Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i)));
                return sceneList;
            }
        }
        private static List<string> sceneList;

        public static string ActiveSceneName
        {
            get
            {
                return SceneManager.GetActiveScene().name;
            }
        }
    }
}