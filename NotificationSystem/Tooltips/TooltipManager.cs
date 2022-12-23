using System.Collections;
using System.Collections.Generic;
using ATBS.Extensions;
using UnityEngine;
namespace ATBS.Notifications
{
    public class TooltipManager : MonoBehaviour
    {
        #region Variables
        public Tooltip DefaultTooltip { get; private set; }
        public Dictionary<string, Tooltip> Tooltips { get => tooltips; private set => tooltips = value; }
        private Dictionary<string, Tooltip> tooltips = new();
        [SerializeField] private GameObject defaultTooltipObject;
        [SerializeField] private TooltipPosition defaultTooltipPosition;
        #endregion
        #region Methods
        /// <summary>
        /// Get the tooltip with a name
        /// </summary>
        /// <param name="tooltipName">Name of the tooltip to find</param>
        /// <returns>The found tooltip or null</returns>
        public Tooltip GetTooltip(string tooltipName)
        {
            tooltipName = tooltipName.Clean();
            tooltips.TryGetValue(tooltipName, out Tooltip tooltip);
            return tooltip;
        }

        /// <summary>
        /// Shows a tooltip.
        /// </summary>
        /// <param name="tooltipName">Name of the tooltip to show</param>
        /// <param name="tooltipParent">Transform of the parent to position the tooltip relative to.</param>
        /// <param name="position">Position from the parent center that the tooltip should go to.</param>
        /// <param name="displayText">Optional text for the display text of the tooltip.</param>
        /// <param name="offset">Optional additional offset</param>
        public void ShowTooltip(string tooltipName, Transform parent, TooltipPosition position, string displayText = "", Vector2 offset = default(Vector2))
        {
            tooltipName = tooltipName.Clean();
            if (!tooltips.TryGetValue(tooltipName, out Tooltip tooltip)) { Debug.LogError("Tooltip with name: " + tooltipName + ", doesn't exist."); return; }
            PositionTooltip(tooltip.transform, parent, position, offset);
            tooltip.DisplayText = displayText;
            if(tooltip.NewParentCheck()) tooltip.NewParent();
            tooltip.Show();
        }

        /// <summary>
        /// Shows the default tooltip, relative to specified parent in the direction of the default tooltip position.
        /// </summary>
        /// <param name="parent">Transform of the object to position the tooltip relative to.</param>
        public void ShowDefaultTooltip(Transform parent)
        {
            PositionTooltip(DefaultTooltip.transform, parent, defaultTooltipPosition, Vector2.zero);
            if(DefaultTooltip.NewParentCheck()) DefaultTooltip.NewParent();
            DefaultTooltip.Show();
        }

        /// <summary>
        /// Changes the display text of the default tooltip.
        /// </summary>
        /// <param name="displayText"></param>
        public void ChangeDefaultTooltipText(string displayText)
        {
            DefaultTooltip.DisplayText = displayText;
            DefaultTooltip.Refresh();
        }

        /// <summary>
        /// Hides the specified tooltip.
        /// </summary>
        /// <param name="tooltipName">Name of the tooltip to hide.</param>
        public void HideTooltip(string tooltipName)
        {
            tooltipName = tooltipName.Clean();
            if (!tooltips.TryGetValue(tooltipName, out Tooltip tooltip)) { Debug.LogError("Tooltip with name: " + tooltipName + ", doesn't exist."); return; }
            tooltip.Hide();
        }

        /// <summary>
        /// Hides the default tooltip.
        /// </summary>
        public void HideDefaultTooltip()
        {
            DefaultTooltip.Hide();
        }

        /// <summary>
        /// Positions a tooltip relative to a parent.
        /// </summary>
        /// <param name="tooltip">Transform of the tooltip to position.</param>
        /// <param name="tooltipParent">Transform of the parent to position the tooltip relative to.</param>
        /// <param name="position">Position from the parent center that the tooltip should go to.</param>
        /// <param name="offset">Optional additional offset</param>
        /// <returns>True if everything went right, false if something failed</returns>
        public bool PositionTooltip(Transform tooltip, Transform tooltipParent, TooltipPosition position, Vector2 offset = default(Vector2))
        {
            if (tooltip == null || tooltipParent == null) { Debug.LogError("tooltip and tooltipParent transforms can't be null"); return false; }

            RectTransform parent = tooltipParent.GetComponent<RectTransform>();
            if (parent == null) { Debug.LogError("Tooltip parent must have RectTransform"); return false; }

            RectTransform rectTransform = tooltip.GetComponent<RectTransform>();
            if (rectTransform == null) { Debug.LogError("Tooltip must have RectTransform"); return false; }

            tooltip.transform.SetParent(tooltipParent, false);

            Vector2 size = rectTransform.sizeDelta;

            switch (position)
            {
                case TooltipPosition.TopLeft:
                {
                    tooltip.transform.localPosition = new Vector3(-parent.rect.width / 2 - size.x / 2, parent.rect.height / 2 + size.y / 2);
                    break;
                }
                case TooltipPosition.Top:
                {
                    tooltip.transform.localPosition = new Vector3(0, parent.rect.height / 2 + size.y / 2);
                    break;
                }
                case TooltipPosition.TopRight:
                {
                    tooltip.transform.localPosition = new Vector3(parent.rect.width / 2 + size.x / 2, parent.rect.height / 2 + size.y / 2);
                    break;
                }
                case TooltipPosition.MiddleLeft:
                {
                    tooltip.transform.localPosition = new Vector3(-parent.rect.width / 2 - size.x / 2, 0);
                    break;
                }
                case TooltipPosition.Middle:
                {
                    tooltip.transform.localPosition = new Vector3(0, 0);
                    break;
                }
                case TooltipPosition.MiddleRight:
                {
                    tooltip.transform.localPosition = new Vector3(parent.rect.width / 2 + size.x / 2, 0);
                    break;
                }
                case TooltipPosition.BottomLeft:
                {
                    tooltip.transform.localPosition = new Vector3(-parent.rect.width / 2 - size.x / 2, -parent.rect.height / 2 - size.y / 2);
                    break;
                }
                case TooltipPosition.Bottom:
                {
                    tooltip.transform.localPosition = new Vector3(0, -parent.rect.height / 2 - size.y / 2);
                    break;
                }
                case TooltipPosition.BottomRight:
                {
                    tooltip.transform.localPosition = new Vector3(parent.rect.width / 2 + size.x / 2, -parent.rect.height / 2 - size.y / 2);
                    break;
                }
                default:
                    Debug.LogError(position + ", this tooltip position is not defined");
                    return false;
            }
            tooltip.localPosition += new Vector3(offset.x, offset.y);
            return true;
        }

        private void Start()
        {
            DefaultTooltip = defaultTooltipObject.GetComponent<Tooltip>();
            if (DefaultTooltip == null) Debug.LogError("Default tooltip is not set");

            GetTooltips();
        }
        
        /// <summary>
        /// Fills the tooltips list.
        /// </summary>
        private void GetTooltips()
        {
            foreach (Transform child in transform)
            {
                Tooltip tooltip = child.GetComponent<Tooltip>();
                if (tooltip != null)
                    tooltips.Add(tooltip.Name, tooltip);
            }
        }
        #endregion
    }
}