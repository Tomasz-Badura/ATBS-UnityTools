using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using ATBS.Extensions;
using ATBS.Core;
namespace ATBS.MenuSystem
{
    [RequireComponent(typeof(Canvas))]
    public abstract class Menu : MonoBehaviour
    {
        #region variables
        [field: SerializeField] public string Name { get; protected set; } // name identificator for this menu
        public Menu Previous { get; set; } // menu that lead to opening this menu
        [SerializeField] protected MenuManager menuManager;
        [SerializeField] private List<Button> Buttons = new(); // list of all buttons in a menu
        [SerializeField] private int sortLayer = 0;
        private Canvas canvas;
        #endregion
        #region methods
        /// <summary>
        /// Allows for enabling and disabling of buttons
        /// </summary>
        /// <param name="state">turn on or off</param>
        /// <param name="buttonName">name of the button to turn off, leave null for all</param>
        public void SetButtonState(bool state, string buttonName = null)
        {
            if (buttonName == null)
            {
                foreach (Button button in Buttons)
                {
                    button.interactable = state;
                }
                return;
            }
            foreach (Button button in Buttons)
            {
                if (button.gameObject.name.Clean() == buttonName.Clean())
                {
                    button.interactable = state;
                    return;
                }
            }
            Debug.LogWarning("Couldn't find a button with that name");
        }

        /// <summary>
        /// Get the button with specified name
        /// </summary>
        /// <param name="buttonName"> name of the button to get </param>
        /// <returns> found button or null </returns>
        protected Button GetButton(string buttonName)
        {
            return Buttons.Find(button => button.gameObject.name.Clean() == buttonName.Clean());
        }
        #endregion
        #region virtual methods
        /// <summary>
        /// called on every opening of the menu
        /// </summary>
        public virtual void Load() { Refresh(); }

        /// <summary>
        /// called on every closing of the menu
        /// </summary>
        public virtual void Unload() { }

        /// <summary>
        /// called on every refresh of the menu
        /// </summary>
        public virtual void Refresh() { }

        protected virtual void Awake()
        {
            menuManager.Menus.Add(this);
            if (string.IsNullOrWhiteSpace(Name.Clean())) Debug.LogError("Menu name of: " + gameObject.name + ", is empty. Make sure to include only alphabet letters.");
            SetupCanvas();
        }

        /// <summary>
        /// Set's up the canvas
        /// </summary>
        private void SetupCanvas()
        {
            // Canvas
            canvas = GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.pixelPerfect = false;
            canvas.sortingOrder = sortLayer;
            // Canvas scaler
            CanvasScaler canvasScaler = GetComponent<CanvasScaler>();
            if (canvasScaler == null) canvasScaler = gameObject.AddComponent<CanvasScaler>();
            canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasScaler.referenceResolution = new Vector2(1920f, 1080f);
            // Graphic raycaster
            GraphicRaycaster graphicRaycaster = GetComponent<GraphicRaycaster>();
            if (graphicRaycaster == null)
                gameObject.AddComponent<GraphicRaycaster>();
        }
        protected virtual void OnDestroy() => menuManager.Menus.Remove(this);
        #endregion
    }
}