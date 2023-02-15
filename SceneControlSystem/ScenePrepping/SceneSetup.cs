using System.Linq;
using ATBS.Core.SavingSystem;
using UnityEngine;
namespace ATBS.SceneControlSystem
{
    /// <summary>
    /// Holds all data about scene objects
    /// </summary>
    [CreateAssetMenu(fileName = "Scene setup", menuName = "SceneManagement/SceneSetup")]
    public class SceneSetup : ScriptableContainer<SceneSetupData>
    {
        /// <summary>
        /// current = current scene
        /// </summary>
        [ContextMenu("Set current = current scene")]
        public void SetSceneToCurrent()
        {   
            Data.sceneName = Scenes.ActiveSceneName;
            Data.sceneObjects.Clear();
            foreach (SceneObject sceneObject in GameObject.FindObjectsOfType<SceneObject>(true))
                Data.sceneObjects.Add(sceneObject.GetData() as SceneObjectData);
        }
        
        /// <summary>
        /// default = current scene
        /// </summary>
        [ContextMenu("Set default = current scene")]
        public void SetSceneToDefault()
        {
            Data.sceneName = Scenes.ActiveSceneName;
            Data.defaultSceneObjects.Clear();
            foreach (SceneObject sceneObject in GameObject.FindObjectsOfType<SceneObject>(true))
                Data.defaultSceneObjects.Add(sceneObject.GetData() as SceneObjectData);
        }

        /// <summary>
        /// current = default
        /// </summary>
        [ContextMenu("Set current = default")]
        public void SetCurrentToDefault()
        {
            Data.sceneObjects = Data.defaultSceneObjects.ToList();
        }

        /// <summary>
        /// default = curent
        /// </summary>
        [ContextMenu("Set default = current")]
        public void SetDefaultToCurrent()
        {
            Data.defaultSceneObjects = Data.sceneObjects.ToList();
        }
    }
}