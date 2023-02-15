using System.Collections.Generic;
using ATBS.Core.EventSystem;
using UnityEngine;
[CreateAssetMenu(fileName = "Dialog", menuName = "DialogSystem/Dialog")]
public class Dialog : ScriptableObject
{
    [field: SerializeField] public string LeadingQuestion { get; private set; }
    [field: SerializeField] public DialogSpeakerData Speaker { get; set; }
    [field: SerializeField, TextArea(3, 10)] public string Content { get; private set; }
    [field: SerializeField] public float DialogDelay { get; private set; }
    [field: SerializeField] public bool OnlyAction { get; private set; }
    [field: SerializeField] public GameEvent DialogAction { get; private set; }
    public bool IsDialogOption { get => isDialogOption; private set => isDialogOption = value; }
    public List<Dialog> DialogOptions { get => dialogOptions; private set => dialogOptions = value; }
    public bool HasEndConversation { get => hasEndConversation; private set => hasEndConversation = value; }
    public Dialog NextDialog { get => nextDialog; private set => nextDialog = value; }

    // used by custom editor
    [SerializeField, HideInInspector] private Dialog nextDialog;
    [SerializeField, HideInInspector] private List<Dialog> dialogOptions;
    [SerializeField, HideInInspector] private bool hasEndConversation;
    [SerializeField] private bool isDialogOption;
}