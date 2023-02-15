using UnityEngine;
using ATBS.DialogSystem;
using TMPro;
using ATBS.Core.TimerUtility;
using UnityEngine.UI;
using System.Collections;
using System;
public class DialogUI : DialogDisplayer
{
    #region Variables
    [SerializeField] private Transform optionsList;
    [SerializeField] private GameObject dialogPanel, optionsPanel, optionButtonPrefab;
    [SerializeField] private TextMeshProUGUI dialogText, dialogSpeakerText, optionsText, optionsSpeaker;
    [SerializeField] private string endOfConversationText = "End conversation";
    [SerializeField] private string optionButtonPrefix = "* ";
    [SerializeField] private string optionButtonPostfix = ".";
    #endregion
    #region Methods
    private void Awake() 
    {
        ClearVisuals();
    }
    
    public override void UpdateVisuals()
    {
        ClearVisuals();
        if(DisplayedDialog == null) return;
        // check if only action
        if (DisplayedDialog.OnlyAction)
        {
            dialogManager.ProgressDialog();
            return;
        }

        // dialog window type
        if (DisplayedDialog.IsDialogOption)
        {
            optionsPanel.SetActive(true);
            optionsSpeaker.text = DisplayedDialog.Speaker.Name;
            StartCoroutine(DialogTextShow(optionsText, ShowOptions));
        }
        else
        {
            dialogPanel.SetActive(true);
            dialogSpeakerText.text = DisplayedDialog.Speaker.Name;
            StartCoroutine(DialogTextShow(dialogText, null));
        }
    }

    private void ShowDialogWindow()
    {
        ClearVisuals();
        if (DisplayedDialog.IsDialogOption)
        {
            optionsPanel.SetActive(true);
            optionsSpeaker.text = DisplayedDialog.Speaker.Name;
            optionsText.text = DisplayedDialog.Content;
            ShowOptions();
        }
        else
        {
            dialogPanel.SetActive(true);
            dialogSpeakerText.text = DisplayedDialog.Speaker.Name;
            dialogText.text = DisplayedDialog.Content;
        }
    }
    
    protected async override void TextSkip()
    {
        StopAllCoroutines();
        ShowDialogWindow();
        await AsyncTimer.DelayDelta(0.01f); // avoids multiple input
        dialogManager.dialogDelay = false;
        if(DisplayedDialog.IsDialogOption)
            optionsList.GetChild(0).GetComponent<Button>().Select();
    }

    private void ClearVisuals()
    {
        dialogPanel.SetActive(false);
        optionsPanel.SetActive(false);
        foreach (Transform child in optionsList.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private IEnumerator DialogTextShow(TextMeshProUGUI text, Action callback)
    {
        dialogManager.dialogDelay = true;
        text.text = "";
        var contentArray = DisplayedDialog.Content.ToCharArray();
        float letterDelay = DisplayedDialog.DialogDelay / (float) contentArray.Length;
        int y = 0;
        for (float i = 0; i < DisplayedDialog.DialogDelay; i += letterDelay)
        {
            text.text += contentArray[y];
            y++;
            if(y == contentArray.Length) break;
            yield return new WaitForSecondsRealtime(letterDelay);
        }
        if(callback != null) callback();
        dialogManager.dialogDelay = false;
    }

    private void ShowOptions()
    {
        foreach (Dialog option in DisplayedDialog.DialogOptions)
        {
            Transform optionButton = Instantiate(optionButtonPrefab, optionsList, false).transform;
            optionButton.GetChild(0).GetComponent<TextMeshProUGUI>().text = optionButtonPrefix + option.LeadingQuestion + optionButtonPostfix;
            optionButton.GetComponent<Button>().onClick.AddListener(() => dialogManager.ChangeDialog(option));
        }
        if(DisplayedDialog.HasEndConversation || DisplayedDialog.DialogOptions.Count == 0)
        {
            Transform optionButton = Instantiate(optionButtonPrefab, optionsList, false).transform;
            optionButton.GetChild(0).GetComponent<TextMeshProUGUI>().text = optionButtonPrefix + endOfConversationText + optionButtonPostfix;
            optionButton.GetComponent<Button>().onClick.AddListener(() => dialogManager.ChangeDialog(null));
        }
        optionsList.GetChild(0).GetComponent<Button>().Select();
    }
    #endregion
}

