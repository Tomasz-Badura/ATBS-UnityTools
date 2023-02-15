using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ATBS.Notifications
{
    [RequireComponent(typeof(RectTransform))]
    public abstract class Popup : MonoBehaviour, IDragHandler, IPointerClickHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IEndDragHandler
    {
        #region Variables
        public Dictionary<string, Action> Actions { get => actions; private set => actions = value; } // Actions stored by this popup
        public List<string> Data {get => data; private set => data = value;} // Data stored by this popup
        public PopupType Type { get; private set; } // this popups type
        public int Layer { get; private set; } // layer at which it should be displayed
        public PopupManager PopupManager { get; private set; } // reference to PopupManager

        private bool isStatic; // does the popup move
        private RectTransform rectTransform, parentRecttransform; 
        private Canvas parentCanvas;
        private float minX, maxX, minY, maxY;
        private Dictionary<string, Action> actions = new();
        public List<string> data = new();
        #endregion
        #region Methods
        /// <summary>
        /// Set popup details
        /// </summary>
        /// <param name="type">Popup type</param>
        /// <param name="popupManager">reference to PopupManager</param>
        /// <param name="layer">Optional Canvas.sortingOrder that this popup uses</param>
        public void Set(PopupType type, PopupManager popupManager, bool IsStatic, int layer = 0)
        {
            isStatic = IsStatic;
            Type = type;
            Layer = layer;
            rectTransform = GetComponent<RectTransform>();
            PopupManager = popupManager;

            try
            {
                parentRecttransform = rectTransform.parent.GetComponent<RectTransform>();
                parentCanvas = parentRecttransform.GetComponent<Canvas>();
                if (parentCanvas == null || parentRecttransform == null)
                    Debug.LogError("Popup must be a child of an another game object with a Canvas");
            }
            catch (SystemException)
            {
                Debug.LogError("Popup must be a child of an another game object with a Canvas");
            }
        }

        // pointer handling 

        public void OnDrag(PointerEventData data)
        {
            if(isStatic) return;
            rectTransform.localPosition += new Vector3(data.delta.x / parentCanvas.scaleFactor, data.delta.y / parentCanvas.scaleFactor);
            ClampToWindow();
            Dragging();
        }

        public void OnPointerDown(PointerEventData data)
        {
            if(isStatic) return;
            rectTransform.SetAsLastSibling(); // Move to the top
        }

        /// <summary>
        /// Gets or sets the value in the data dictionary with the specified key.
        /// </summary>
        /// <param name="key">The key of the value to get or set.</param>
        /// <returns>The value associated with the specified key.</returns>
        public string this[int key]
        {
            get
            {
                return Data[key];
            }
            set
            {
                Data[key] = value;
            }
        }

        /// <summary>
        /// Clamps the popup to the screen size.
        /// </summary>
        private void ClampToWindow()
        {
            Vector3 pos = rectTransform.localPosition;
            Vector3 minPosition = parentRecttransform.rect.min;
            Vector3 maxPosition = parentRecttransform.rect.max;
            pos.x = Mathf.Clamp(pos.x, minPosition.x, maxPosition.x);
            pos.y = Mathf.Clamp(pos.y, minPosition.y, maxPosition.y);
            rectTransform.localPosition = pos;
        }

        #endregion
        #region Virtual methods
        /// <summary>
        /// Called when the popup is showed
        /// </summary>
        public virtual void Open() { Refresh(); }

        /// <summary>
        /// Called before the popup is destroyed
        /// </summary>
        public virtual void Close() { }

        /// <summary>
        /// Additional method for reshreshing a popup
        /// </summary>
        public virtual void Refresh() { }

        /// <summary>
        /// Called inside OnDrag
        /// </summary>
        public virtual void Dragging() { }

        public virtual void OnPointerEnter(PointerEventData eventData) { }
        public virtual void OnPointerExit(PointerEventData eventData) { }

        public virtual void OnBeginDrag(PointerEventData eventData) { }
        public virtual void OnEndDrag(PointerEventData eventData) { }

        public virtual void OnPointerClick(PointerEventData eventData) { }

        #endregion
    }
}