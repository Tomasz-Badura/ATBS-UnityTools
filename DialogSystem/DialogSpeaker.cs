using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ATBS.DialogSystem
{
    public class DialogSpeaker : MonoBehaviour
    {
        [field: SerializeField] public Dialog CurrentDialog { get; set; }
        [field: SerializeField] public DialogSpeakerData DialogSpeakerData { get; private set; }
        [SerializeField] private DialogManager dialogManager;
        public bool isInteractable = true;
        public void Interact()
        {
            if (isInteractable)
                dialogManager.StartDialog(CurrentDialog);
        }

        private void Awake()
        {
            DialogSpeakerData.transform = transform;
        }
    }
}