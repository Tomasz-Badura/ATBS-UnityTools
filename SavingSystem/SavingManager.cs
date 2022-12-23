using UnityEngine;
using System;
namespace ATBS.SavingSystem
{
    public class SavingManager : MonoBehaviour 
    {
        #region Variables   
        [SerializeField] private string fileExtension = "json"; // Extension of save file (without ".")
        private FileDataHandler dataHandler;
        #endregion
        #region Events
        public event EventHandler OnNewGame;
        public event EventHandler OnGameLoaded;
        public event EventHandler OnGameSaved;
        #endregion
        #region Methods
        private void Awake() 
        {
            dataHandler = new FileDataHandler(Application.persistentDataPath, fileExtension);
        }

        /// <summary>
        /// Starts a new game.
        /// </summary>
        /// <param name="gameSettings">Settings for the new game.</param>
        public void New(GameSettingsData gameSettings)
        {
            // Start a new game
            OnNewGame?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Loads game from a file.
        /// </summary>
        /// <param name="fileName">Name of the file to load from.</param>
        public void Load(string fileName)
        {
            SaveData saveData = dataHandler.Load(fileName);
            if(saveData == null)
            {
                Debug.LogError("Couldn't find save with that file name: " + fileName);
                return;
            }
            Load(saveData);
        }

        /// <summary>
        /// Load game from SaveData.
        /// </summary>
        /// <param name="saveData">SaveData to load from.</param>
        public void Load(SaveData saveData)
        {
            // Custom logic for loading all data
            OnGameLoaded?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Loads SaveData from the file with specified name and returns it.
        /// </summary>
        /// <param name="fileName">Name of the file to get SaveData from.</param>
        /// <returns>SaveData loaded from the file.</returns>
        public SaveData GetSave(string fileName)
        {
            SaveData saveData = dataHandler.Load(fileName);
            if(saveData == null)
            {
                Debug.LogError("Couldn't find save with that file name: " + fileName);
                return null;
            }
            return saveData;
        }

        /// <summary>
        /// Saves game data to a file.
        /// </summary>
        /// <param name="fileName">Name of the file to save to.</param>
        public void Save(string fileName)
        {
            dataHandler.Save(CreateSave(), fileName);
            OnGameSaved?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Creates new SaveData.
        /// </summary>
        /// <returns>Created SaveData.</returns>
        private SaveData CreateSave()
        {
            SaveData saveData = new();

            // Custom logic for saving all data

            return saveData;
        }
        #endregion
    }
}