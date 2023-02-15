using ATBS.Extensions;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ATBS.Notifications
{
    public abstract class Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        #region Variables
        [field: SerializeField] public string Name { get; private set; }
        public string DisplayText 
        { 
            get => displayText; 
            set
            {
                displayText = value; 
                Refresh(); 
            }
        }
        
        public bool IsShowed { get; protected set; }
        public bool Interactable { get; protected set; }
        [SerializeField] private TooltipManager tooltipManager;
        private Transform LastParent;
        private string displayText;
        #endregion
        #region Methods
        protected virtual void Awake()
        {
            if (transform.parent == null) { Debug.LogError("Tooltip must be a child of TooltipManager"); return; };
            if (transform.parent.GetComponent<TooltipManager>() == null) Debug.LogError("Tooltip must be a child of TooltipManager");

            IsShowed = false;
            Interactable = false;
            
            Name = Name.Clean();
            if (string.IsNullOrWhiteSpace(Name)) Debug.LogError("Tooltip name of: " + gameObject.name + ", is empty. Make sure to include only alphabet letters.");
        }

        public bool NewParentCheck()
        {
            if (LastParent == transform.parent) return false;
            LastParent = transform.parent;
            return true;
        }
        #endregion
        #region Virtual methods

        /// <summary>
        /// Called when showing a tooltip.
        /// </summary>
        public virtual void Show() { IsShowed = true; }

        /// <summary>
        /// Called when hiding a tooltip.
        /// </summary>
        public virtual void Hide() { IsShowed = false; }

        /// <summary>
        /// called on every change of displayText of the tooltip.
        /// </summary>
        public virtual void Refresh() { }

        /// <summary>
        /// called when the parent changes.
        /// </summary>
        public virtual void NewParent() { }

        public virtual void OnPointerEnter(PointerEventData eventData) { if (Interactable) Show(); }

        public virtual void OnPointerExit(PointerEventData eventData) { if (Interactable) Hide(); }

        public virtual void OnPointerClick(PointerEventData eventData) { }

        private void OnEnable() => tooltipManager.Tooltips.Add(this);
        private void OnDisable() => tooltipManager.Tooltips.Remove(this);
        #endregion
    }
}