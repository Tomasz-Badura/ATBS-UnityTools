using System.Collections.Generic;
using ATBS.Extensions;
using UnityEngine;

namespace ATBS.MenuSystem
{
    [CreateAssetMenu(menuName = "MenuSystem/InputForm")]
    public class InputForm : ScriptableObject
    {
        [SerializeField] List<FormInputData> inputs;

        public List<SaveableFormInputData> SaveInputs()
        {
            List<SaveableFormInputData> result = new();
            foreach (FormInputData input in inputs)
                result.Add(input.Save());

            return result;
        }

        public void LoadInputs(List<SaveableFormInputData> saved)
        {
            foreach (FormInputData input in inputs)
            {
                SaveableFormInputData save = saved.Find(save => save.id.Clean() == input.Id.Clean());
                if (save == null)
                {
                    Debug.LogWarning("couldn't find saved form input for " + input);
                    continue;
                }
                input.Load(save);
            }
        }

        public void SetAllToDefault()
        {
            foreach (FormInputData input in inputs)
                input.SetCurrentToDefault();
        }

        public bool ValidateForm()
        {
            foreach (FormInputData input in inputs)
            {
                if (!input.Validate()) return false;
            }
            return true;
        }

        public FormInputData GetFormInputData(string id) => inputs.Find(input => input.Id.Clean() == id.Clean());
        
        public string GetFormInputString(string id)
        {
            FormInputData input = inputs.Find(input => input.Id.Clean() == id.Clean());
            if (input == null)
            {
                Debug.LogWarning("Couldn't find form input with id: " + id);
                return string.Empty;
            }
            return input.CurrentInput;
        }
    }
}