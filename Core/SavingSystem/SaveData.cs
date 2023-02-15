using System.Collections.Generic;

namespace ATBS.Core.SavingSystem
{
    /// <summary>
    /// All data that is saved
    /// </summary>
    [System.Serializable]
    public class SaveData
    {
        public string saveName;
        public System.DateTime saveCreation;
        public Dictionary<string, DataContainer> data;
        public Dictionary<int, DataContainer> soData;
        public SaveData(string saveName, System.DateTime saveCreation, Dictionary<string, DataContainer> data)
        {
            this.saveName = saveName;
            this.saveCreation = saveCreation;
            this.data = data;
        }
    }
}
