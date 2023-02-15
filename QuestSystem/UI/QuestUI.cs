using ATBS.Extensions;
using ATBS.InputSystem;
using ATBS.MenuSystem;
using ATBS.QuestSystem;
using ATBS.Core.TimerUtility;
using UnityEngine;

public class QuestUI : QuestDisplayer, IElementHandler
{
    public Element SelectedElement { get; set; }
    public bool IgnoreDeselect { get; set; }
    [SerializeField] private InputRequestAction showInput;
    [SerializeField] private Transform questList;
    [SerializeField] private GameObject questElement, questFinished, questStarted;
    [SerializeField] private MenuManager menuManager;

    private void AddQuestToQuestLog(Quest quest)
    {
        Element element = menuManager.CreateElement(questElement, questList, ElementType.Placeholder01, this);

        element.Data["description".Clean()] = quest.questDescription;
        element.Data["title".Clean()] = quest.questTitle;
        element.Data["exp".Clean()] = quest.questData.Exp.ToString();
        element.Data["difficulty".Clean()] = quest.questData.Difficulty.ToString();
        element.UpdateVisuals();
    }

    public override void UpdateVisuals()
    {
        foreach (Transform child in questList)
        {
            Destroy(child.gameObject);
        }

        foreach (Quest quest in displayedQuests)
        {
            AddQuestToQuestLog(quest);
        }
    }

    public override void Show()
    {
        questList.gameObject.SetActive(true);
    }
    public override void Hide()
    {
        questList.gameObject.SetActive(false);
    }

    protected override void OnEnable() 
    {
        base.OnEnable();
        Hide();
        showInput.Performed += Show;
        showInput.Canceled += Hide;
    }

    protected override void OnDisable() 
    {
        base.OnDisable();
        showInput.Performed -= Show;
        showInput.Canceled -= Hide;
    }

    protected async override void QuestFinished()
    {
        questFinished.SetActive(true);
        await AsyncTimer.DelayDelta(2f);
        questFinished.SetActive(false);
    }

    protected async override void QuestStarted()
    {
        questStarted.SetActive(true);
        await AsyncTimer.DelayDelta(2f);
        questStarted.SetActive(false);
    }
}
