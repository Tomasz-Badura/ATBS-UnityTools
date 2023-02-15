using ATBS.Core.TimerUtility;
using ATBS.InputSystem;
using UnityEngine;
namespace ATBS.DialogSystem
{
    [CreateAssetMenu(menuName = "DialogSystem/DialogManager")]
    public class DialogManager : ScriptableObject 
    {
        public Dialog CurrentDialog { get; private set; }
        public bool dialogDelay { get; set; }
        [SerializeField] private InputRequestAction progressDialogInput;
        [SerializeField] private float betweenDialogDelay;
        public delegate void DialogEvent();
        public event DialogEvent OnDialogStarted;
        public event DialogEvent OnDialogChanged;
        public event DialogEvent OnDialogEnded;
        public event DialogEvent OnDialogTextSkipped;

        public void StartDialog(Dialog dialog)
        {
            if(dialogDelay) return;
            if(CurrentDialog != null) return;
            ChangeDialog(dialog);
            OnDialogStarted?.Invoke();
        }

        public void ProgressDialog()
        { 
            if(DialogDelay()) return;
            if(CurrentDialog == null || CurrentDialog.IsDialogOption == true) return;
            ChangeDialog(CurrentDialog.NextDialog);
        }

        public void ChangeDialog(Dialog dialog)
        {
            if(DialogDelay()) return;
            if(dialog != null) 
            {
                CurrentDialog = dialog;
                dialog.DialogAction?.Raise();
                if(dialog.OnlyAction)
                {
                    ProgressDialog();
                    return;
                }
            }
            else
            {
                CurrentDialog = dialog;
                OnDialogEnded?.Invoke();
            }
            OnDialogChanged?.Invoke();
        }

        public void Reset()
        {
            CurrentDialog = null;
            dialogDelay = false;
        }

        private void OnEnable() 
        {
            OnDialogEnded += BetweenDialogDelay;
            if(progressDialogInput == null) return;
            progressDialogInput.Performed += ProgressDialog;    
        }
        private void OnDisable() 
        {
            OnDialogEnded -= BetweenDialogDelay;
            Reset();
            if(progressDialogInput == null) return;
            progressDialogInput.Performed -= ProgressDialog;
        }

        private async void BetweenDialogDelay()
        {
            dialogDelay = true;
            await AsyncTimer.DelayDelta(betweenDialogDelay);
            dialogDelay = false;
        }

        private bool DialogDelay()
        {
            if(dialogDelay) 
            {
                OnDialogTextSkipped?.Invoke();
                return true;
            }
            return false;
        }
    }
}