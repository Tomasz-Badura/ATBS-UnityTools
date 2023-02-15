using System;
using UnityEngine;

[Serializable]public struct QuestData
{
    // Put here your additional data for a quest
    [field: SerializeField] public int Exp { get; set; }
    [field: SerializeField] public string Difficulty { get; set; }
}