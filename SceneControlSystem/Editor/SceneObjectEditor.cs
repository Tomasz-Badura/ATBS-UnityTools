using UnityEngine;
using UnityEditor;
using System.Linq;
using ATBS.Core.SavingSystem;
using System.Collections.Generic;

namespace ATBS.SceneControlSystem
{
    [CustomEditor(typeof(SceneObject))]
    public class SceneObjectEditor : UnityEditor.Editor 
    {
        SerializedProperty guid;
        SerializedProperty assetGuid;
        SceneObject sceneObject;
        void OnEnable()
        {
            sceneObject = (SceneObject) target;
            guid = serializedObject.FindProperty("guid");
        }
        public override void OnInspectorGUI() 
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Set default"))
            {
                // guid
                if(string.IsNullOrEmpty(guid.stringValue))
                {
                    guid.stringValue = System.Guid.NewGuid().ToString();
                    serializedObject.ApplyModifiedProperties();
                }
                // default data
                List<DataContainer> defaultData = new();
                foreach (IResolver saveable in sceneObject.gameObject.GetComponents<MonoBehaviour>().OfType<IResolver>().ToArray())
                    defaultData.Add(saveable.Save());
                sceneObject.DefaultData.data = defaultData;
                // defaults
                sceneObject.DefaultData.guid = guid.stringValue;
                sceneObject.DefaultData.assetGuid = sceneObject.PrefabReference.AssetGUID;
                sceneObject.DefaultData.moveToNextScene = sceneObject.moveToNextScene;
                sceneObject.DefaultData.isActive = sceneObject.gameObject.activeSelf;
                sceneObject.DefaultData.position = sceneObject.transform.position;
                sceneObject.DefaultData.rotation = sceneObject.transform.rotation;
                sceneObject.DefaultData.scale = sceneObject.transform.localScale;
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}