using ATBS.Core.UIUtility;
using UnityEngine;
namespace ATBS.DialogSystem
{
    public abstract class DialogDisplayer : Displayer
    {
        #region Variables
        [SerializeField] protected DialogManager dialogManager;
        public Dialog DisplayedDialog { get; protected set; }
        #endregion
        #region Methods
        protected override void OnEnable()
        {
            dialogManager.OnDialogChanged += DialogChanged;
            dialogManager.OnDialogTextSkipped += TextSkip;
        }
        
        protected override void OnDisable()
        {
            dialogManager.OnDialogChanged -= DialogChanged;
            dialogManager.OnDialogTextSkipped -= TextSkip;
        }

        protected virtual void TextSkip() { }
        private void DialogChanged()
        {
            DisplayedDialog = dialogManager.CurrentDialog;
            UpdateVisuals();
        }
        #endregion
    }
}