using UnityEngine;
namespace ATBS.Core.SavingSystem
{
    [CreateAssetMenu(menuName = "SavingSystem/SavingManager")]
    public class SavingManager : ScriptableObject 
    {
        #region Variables
        [field: SerializeField] public SavingHandler SavingHandler { get; private set; }
        [field: SerializeField] public SOResolver SOResolver { get; private set; } = new();
        #endregion
        #region Methods
        /// <summary>
        /// Saves SaveData using the current SavingHandler
        /// </summary>
        /// <param name="saveData"></param>
        /// <param name="saveName"></param>
        public void Save(SaveData saveData, string saveName)
        {
            saveData.soData = SOResolver.Save();
            saveData.saveCreation = System.DateTime.Now;
            saveData.saveName = saveName;
            SavingHandler.Save(saveData, saveName);
        }

        /// <summary>
        /// Loads SaveData using the current SavingHandler
        /// </summary>
        /// <param name="saveName"></param>
        /// <returns> Loaded data or null when not found </returns>
        public SaveData GetSave(string saveName)
        {
            SaveData saveData = SavingHandler.Load(saveName);
            if(saveData == null)
            {
                Debug.LogError("Couldn't find save with that save name: " + saveName);
                return null;
            }
            SOResolver.Load(saveData.soData);
            return saveData;
        }
        #endregion
    }
}