using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace ATBS.MenuSystem
{
    /// <summary>
    /// A class that extends the functionality of a Unity Slider.
    /// </summary>
    [RequireComponent(typeof(Slider))]
    public class SliderExtension : MonoBehaviour
    {
        [Header("Text to update with the slider")]
        [SerializeField] private TextMeshProUGUI text;
        private Slider thisSlider;

        private void Awake()
        {
            thisSlider = GetComponent<Slider>();
            thisSlider.onValueChanged.AddListener(UpdateText);
        }

        /// <summary>
        /// A method that updates the text to display the current value of the slider.
        /// </summary>
        /// <param name="value">The current value of the slider.</param>
        private void UpdateText(float value)
        {
            if (text == null) return;
            text.text = value.ToString();
        }
    }
}