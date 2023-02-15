using System.Collections.Generic;
using System.Linq;
using ATBS.Core.SavingSystem;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace ATBS.SceneControlSystem
{
    public class SceneObject : MonoBehaviour
    {
        [field: SerializeField] public List<IResolver> Data { get; private set; }
        [field: SerializeField] public SceneObjectData DefaultData { get; private set; }
        [field: Header("- - - - - - - - - - - - - - - - -")]
        [field: SerializeField] public AssetReferenceGameObject PrefabReference { get; private set; }
        [field: SerializeField] public bool PersistTransform { get; private set; } = true;
        [field: SerializeField] public bool PersistData { get; private set; } = true;
        public string Guid { get => guid; private set => guid = value; }
        public bool moveToNextScene = false;
        [SerializeField] string guid;
        [SerializeField] SceneSetupManager sceneSetupManager;

        private void Awake()
        {
            Data = GetComponents<MonoBehaviour>().OfType<IResolver>().ToList();
        }

        public DataContainer GetData()
        {
            List<DataContainer> GetAdditionalData()
            {
                List<DataContainer> data = new();
                for (int i = 0; i < Data.Count; i++)
                    data.Add(Data[i].Save());
                return data;
            }

            return new SceneObjectData(
                GetAdditionalData(),
                Guid,
                PrefabReference.AssetGUID,
                moveToNextScene,
                gameObject.activeSelf,
                transform.position,
                transform.rotation,
                transform.localScale
            );
        }

        public void SetData(SceneObjectData data)
        {
            void SetAdditionalData(List<DataContainer> data)
            {
                for (int i = 0; i < Data.Count; i++)
                    Data[i].Load(data[i]);
            }

            Guid = data.guid;
            PrefabReference = new AssetReferenceGameObject(data.assetGuid);

            if (PersistData)
                SetAdditionalData(data.data);
            else
                SetAdditionalData(DefaultData.data);

            if (PersistTransform)
            {
                gameObject.SetActive(data.isActive);
                transform.position = data.position;
                transform.rotation = data.rotation;
                transform.localScale = data.scale;
            }
            else
            {
                gameObject.SetActive(DefaultData.isActive);
                transform.position = DefaultData.position;
                transform.rotation = DefaultData.rotation;
                transform.localScale = DefaultData.scale;
            }
        }

        private void OnEnable() => sceneSetupManager.SceneObjects.Add(this);
        private void OnDestroy() => sceneSetupManager.SceneObjects.Remove(this);
    }
}