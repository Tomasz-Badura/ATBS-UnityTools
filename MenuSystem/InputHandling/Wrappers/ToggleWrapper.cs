using UnityEngine;
using UnityEngine.UI;

namespace ATBS.MenuSystem
{
    [RequireComponent(typeof(Toggle))]
    public class ToggleWrapper : InputWrapper
    {
        Toggle toggle;
        private void Awake()
        {
            toggle = GetComponent<Toggle>();
            ValueChange();
            toggle.onValueChanged.AddListener((change) => ValueChange());
            InputData.OnInputChanged += ValueSet;
        }

        private void ValueSet()
        {
            toggle.isOn = bool.Parse(InputData.CurrentInput);
        }

        private void OnDestroy() 
        {
            InputData.OnInputChanged -= ValueSet;
        }

        public override void ValueChange() 
        { 
            InputData.CurrentInput = toggle.isOn.ToString();
            if(ActiveValidation) InputData.Validate();
        }
    }
}