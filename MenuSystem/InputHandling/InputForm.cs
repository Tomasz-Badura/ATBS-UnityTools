using UnityEngine;
using System.Collections.Generic;
namespace ATBS.MenuSystem
{
    [DisallowMultipleComponent]
    public class InputForm : MonoBehaviour
    {
        #region variables
        [Tooltip("Inputs that this form consists of, leave empty to fill with inputs from InputsLocation")]
        [SerializeField] private List<InputWrapper> Inputs = new();

        [Tooltip("Location to search for inputs if the Inputs list is empty")]
        [SerializeField] private Transform InputsLocation;
        public Dictionary<string, string> InputValues { get; private set; }
        #endregion
        #region methods
        private void Awake()
        {
            InputValues = new();
            if (InputsLocation == null) InputsLocation = transform;
            if (Inputs.Count == 0) GetInputs();
        }
        
        /// <summary>
        /// fills the Inputs list
        /// </summary>
        private void GetInputs()
        {
            Inputs.Clear();
            foreach (Transform child in InputsLocation)
            {
                InputWrapper input = child.GetComponent<InputWrapper>();
                if (input == null)
                    continue;
                Inputs.Add(input);
            }
        }

        /// <summary>
        /// fills the dictionary with the form inputs
        /// </summary>
        public void UpdateDictionary()
        {
            InputValues.Clear();
            foreach (InputWrapper input in Inputs)
            {
                InputValues.Add(input.Id, input.GetValue());
            }
        }

        /// <summary>
        /// Validates all the inputs in the form.
        /// </summary>
        /// <param name="validationResult">The validation result. If the form is valid, this will be set to "True". If the form is invalid, this will contain a tag describing the error.</param>
        /// <returns>True if the form is valid, false if the form is invalid.</returns>
        public bool ValidateForm(out string validationResult)
        {
            foreach (InputWrapper input in Inputs)
            {
                if(!input.ValidateInput(out validationResult))
                {
                    return false;
                }
            }
            validationResult = "True";
            return true;
        }

        /// <summary>
        /// Validates all the inputs in the form.
        /// </summary>
        /// <returns>True if the form is valid, false if the form is invalid.</returns>
        public bool ValidateForm()
        {
            return ValidateForm(out string validationResult);
        }

        /// <summary>
        /// Gets or sets the value of the input with the specified key in the `InputValues` dictionary.
        /// </summary>
        /// <param name="key">The key of the input to get or set.</param>
        /// <returns>The value of the input with the specified key, or an empty string if the key does not exist in the dictionary.</returns>
        public string this[string key]
        {
            get => InputValues.ContainsKey(key) ? InputValues[key] : string.Empty;
            set => InputValues[key] = value;
        }
        #endregion
    }
}