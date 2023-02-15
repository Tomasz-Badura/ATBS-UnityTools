using System.Collections.Generic;
using UnityEngine;

namespace ATBS.MenuSystem
{
    [CreateAssetMenu(menuName = "MenuSystem/FormInputData")]
    public class FormInputData : ScriptableObject
    {
        #region Variables
        [field: SerializeField] public string Id { get; private set; }
        [field: SerializeField] public string DefaultInput {get; private set;}
        [field: SerializeField] public string CurrentInput {get; set;}
        [field: SerializeField] public List<ValidationMethod> Validations { get; private set; }
        #endregion

        #region Events
        public delegate string FormInputHandler();
        public delegate void FormWrapperHandler();
        public delegate void FormValidationHandler(ValidationCode validationCode);
        public event FormWrapperHandler OnInputChanged;
        public event FormValidationHandler OnValidationError;
        #endregion

        #region Methods
        public bool Validate()
        {
            foreach (ValidationMethod validation in Validations)
            {
                if(!validation.Validate(CurrentInput)) 
                {
                    OnValidationError?.Invoke(validation.ValidationCode);
                    return false;
                }
            }
            return true;
        }

        public void SetCurrentToDefault()
        {
            CurrentInput = DefaultInput;
            OnInputChanged?.Invoke();
        }

        public void Load(SaveableFormInputData data)
        {
            Id = data.id;
            CurrentInput = data.currentInput;
            DefaultInput = data.defaultInput;
            OnInputChanged?.Invoke();    
        }   

        public SaveableFormInputData Save()
        {
            return new SaveableFormInputData(Id, CurrentInput, DefaultInput);
        }

        [ContextMenu("Set current input as the default.")]
        private void SetCurrentAsDefault()
        {
            DefaultInput = CurrentInput;
        }
        #endregion
    }

    [System.Serializable]
    public class SaveableFormInputData
    {
        public string id;
        public string currentInput;
        public string defaultInput;
        public SaveableFormInputData(string id, string currentInput, string defaultInput)
        {
            this.id = id;
            this.currentInput = currentInput;
            this.defaultInput = defaultInput;
        }
    }
}