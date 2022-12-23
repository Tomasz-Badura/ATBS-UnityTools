using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine.UI;
using System.Collections;

namespace ATBS.Notifications
{
    public class PopupManager : MonoBehaviour
    {
        #region Variables
        [field: SerializeField]public Transform QueuePopupLocation { get; private set; } // Location for queued popups, should have a canvas.
        [field: SerializeField]public Transform SystemPopupLocation { get; private set; } // Location for system popups, should have a canvas.
        public Popup CurrentPopup { get; private set; } // Currently open popup
        public int ImportantLayer { get => importantLayer; private set => importantLayer = value; } // default layer at which Important popups should be opened.
        public int NonimportantLayer { get => nonimportantLayer; private set => nonimportantLayer = value; } // default layer at which Nonimportant popups should be opened.
        public int SystemLayer { get => systemLayer; private set => systemLayer = value; } // default layer at which System popups should be opened.
        public List<Popup> PopupQueue { get => popupQueue; private set => popupQueue = value; } // All popups that are in the queue
        
        [SerializeField]private int importantLayer = 2;
        [SerializeField]private int nonimportantLayer = 1;
        [SerializeField]private int systemLayer = 99;
        [SerializeField]private List<GameObject> PopupPrefabs = new();
        private int defaultLayerNumber = 1;
        private List<Popup> popupQueue = new();
        private Canvas QueuePopupCanvas, SystemPopupCanvas;
        #endregion
        #region Events
        public event EventHandler OnImportantOpen;
        public event EventHandler OnImportantClosed;
        #endregion
        #region Methods
        /// <summary>
        /// Opens a new popup.
        /// </summary>
        /// <param name="type">The type of the popup to be opened.</param>
        /// <param name="prefab">The prefab of the popup.</param>
        /// <param name="isStatic">Is the popup moveable, default false.</param>
        /// <param name="layer">Optional layer at which to open this popup</param>
        /// <returns>The opened popup or null if went wrong.</returns>
        public Popup OpenPopup(PopupType type, GameObject prefab, bool isStatic = false, int? layer = null)
        {
            GameObject popupObject = Instantiate(prefab);
            Popup popup = popupObject.GetComponent<Popup>();

            if (popup == null) { Debug.LogError("This GameObject is not a popup"); Destroy(popupObject); return null; }
            bool state = false;
            switch (type)
            {
                case PopupType.Important:
                {
                    popupObject.transform.SetParent(QueuePopupLocation, false);
                    if(!QueueHasImportant()) OnImportantOpen?.Invoke(this, EventArgs.Empty);
                    popupQueue.Insert(0, popup);
                    break;
                }
                case PopupType.NonImportant:
                {
                    popupObject.transform.SetParent(QueuePopupLocation, false);
                    popupQueue.Add(popup);
                    break;
                }
                case PopupType.System:
                popupObject.transform.SetParent(SystemPopupLocation, false);
                state = true;
                    break;

                case PopupType.Custom:
                popupObject.transform.SetParent(SystemPopupLocation, false);
                state = true;
                    break;

                default:
                    Debug.LogError("PopupType: " + type + ", is not defined");
                    break;
            }

            popup.Set(type, this, isStatic, GetLayer(type, layer));
            popupObject.SetActive(state);
            if(state) popup.Open();
            TryNextInQueue();
            return popup;
        }
        
        /// <summary>
        /// Opens a new custom popup.
        /// </summary>
        /// <param name="prefab">The prefab to be opened.</param>
        /// <param name="location">Location at which the popup should be created.</param>
        /// <param name="isStatic">Is the popup moveable, default false.</param>
        /// <returns>The opened popup or null if went wrong.</returns>
        public Popup OpenPopup(GameObject prefab, Transform location, bool isStatic = false)
        {
            GameObject popupObject = Instantiate(prefab, location, false);
            Popup popup = popupObject.GetComponent<Popup>();

            if (popup == null) { Debug.LogError("This GameObject is not a popup"); Destroy(popupObject); return null; }

            popup.Set(PopupType.Custom, this, isStatic);
            popupObject.SetActive(true);
            popup.Open();
            TryNextInQueue();
            return popup;
        }

        /// <summary>
        /// Opens a new popup.
        /// </summary>
        /// <param name="type">Type of the popup to open.</param>
        /// <param name="popupIndex">Index of the popup prefab in the popupPrefabs list.</param>
        /// <param name="isStatic">Is the popup moveable, default false.</param>
        /// <param name="layer">Optional layer at which to open this popup</param>
        /// <returns>The opened popup or null if went wrong.</returns>
        public Popup OpenPopup(PopupType type, int popupIndex, bool isStatic = false, int? layer = null)
        {
            if(PopupPrefabs.Count - 1 < popupIndex || popupIndex < 0) return null;
            return OpenPopup(type, PopupPrefabs[popupIndex], isStatic, layer);
        }

        /// <summary>
        /// Closes the specified popup.
        /// </summary>
        /// <param name="popupObject">Popup GameObject to close.</param>
        public void ClosePopup(GameObject popupObject)
        {
            if(popupObject == null) return;
            Popup popup = popupObject.GetComponent<Popup>();

            if (popup == null) { Debug.LogError("This GameObject is not a popup"); return; }
            if(popup.Type == PopupType.Important || popup.Type == PopupType.NonImportant)
                popupQueue.Remove(popup);
            popup.Close();
            if(!QueueHasImportant() && popup.Type == PopupType.Important) OnImportantClosed?.Invoke(this, EventArgs.Empty);
            DestroyImmediate(popupObject, false);
            TryNextInQueue();
        }

        /// <summary>
        /// Closes the specified popup without calling TryNextInQueue, used to close popups while iterating a list of popups.
        /// </summary>
        /// <param name="popupObject">Popup GameObject to close.</param>
        public void ClosePopupSafe(GameObject popupObject)
        {
            if(popupObject == null) return;
            Popup popup = popupObject.GetComponent<Popup>();

            if (popup == null) { Debug.LogError("This GameObject is not a popup"); return; }
            if(popup.Type == PopupType.Important || popup.Type == PopupType.NonImportant)
                popupQueue.Remove(popup);
            popup.Close();
            if(!QueueHasImportant() && popup.Type == PopupType.Important) OnImportantClosed?.Invoke(this, EventArgs.Empty);
            Destroy(popupObject);
        }

        /// <summary>
        /// Close the currently open popup.
        /// </summary>
        public void CloseCurrentPopup()
        {
            if(CurrentPopup == null ? false : !CurrentPopup.Equals(null))
            {
                popupQueue.Remove(CurrentPopup);
                CurrentPopup.Close();
                if(!QueueHasImportant() && CurrentPopup.Type == PopupType.Important) OnImportantClosed?.Invoke(this, EventArgs.Empty);
                DestroyImmediate(CurrentPopup.gameObject, false);
                TryNextInQueue();
            }
        }
        /// <summary>
        /// Closes all popups that are in the queue
        /// </summary>
        public void CloseAllQueuedPopups()
        {
            foreach (Popup popup in popupQueue)
            {
                ClosePopupSafe(popup.gameObject);
            }
        }

        /// <summary>
        /// Close all popups that are in the SystemPopupLocation
        /// </summary>
        public void CloseAllSystemPopups()
        {
            foreach (Transform child in SystemPopupLocation)
            {
                Popup popup = child.GetComponent<Popup>();
                if(popup == null) continue;
                ClosePopupSafe(child.gameObject);            
            }
        }
        
        /// <summary>
        /// Checks if there is currently open popup, if not opens the next popup in queue.
        /// </summary>
        public void TryNextInQueue()
        {
            if(CurrentPopup == null ? true : CurrentPopup.Equals(null))
            {
                if(popupQueue.Count == 0) return;
                QueuePopupCanvas.sortingOrder = popupQueue[0].Layer;
                popupQueue[0].gameObject.SetActive(true);
                popupQueue[0].Open();
                CurrentPopup = popupQueue[0];
            }   
        }

        /// <summary>
        /// Moves all Important popus in the queue to the top.
        /// </summary>
        public void SortQueue()
        {
            int index = 0;
            for (int i = 0; i < popupQueue.Count; i++)
            {
                Popup popup = popupQueue[i];
                if(popup.Type == PopupType.Important)
                {
                    popupQueue.RemoveAt(i);
                    popupQueue.Insert(index, popup);
                    index++;
                }
            }
        }

        /// <summary>
        /// Finds if any popup in the queue is of type Important.
        /// </summary>
        /// <returns>True if an Important popup is found in the queue, false otherwise.</returns>
        public bool QueueHasImportant()
        {
            return popupQueue.Any(x => x.Type == PopupType.Important);
        }

        /// <summary>
        /// Get's the layer number for the PopupType.
        /// </summary>
        /// <param name="type">Type to get the layer number for.</param>
        /// <param name="customLayer"></param>
        /// <returns>Layer number for that type or default layer number.</returns>
        public int GetLayer(PopupType type, int? customLayer = null)
        {
            if(customLayer != null)
                return Convert.ToInt32(customLayer);
            
            switch (type)
            {
                case PopupType.Important:
                    return ImportantLayer;
                case PopupType.NonImportant:
                    return NonimportantLayer;
                case PopupType.System:
                    return SystemLayer;
                case PopupType.Custom:
                    return SystemLayer;
                default:
                Debug.LogError("PopupType: " + type + ", is not defined");
                    return defaultLayerNumber;
            }
        }

        private void Awake()
        {
            SetupLocations();
        }

        /// <summary>
        /// Check if all popup locations are properly set.
        /// </summary>
        private void SetupLocations()
        {
            if (QueuePopupLocation == null) { Debug.LogError("Popup Location is not set"); return; }
            if (SystemPopupLocation == null) { Debug.LogError("System Popup Location is not set"); return; }
            QueuePopupCanvas = CheckCanvas(QueuePopupLocation.gameObject);
            SystemPopupCanvas = CheckCanvas(SystemPopupLocation.gameObject);
            SystemPopupCanvas.sortingOrder = SystemLayer;
        }

        /// <summary>
        /// Checks if a GameObject has a canvas and canvas related components.
        /// </summary>
        /// <param name="target">GameObject to check canvas for.</param>
        private Canvas CheckCanvas(GameObject target)
        {
            bool sendWarning = false;
            // Canvas
            Canvas canvas = target.GetComponent<Canvas>();
            if(canvas == null) 
            {   
                canvas = target.AddComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                canvas.pixelPerfect = false;
                sendWarning = true;
            }
            // Canvas scaler
            CanvasScaler canvasScaler = target.GetComponent<CanvasScaler>();
            if(canvasScaler == null)
            { 
                canvasScaler = target.AddComponent<CanvasScaler>();
                canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                canvasScaler.referenceResolution = new Vector2(1920f, 1080f);
                sendWarning = true;
            }
            // Graphic raycaster
            GraphicRaycaster graphicRaycaster = target.GetComponent<GraphicRaycaster>();
            if(graphicRaycaster == null)
            {
                target.AddComponent<GraphicRaycaster>();
                sendWarning = true;
            }

            if(sendWarning)
                Debug.LogWarning("There wasn't properly setup canvas on " + target.name);

            return canvas;
        }
        #endregion
    }
}