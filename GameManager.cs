using ATBS.Core.PauseControl;
using ATBS.Core.SavingSystem;
using UnityEngine;
public abstract class GameManager : ScriptableObject
{
    [SerializeField] protected SavingManager savingManager;

    /// <summary>
    /// Loads the game from saving manager
    /// </summary>
    /// <param name="saveName"></param>
    public void LoadGame(string saveName)
    {
        LoadGame(savingManager.GetSave(saveName));
    }

    /// <summary>
    /// Saves the game to saving manager
    /// </summary>
    /// <param name="saveName"></param>
    public void SaveGame(string saveName)
    {
        savingManager.Save(GetSaveData(saveName), saveName);
    }

    /// <summary>
    /// Loads the default save
    /// </summary>
    public void StartNewGame()
    {
        LoadGame(GetDefaultSaveData());
        OnNewGame();
    }

    /// <summary>
    /// Pauses the game
    /// </summary>
    public void Pause()
    {
        if(PauseAgent.IsPaused) return;
        OnPause();
        PauseAgent.Pause(); 
    }

    /// <summary>
    /// UnPauses the game
    /// </summary>
    public void UnPause()
    {
        if(!PauseAgent.IsPaused) return;
        OnUnPause();
        PauseAgent.UnPause();
    }

    /// <summary>
    /// Loads the game from save data
    /// </summary>
    /// <param name="save"></param>
    public abstract void LoadGame(SaveData save);

    /// <summary>
    /// Gets the default save data
    /// </summary>
    public abstract SaveData GetDefaultSaveData();

    /// <summary>
    /// Gets the save data from current game state
    /// </summary>
    /// <param name="saveName"></param>
    /// <returns> Current save data </returns>
    public abstract SaveData GetSaveData(string saveName);

    /// <summary>
    /// Called before pausing
    /// </summary>
    protected virtual void OnPause() { }

    /// <summary>
    /// Called before unpausing
    /// </summary>
    protected virtual void OnUnPause() { }
    
    /// <summary>
    /// Called after loading new game
    /// </summary>
    protected virtual void OnNewGame() { }
}