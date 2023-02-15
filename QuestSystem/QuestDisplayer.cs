using System.Collections.Generic;
using ATBS.Core.UIUtility;
using UnityEngine;
namespace ATBS.QuestSystem
{
    public abstract class QuestDisplayer : Displayer
    {
        [SerializeField] protected QuestManager questManager;
        protected List<Quest> displayedQuests = new();
        private void Start()
        {
            displayedQuests = questManager.Quests;
        }
        protected override void OnEnable()
        {
            questManager.OnQuestStarted += AddQuest;
            questManager.OnQuestFinished += RemoveQuest;
            questManager.OnQuestsLoaded += NewQuests;
        }
        protected override void OnDisable()
        {
            questManager.OnQuestStarted -= AddQuest;
            questManager.OnQuestFinished -= RemoveQuest;
            questManager.OnQuestsLoaded -= NewQuests;
        }
        
        protected virtual void QuestFinished() { }
        protected virtual void QuestStarted() { }

        private void AddQuest(Quest quest)
        {
            displayedQuests.Add(quest);
            UpdateVisuals();
            QuestStarted();
        }
        private void RemoveQuest(Quest quest)
        {
            displayedQuests.Remove(quest);
            UpdateVisuals();
            QuestFinished();
        }
        private void NewQuests(List<Quest> quests)
        {
            displayedQuests = quests;
            UpdateVisuals();
        }
    }
}
