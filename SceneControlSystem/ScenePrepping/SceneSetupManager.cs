// DEPENDENCIES:
// Addressables package
// tested on: Version 1.19.19 - March 04, 2022
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ATBS.Core;
using ATBS.Core.EventSystem;
using ATBS.Extensions;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace ATBS.SceneControlSystem
{
    public class SceneSetupManager : ScriptableObject
    {
        public RuntimeSet<SceneObject> SceneObjects { get; private set; }
        public SceneSetup CurrentSceneSetup { get; private set; }
        [Tooltip("All scene setups")]
        [SerializeField] List<SceneSetup> setups;
        [SerializeField] EventWrapper onSceneChangeStart;
        [SerializeField] EventWrapper onSceneChangeFinish;
        [SerializeField] EventWrapper onLoadingFinished;
        [SerializeField] EventWrapper onLoadingStarted;
        [SerializeField] EventWrapper onSetupSaved;

        System.EventHandler<object> start;
        System.EventHandler<object> finish;

        public async Task LoadSetup(SceneSetup setup)
        {
            onLoadingStarted?.Invoke(this, setup);
            if (setup == null)
            {
                onLoadingFinished?.Invoke(this, setup);
                return;
            }

            if (setup.Data.sceneName != Scenes.ActiveSceneName)
                this.LogWarning($"Loaded {setup.Data.sceneName} scene data into {Scenes.ActiveSceneName} scene.");

            if(setup.Data.IsStatic)
                setup.SetCurrentToDefault();

            // move sceneObjects
            foreach (SceneObjectData sceneObject in CurrentSceneSetup.Data.sceneObjects)
            {
                if(sceneObject.moveToNextScene)
                {
                    CurrentSceneSetup.Data.sceneObjects.Remove(sceneObject);
                    setup.Data.sceneObjects.Add(sceneObject);
                }
            }
            CurrentSceneSetup = setup;

            // Change the existing scene objects
            List<int> indexes = Enumerable.Range(0, setup.Data.sceneObjects.Count - 1).ToList();
            foreach (SceneObject so in SceneObjects.Items)
            {
                int i = setup.Data.sceneObjects.FindIndex((setup) => setup.guid == so.Guid);
                if (i >= 0)
                {
                    // set data
                    so.SetData(setup.Data.sceneObjects[i]);
                    indexes.Remove(i);
                }
                else
                {
                    // destroy if not found
                    Destroy(so.gameObject);
                }
            }

            // Create the new scene objects
            foreach (int y in indexes)
            {
                await CreateScenePrefab(setup.Data.sceneObjects[y]);
            }
            onLoadingFinished?.Invoke(this, setup);
        }

        public async Task CreateScenePrefab(SceneObjectData sceneObjectData)
        {
            AsyncOperationHandle<GameObject> asyncOperationHandle = new AssetReferenceGameObject(sceneObjectData.assetGuid).InstantiateAsync();
            await asyncOperationHandle.Task;

            if (asyncOperationHandle.Status != AsyncOperationStatus.Succeeded)
                this.LogError("Loading of an object failed");

            SceneObject sceneObject = asyncOperationHandle.Result.GetComponent<SceneObject>();
            if (sceneObject == null)
            {
                this.LogError("Couldn't find SceneObject in the prefab root");
                return;
            }
            sceneObject.SetData(sceneObjectData);
        }

        public void SetAllToDefault()
        {
            foreach (SceneSetup setup in setups)
                setup.SetCurrentToDefault();
        }

        public void OverrideSceneSetup(SceneSetup setup)
        {
            if(setup.Data.IsStatic) return;
            setup.SetSceneToCurrent();
            onSetupSaved?.Invoke(this, setup);
        }

        public void OverrideSceneSetup()
        {
            var setup = GetSceneSetup();
            if(setup == null)
            {
                this.LogWarning($"Scene: {Scenes.ActiveSceneName} does not have a scene setup");
                return;
            }
            OverrideSceneSetup(setup);
        }

        public async void LoadSetup(string sceneName) => await LoadSetup(GetSceneSetup(sceneName));
        public async void LoadSetup() => await LoadSetup(GetSceneSetup());
        public SceneSetup GetSceneSetup() => GetSceneSetup(Scenes.ActiveSceneName);
        public SceneSetup GetSceneSetup(string sceneName) => setups.Find((setup) => setup.Data.sceneName == sceneName);

        private void OnEnable()
        {
            start = (_, _) => OverrideSceneSetup();
            finish = (_, _) => LoadSetup();

            if(onSceneChangeStart != null)
                onSceneChangeStart.eventHandler += start;
            if(onSceneChangeFinish != null)
                onSceneChangeFinish.eventHandler += start;
        }

        private void OnDisable()
        {
            if(onSceneChangeStart != null)
                onSceneChangeStart.eventHandler -= start;
            if(onSceneChangeFinish != null)
                onSceneChangeFinish.eventHandler -= start;
        }
    }
}