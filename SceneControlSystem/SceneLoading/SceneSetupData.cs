using System.Collections.Generic;
using ATBS.Core.SavingSystem;
using UnityEngine;

namespace ATBS.SceneControlSystem
{
    [System.Serializable]
    public class SceneSetupData : DataContainer
    {
        [Tooltip("Can it be overriden by SetupManager")]
        [field: SerializeField] public bool IsStatic { get; private set; }
        public string sceneName;
        public List<SceneObjectData> sceneObjects = new();
        public List<SceneObjectData> defaultSceneObjects = new();
    }
}