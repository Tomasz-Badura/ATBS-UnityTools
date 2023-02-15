using UnityEngine;
using TMPro;

namespace ATBS.MenuSystem
{
    [RequireComponent(typeof(TMP_InputField))]
    public class TextFieldWrapper : InputWrapper
    {
        TMP_InputField inputField;
        private void Awake()
        {
            inputField = GetComponent<TMP_InputField>();
            ValueChange();
            inputField.onValueChanged.AddListener((change) => ValueChange());
            InputData.OnInputChanged += ValueSet;
        }

        private void ValueSet()
        {
            inputField.text = InputData.CurrentInput;
        }

        private void OnDestroy() 
        {
            InputData.OnInputChanged -= ValueSet;
        }

        public override void ValueChange() 
        { 
            InputData.CurrentInput = inputField.text;
            if(ActiveValidation) InputData.Validate();
        }
    }
}