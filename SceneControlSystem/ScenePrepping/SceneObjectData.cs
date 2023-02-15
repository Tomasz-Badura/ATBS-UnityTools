using System.Collections.Generic;
using ATBS.Core.SavingSystem;
using UnityEngine;

namespace ATBS.SceneControlSystem
{
    [System.Serializable]
    public class SceneObjectData : DataContainer
    {
        public List<DataContainer> data;
        public string guid;
        public string assetGuid;
        public bool moveToNextScene;
        public bool isActive;
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 scale;

        public SceneObjectData(List<DataContainer> data, string guid, string assetGuid, bool moveToNextScene, bool isActive, Vector3 position, Quaternion rotation, Vector3 scale)
        {
            this.data = data;
            this.guid = guid;
            this.assetGuid = assetGuid;
            this.moveToNextScene = moveToNextScene;
            this.isActive = isActive;
            this.position = position;
            this.rotation = rotation;
            this.scale = scale;
        }
    }
}