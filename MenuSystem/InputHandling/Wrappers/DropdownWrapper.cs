using UnityEngine;
using TMPro;
namespace ATBS.MenuSystem
{
    [RequireComponent(typeof(TMP_Dropdown))]
    public class DropdownWrapper : InputWrapper
    {
        public TMP_Dropdown Dropdown { get => dropdown; set => dropdown = value; }
        private TMP_Dropdown dropdown;
        private void Awake()
        {
            dropdown = GetComponent<TMP_Dropdown>();
            ValueChange();
            dropdown.onValueChanged.AddListener((_) => ValueChange());
            InputData.OnInputChanged += ValueSet;
        }

        private void ValueSet()
        {
            dropdown.value = int.Parse(InputData.CurrentInput);
        }

        private void OnDestroy()
        {
            InputData.OnInputChanged -= ValueSet;
        }

        public override void ValueChange()
        {
            InputData.CurrentInput = dropdown.value.ToString();
            if (ActiveValidation) InputData.Validate();
        }
    }
}