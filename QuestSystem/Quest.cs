using UnityEngine;
namespace ATBS.QuestSystem
{
    [CreateAssetMenu(menuName = "QuestSystem/Quest")]
    public class Quest : ScriptableObject
    {
        public string questTitle;
        [TextArea(4, 30)] public string questDescription;
        public QuestData questData;
        [HideInInspector] public bool isFinished, isActive;
    }
}