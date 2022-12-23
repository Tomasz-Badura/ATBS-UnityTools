using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ATBS.SavingSystem
{
    /// <summary>
    /// All data that is saved
    /// </summary>
    [Serializable] 
    public class SaveData
    {
        public GameSettingsData gameSettings;
        public GameStateData gameState;
        public PlayerData player;
    }

    /// <summary>
    /// Game settings related data
    /// </summary>
    [Serializable]
    public class GameSettingsData
    {

    }

    /// <summary>
    /// Game state related data
    /// </summary>
    [Serializable]
    public class GameStateData
    {
        
    }

    /// <summary>
    /// Player related data
    /// </summary>
    [Serializable]
    public class PlayerData
    {

    }
}
