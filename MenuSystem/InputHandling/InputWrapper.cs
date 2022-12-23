using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using ATBS.Extensions;

namespace ATBS.MenuSystem
{
    [DisallowMultipleComponent]
    public class InputWrapper : MonoBehaviour
    {
        #region variables
        public string Id { get => _id; private set => _id = value; } // id of the input
        public InputType Type { get => _type; private set => _type = value; } // type of input   

        [SerializeField] private string _id;
        [SerializeField] private InputType _type;
        //Custom Inspector Validation (see: InputWrapperEditor)
        [SerializeField] private bool alphabet, digits, specialCharacters, canBeEmpty, capitalization, checkLength = false;
        [SerializeField] private bool mustBeOn, allCaps, unicode, canHaveSpace = true;
        [SerializeField] private int minLength, maxLength = 0;
        [SerializeField] private float minValue, maxValue = 0f;
        #endregion
        #region methods
        private void Awake()
        {
            Id = Id.Clean();
            if (String.IsNullOrWhiteSpace(Id)) Debug.LogError("Input id of: " + gameObject.name + ", is empty. Make sure to include only alphabet letters.");
        }

        /// <summary>
        /// Gets the value of the input field based on the specified input type.
        /// </summary>
        /// <returns>The value of the input field as a string, or null if the input field is not found or the input type is not recognized.</returns>
        public string GetValue()
        {
            // Define here your own input types
            switch (Type)
            {
                case InputType.Dropdown:
                    {
                        TMP_Dropdown input = GetComponent<TMP_Dropdown>();
                        if (input != null) return input.captionText.text;
                        break;
                    }
                case InputType.InputField:
                    {
                        TMP_InputField input = GetComponent<TMP_InputField>();
                        if (input != null) return input.text;
                        break;
                    }
                case InputType.Slider:
                    {
                        Slider input = GetComponent<Slider>();
                        if (input != null) return input.value.ToString();
                        break;
                    }
                case InputType.Toggle:
                    {
                        Toggle input = GetComponent<Toggle>();
                        if (input != null) return input.isOn.ToString();
                        break;
                    }
                default:
                    {
                        Debug.LogWarning("input type: " + Type + ", doesn't have a definition");
                        return null;
                    }
            }
            Debug.LogError("GameObject: " + gameObject.name + ", doesn't contain the specified input type: " + Type);
            return null;
        }

        /// <summary>
        /// Validates the input based on its type.
        /// </summary>
        /// <param name="validationResult">The validation result. If the input is valid, this will be set to "True". If the input is invalid, this will contain a tag describing the error.</param>
        /// <returns>True if the input is valid, false if the input is invalid.</returns>
        public bool ValidateInput(out string validationResult)
        {
            string input = GetValue();
            switch (Type)
            {
                case InputType.Toggle:
                    if (!mustBeOn)
                        break;

                    if (!Boolean.Parse(input))
                    {
                        validationResult = "NOTON";
                        return false;
                    }
                    break;
                case InputType.Dropdown:
                    // No validation for a dropdown, might be added in future updates
                    break;
                case InputType.Slider:
                    if(minValue == float.MinValue && maxValue == float.MaxValue)
                        break;
                    if (!InputValidate.CheckRange(float.Parse(input), minValue, maxValue, true))
                    {
                        validationResult = "NOTRANGE";
                        return false;
                    }
                    break;
                case InputType.InputField:
                    return ValidateInputfield(input, out validationResult);
                default:
                    Debug.LogWarning("Validation for this input type has not been defined");
                    break;
            }

            validationResult = "True";
            return true;
        }

        /// <summary>
        /// Validates the input based on its type.
        /// </summary>
        /// <returns>True if the input is valid, false if the input is invalid.</returns>
        public bool ValidateInput()
        {
            return ValidateInput(out string validationResult);
        }

        /// <summary>
        /// Validates the input field based on the specified validation rules.
        /// </summary>
        /// <param name="input">The input field to validate.</param>
        /// <param name="validationResult">The validation result. If the input is valid, this will be set to "True". If the input is invalid, this will contain a tag describing the error.</param>
        /// <returns>True if the input field is valid, false otherwise.</returns>
        private bool ValidateInputfield(string input, out string validationResult)
        {
            validationResult = "True";

            if (checkLength && !InputValidate.CheckLength(input, minLength, maxLength))
            {
                validationResult = "NOTLENGTH";
                return false;
            }

            if (capitalization)
            {
                bool isValidCase = allCaps ? InputValidate.UpperCaseOnly(input) : InputValidate.LowerCaseOnly(input);
                if (!isValidCase)
                {
                    validationResult = allCaps ? "NOTUPPERCASE" : "NOTLOWERCASE";
                    return false;
                }
            }

            if (String.IsNullOrEmpty(input))
            {
                if (!canBeEmpty)
                {
                    validationResult = "ISEMPTY";
                    return false;
                }
            }
            else
            {
                if(input.Contains(" ") && !canHaveSpace)
                {
                    validationResult = "HASSPACE";
                    return false;
                }
                
                if (alphabet && InputValidate.HasAlphabetLetters(input))
                {
                    validationResult = "HASALPHABET";
                    return false;
                }

                if (digits && InputValidate.HasNumbers(input))
                {
                    validationResult = "HASNUMBER";
                    return false;
                }

                if (specialCharacters)
                {
                    bool hasSpecialChars = unicode ? InputValidate.HasSpecialCharactersUnicode(input) : InputValidate.HasSpecialCharacters(input);
                    if (hasSpecialChars)
                    {
                        validationResult = "HASSPECIALCHAR";
                        return false;
                    }
                }
            }

            return true;
        }
        #endregion
    }
}