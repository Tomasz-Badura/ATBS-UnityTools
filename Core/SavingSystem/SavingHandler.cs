using UnityEngine;
namespace ATBS.Core.SavingSystem
{
    public abstract class SavingHandler : ScriptableObject
    {
        public string SavePath { get { return $"{Application.persistentDataPath}{System.IO.Path.DirectorySeparatorChar}{filePath}"; } }
        [field: SerializeField] public string fileExtension { get; private set; } = "save";
        [field: SerializeField] public string filePath { get; private set; }
        
        /// <summary>
        /// Gets the SaveData
        /// </summary>
        /// <param name="saveName"></param>
        /// <returns></returns>
        public abstract SaveData Load(string saveName);

        /// <summary>
        /// Saves the SaveData
        /// </summary>
        /// <param name="saveData"></param>
        /// <param name="saveName"></param>
        public abstract void Save(SaveData saveData, string saveName);
    }
}