using UnityEngine;
using ATBS.Core;
using System.Collections.Generic;

namespace ATBS.TextSystem
{
    [CreateAssetMenu(menuName = "TextChartSystem/TextManager")]
    public class TextManager : ScriptableObject 
    {
        public Dictionary<string, string> TextSet { get; private set; }
        [SerializeField] private StringVariable currentSet;
        [SerializeField] private TextAsset csvSetsFile; 
        private CsvReader setsReader;

        public string GetText(string key)
        {
            return setsReader.FindValue(key, currentSet.Value);
        }

        public void ChangeCurrentSet(string newSet)
        {
            if(setsReader == null) setsReader = new(csvSetsFile);

            try
            {
                TextSet = setsReader.GetColumnData(newSet);
            }
            catch
            {
                Debug.LogError("Key: " + newSet + ", doesn't exist in: " + csvSetsFile.name);
                return;
            }
            currentSet.SetValue(newSet);
        }

        public string GetCurrentSet()
        {
            return currentSet.Value;
        }

    }
}