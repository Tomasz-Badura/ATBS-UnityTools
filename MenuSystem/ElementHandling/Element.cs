using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ATBS.MenuSystem
{
    [DisallowMultipleComponent]
    public abstract class Element : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        #region variables
        public Dictionary<string, string> Data { get; private set; } = new(); // data of this element
        public ElementType Type { get; private set; } // type of element
        public bool IsSelectable { get; protected set; }// can the element be selected
        private IElementHandler reference;// reference to the element handler
        #endregion
        #region virtual methods
        /// <summary>
        /// Set default data for the element
        /// </summary>
        public virtual void SetDefaultData() { Data.Clear(); }

        /// <summary>
        /// Update the visuals of the element
        /// </summary>
        public virtual void UpdateVisuals() { } // Don't use element indexer to assign values in here, instead use Data[key]

        /// <summary>
        /// custom logic for selecting an element
        /// </summary>
        public virtual void OnSelectElement() { }

        /// <summary>
        /// custom logic for deselecting an element
        /// </summary>
        public virtual void OnDeselectElement() { }

        /// <summary>
        /// custom logic for right clicking an element
        /// </summary>
        public virtual void OnRightClick() { }

        /// <summary>
        /// custom logic for left clicking an element
        /// </summary>
        public virtual void OnLeftClick() { }

        /// <summary>
        /// custom logic for pointer entering an element
        /// </summary>
        public virtual void OnPointerEnter(PointerEventData eventData) { }

        /// <summary>
        /// custom logic for pointer exiting an element
        /// </summary>
        public virtual void OnPointerExit(PointerEventData eventData) { }
        #endregion
        #region methods
        /// <summary>
        /// Set element details
        /// </summary>
        public void Set(ElementType type, IElementHandler reference)
        {
            this.Type = type;
            this.reference = reference;
            SetDefaultData();
            UpdateVisuals();
            OnDeselectElement();
        }

        /// <summary>
        /// Gets or sets the value in the data dictionary with the specified key.
        /// </summary>
        /// <param name="key">The key of the value to get or set.</param>
        /// <returns>The value associated with the specified key.</returns>
        public string this[string key]
        {
            get
            {
                return Data[key];
            }
            set // Use Element.Data[key] instead of the indexer if you care about not calling UpdateVisuals() unnecessary amount of times
            {
                Data[key] = value;
                UpdateVisuals();
            }
        }

        // pointer handling

        public void OnPointerClick(PointerEventData eventData)
        {
            if (reference == null)
            {
                Debug.LogWarning("When instantiating new elements call the Set method");
                return;
            }

            if (eventData.button == PointerEventData.InputButton.Left)
            {
                OnLeftClick();
                if (IsSelectable)
                {
                    EventSystem.current.SetSelectedGameObject(this.gameObject);
                }
            }

            if (eventData.button == PointerEventData.InputButton.Right)
            {
                OnRightClick();
            }
        }

        public void OnSelect(BaseEventData eventData)
        {
            if (reference.IgnoreDeselect)
                return;
            OnSelectElement();
            reference.SelectedElement = this;
        }

        public void OnDeselect(BaseEventData eventData)
        {
            if (reference.IgnoreDeselect)
                return;
            OnDeselectElement();
        }
        #endregion
    }
}