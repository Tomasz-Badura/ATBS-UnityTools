using System.Collections.Generic;
using UnityEngine;
namespace ATBS.Core.SavingSystem
{
    [CreateAssetMenu(fileName = "SOResolver", menuName = "SavingSystem/SOResolver")]
    public class SOResolver : ScriptableObject
    {
        public Dictionary<int, DataContainer> SavedData { get; private set; } 

        [SerializeField, Tooltip("Make sure to include ScriptableObjects with IResolver interface.")]
        List<ScriptableObject> objects;
        List<IResolver> saveableObjects;

        /// <summary>
        /// Fills dictionary of data to save
        /// </summary>
        /// <returns> dictionary of data to save </returns>
        public Dictionary<int, DataContainer> Save()
        {
            GetObjects();
            SavedData = new();
            for (int i = 0; i < saveableObjects.Count; i++)
                SavedData.Add(i, saveableObjects[i].Save());
            return SavedData;
        }

        /// <summary>
        /// Loads data into scriptable objects
        /// </summary>
        /// <param name="data"></param>
        public void Load(Dictionary<int, DataContainer> data)
        {
            SavedData = data;
            Load();
        }

        /// <summary>
        /// Loads current saveData into scriptable objects
        /// </summary>
        public void Load()
        {
            GetObjects();
            for (int i = 0; i < saveableObjects.Count; i++)
                saveableObjects[i].Load(SavedData.GetValueOrDefault(i));
        }

        /// <summary>
        /// Fills the saveableObjects list from objects list
        /// </summary>
        private void GetObjects()
        {
            if (saveableObjects != null) return;
            saveableObjects = new List<IResolver>();
            foreach (ScriptableObject so in objects)
            {
                if (so is IResolver resolver)
                    saveableObjects.Add(resolver);
                else
                    Debug.LogWarning(so + ", Does not implement IResolver. It won't be included in saving.");
            }
        }

        private void OnDisable()
        {
            saveableObjects = null;
        }
    }
}