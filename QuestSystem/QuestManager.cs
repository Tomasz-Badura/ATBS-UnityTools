using System;
using System.Collections.Generic;
using ATBS.Extensions;
using UnityEngine;
namespace ATBS.QuestSystem
{
    [CreateAssetMenu(menuName = "QuestSystem/QuestManager")]
    public class QuestManager : ScriptableObject
    {
        #region Variables
        [field: SerializeField] public List<Quest> Quests { get; private set; }
        #endregion
        #region Events  
        public delegate void QuestEvent(Quest quest);
        public delegate void QuestsEvent(List<Quest> quests);
        public event QuestsEvent OnQuestsLoaded;
        public event QuestEvent OnQuestStarted;
        public event QuestEvent OnQuestFinished;
        #endregion
        #region Methods
        public void LoadQuests(List<Quest> quests)
        {
            if (quests == null) return;
            Quests = quests;
            OnQuestsLoaded?.Invoke(Quests);
        }

        public void StartQuest(Quest newQuest)
        {
            if (newQuest == null) return;
            newQuest.isFinished = false;
            OnQuestStarted?.Invoke(newQuest);
        }

        public void FinishQuest(Quest quest)
        {
            if (quest == null) return;
            quest.isFinished = true;
            OnQuestFinished?.Invoke(quest);
        }
        #endregion
    }
}