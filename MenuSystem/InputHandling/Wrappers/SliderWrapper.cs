using UnityEngine;
using UnityEngine.UI;

namespace ATBS.MenuSystem
{
    [RequireComponent(typeof(Slider))]
    public class SliderWrapper : InputWrapper
    {
        Slider slider;
        private void Awake() 
        {
            slider = GetComponent<Slider>();
            ValueChange();
            slider.onValueChanged.AddListener((_) => ValueChange());
            InputData.OnInputChanged += ValueSet;
        }

        private void ValueSet()
        {
            slider.value = float.Parse(InputData.CurrentInput);
        }

        private void OnDestroy() 
        {
            InputData.OnInputChanged -= ValueSet;
        }

        public override void ValueChange() 
        { 
            InputData.CurrentInput = slider.value.ToString();
            if(ActiveValidation) InputData.Validate();
        }
    }
}